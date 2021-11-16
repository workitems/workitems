# Pipelines

Purpose
- Extension Point to the core logic


## Use Cases

### DetailWorkItem Use Case

- Get Workitem (`WorkItemManager.GetAsync`) -> Pipeline
- Display

## ListWorkItems Use Case

- List Workteims (`workItemManager.DataProvider.ListWorkItemsAsync`) _> TODO Pipeline
- Display

### NewWorkItem Use Case

- Load Template (`WorkItemManager.CreateTemplateAsync`) -> Pipeline
- Get Current Property Descriptors (`WorkItemManager.DescriptorManager.GetCurrentPropertyDescriptors`) -> TODO Pipeline
- Get Available Values for dropdown / suggestions (`ValueProvider`) -> TODO Pipeline
- Edit stuff
- Create WorkItem (`WorkItemManager.CreateAsync`) -> Pipeline
- Emit Errors

### EditWorkItem Use Case

- Load WorkItem (`WorkItemManager.GetAsync`) -> Pipeline
- Get Current Property Descriptors (`WorkItemManager.DescriptorManager.GetCurrentPropertyDescriptors`) -> TODO Pipeline
- Get Available Values for dropdown / suggestions (`ValueProvider`) -> TODO Pipeline
- Edit stuff
- Update WorkItem (`WorkItemManager.UpdateAsync`) -> Pipeline
- Emit Errors

### ExecuteCommand Use Case
- Load WorkItem (`WorkItemManager.GetAsync`) -> Pipeline
- Get Current Property Descriptors (`WorkItemManager.DescriptorManager.GetCurrentPropertyDescriptors`) -> TODO Pipeline
- Display Command List
- Click a command
- Execute Command (`WorkItemManager.ExecuteCommandAsync`) -> Pipeline
- Emit Errors

## Pipelines
### Load Template Pipeline

1. Identity Check/Contribution
1. Authorization by Project
1. Core Processing
   1. Load Property Descriptors (`DescriptorManager.GetAllPropertyDescriptors`)
   1. Instantiate WorkItem based on descriptors

### Get WorkItem Pipeline

1. Identity Check/Contribution
1. Authorization by Project
1. Core Processing
   1. Load from valid source (@ `WorkItemManager.GetAsync`)

### Create Pipeline

1. Identity Check/Contribution
1. Authorization by Project
1. Core Processing
   1. Instantiate Template & Merge suggested WorkItem (@ `WorkItemManager.CreateAsync`)
1. Validation (@ `WorkItemManager.CreateAsync`)
1. Persistence (@ `WorkItemManager.CreateAsync`)
   1. Create New Identifier (@ `WorkItemManager.CreateAsync`)
1. Notify Other Users

### Execute Command Pipeline

1. Identity Check/Contribution
1. Authorization by Project
1. Core Processing (Command)
   1. Load from valid source (@ `WorkItemManager.ExecuteCommandAsync`)
   1. Execute and side-effect PropertyChanges (@ `WorkItemManager.ExecuteCommandAsync`)
1. Core Processing (Update) (@ `WorkItemManager.UpdateAsync`)
1. Validation (@ `WorkItemManager.UpdateAsync`)
1. Persistence (@ `WorkItemManager.UpdateAsync`)
1. Notify Other Users

### Update Pipeline

1. Identity Check/Contribution
1. Authorization by Project
1. Core Processing (Update)
   1. Load from valid source (@ `WorkItemManager.UpdateAsync`)
   1. Analyze Changes & Merge suggested WorkItem (@ `WorkItemManager.UpdateAsync`)
   1. ApplyChanges to valid record (@ `WorkItemManager.UpdateAsync`)
   1. Extend Log with changes (@ `WorkItemManager.UpdateAsync`)
1. Validation (@ `WorkItemManager.UpdateAsync`)
1. Persistence (@ `WorkItemManager.UpdateAsync`)
1. Notify Other Users