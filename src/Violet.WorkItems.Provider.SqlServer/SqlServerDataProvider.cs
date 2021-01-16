using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Violet.WorkItems.Provider.SqlServer;

namespace Violet.WorkItems.Provider
{
    public class SqlServerDataProvider : IDataProvider
    {
        public bool AllowLogRewrite { get; } = false;

        private readonly WorkItemDbContext _context;

        public SqlServerDataProvider(string connectionString)
        {
            _context = new WorkItemDbContext(connectionString);
        }

        public bool Read => true;
        public bool Write => true;

        public async Task InitAsync()
        {
            await _context.Database.MigrateAsync();
        }

        public async Task DeleteAsync()
        {
            await _context.Database.EnsureDeletedAsync(); ;
        }

        public async Task SaveNewWorkItemAsync(WorkItem workItem)
        {
            await _context.WorkItems.AddAsync(workItem);

            await _context.SaveChangesAsync();
        }

        public async Task SaveUpdatedWorkItemAsync(WorkItem workItem)
        {
            // stupid: https://github.com/aspnet/EntityFrameworkCore/issues/11457

            var trackedOne = await GetAsync(workItem.ProjectCode, workItem.Id);

            foreach (var property in workItem.Properties)
            {
                trackedOne.Properties.First(p => p.Name == property.Name).Value = property.Value;
            }

            var log = new HashSet<LogEntry>(trackedOne.Log);

            var lastLogEntryDate = (log.Count() == 0) ? DateTimeOffset.MinValue : log.Max(l => l.Date);

            foreach (var logEntry in workItem.Log.Where(l => l.Date > lastLogEntryDate))
            {
                log.Add(logEntry);
            }

            trackedOne.Log = log;

            await _context.SaveChangesAsync();
        }


        public Task<WorkItem> GetAsync(string projectCode, string id)
        {
            var item = _context.WorkItems.FirstOrDefault(wi => wi.ProjectCode == projectCode && wi.Id == id);

            return Task.FromResult(item);
        }

        public Task<IEnumerable<WorkItem>> ListWorkItemsAsync(string project, string type = null)
        {
            var items = _context.WorkItems.Where(x =>
                x.ProjectCode == project &&
                (string.IsNullOrWhiteSpace(type) || x.WorkItemType == type)
            ).OrderBy(wi => wi.Id);

            return Task.FromResult((IEnumerable<WorkItem>)items);
        }

        // TODO: Fix This. Change it to sequences per project.
        public Task<int> NextNumberAsync(string projectCode)
        {
            var number = _context.WorkItems
                .Where(wi => wi.ProjectCode == projectCode)
                .Select(wi => wi.Id)
                .ToList()
                .Union(new string[] { "0" })
                .Select(nr => int.Parse(nr))
                .Max() + 1;

            return Task.FromResult(number);
        }

        public IEnumerable<QueryError> ValidateQuery(Query query)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult> QueryWorkItemsAsync(Query query)
        {
            throw new NotImplementedException();
        }
    }
}
