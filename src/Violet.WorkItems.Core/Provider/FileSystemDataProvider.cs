using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Violet.WorkItems.Provider;

public class FileSystemDataProvider : IDataProvider
{
    private readonly string _basePath;

    public bool Read => true;
    public bool Write => true;

    public FileSystemDataProvider(string connectionString)
    {
        _basePath = connectionString;
    }
    public const string WorkItemExtension = ".workitem.json";
    private string GetWorkItemPath(string projectCode, string id)
        => Path.Combine(_basePath, projectCode, id + WorkItemExtension);
    private string GetProjectPath(string projectCode)
        => Path.Combine(_basePath, projectCode);
    private void EnsureProjectLocation(string projectCode)
    {
        var path = GetProjectPath(projectCode);

        Directory.CreateDirectory(path);
    }

    public async Task<WorkItem?> GetAsync(string projectCode, string id)
    {
        var fileInfo = new FileInfo(GetWorkItemPath(projectCode, id));

        if (fileInfo.Exists)
        {
            return await ReadWorkItemByFileInfoAsync(fileInfo);
        }
        else
        {
            return null;
        }
    }

    private static async Task<WorkItem?> ReadWorkItemByFileInfoAsync(FileInfo fileInfo)
    {
        using var stream = fileInfo.OpenRead();
        var workItem = await JsonSerializer.DeserializeAsync<WorkItem>(stream);
        stream.Close();

        return workItem;
    }
    private static async Task WriteWorkItemByFileInfoAsync(FileInfo fileInfo, WorkItem workItem)
    {
        using var stream = fileInfo.OpenWrite();
        await JsonSerializer.SerializeAsync(stream, workItem);
        stream.Flush();
        stream.Close();
    }

    public async Task<IEnumerable<WorkItem>> ListWorkItemsAsync(string projectCode, string? workItemType = null)
    {
        EnsureProjectLocation(projectCode);
        var directoryInfo = new DirectoryInfo(GetProjectPath(projectCode));

        var allFiles = directoryInfo.GetFiles("*" + WorkItemExtension);

        var result = new List<WorkItem>();
        foreach (var file in allFiles)
        {
            var workItem = await ReadWorkItemByFileInfoAsync(file);

            if (workItem is not null)
            {
                result.Add(workItem);
            }
        }

        return result;
    }

    public Task<int> NextNumberAsync(string projectCode)
    {
        EnsureProjectLocation(projectCode);
        var directoryInfo = new DirectoryInfo(GetProjectPath(projectCode));

        var highestNumber = directoryInfo.GetFiles()
            .Select(fi => fi.Name
                [..^WorkItemExtension.Length])
            .Select(nr => Convert.ToInt32(nr))
            .OrderByDescending(nr => nr)
            .FirstOrDefault(0);

        return Task.FromResult(highestNumber + 1);
    }

    public async Task SaveNewWorkItemAsync(WorkItem workItem)
    {
        var path = GetWorkItemPath(workItem.ProjectCode, workItem.Id);

        var fileInfo = new FileInfo(path);

        await WriteWorkItemByFileInfoAsync(fileInfo, workItem);
    }

    public Task SaveUpdatedWorkItemAsync(WorkItem workItem)
        => SaveNewWorkItemAsync(workItem);
}