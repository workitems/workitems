import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { WorkItemCommandDescriptor } from './descriptor-manager.service';

export interface WorkItemProperty {
  name: string;
  dataType: string;
  value: string;
}

export interface WorkItem {
  projectCode: string;
  id: string;
  workItemType: string;

  properties: WorkItemProperty[];
}

export interface ErrorMessage {
  projectCode: string;
  id: string;
  property: string;
  source: string;
  errorCode: string;
  message: string;
}

export interface WorkItemUpdatedResult {
  success: boolean;
  projectCode: string;
  id: string;
  workItem?: WorkItem;
  errors: ErrorMessage[];
}

export interface WorkItemResponse {
  success: boolean;
  projectCode: string;
  workItemId: string;
  workItem?: WorkItem;
}

export interface WorkItemListApiResponse {
  success: boolean;
  workItems: WorkItem[];
}

@Injectable({
  providedIn: 'root'
})
export class WorkItemService {

  private baseUri: string = 'https://localhost:5001/';

  constructor(private httpClient: HttpClient) { }

  createTemplate(projectCode: string, workItemType: string): Observable<WorkItem> {
    const uri = this.baseUri + 'api/v1/projects/' + projectCode + '/types/' + workItemType + '/new';

    return this.httpClient.get<WorkItemResponse>(uri).pipe(
      map(response => response.workItem as WorkItem)
    );
  }

  getWorkItems(projectCode: string, filter: string): Observable<WorkItem[]> {
    const uri = this.baseUri + 'api/v1/projects/' + projectCode + '/workitems';

    return this.httpClient.get<WorkItemListApiResponse>(uri).pipe(
      map(response => response.workItems as WorkItem[])
    );
  }

  getWorkItem(projectCode: string, id: string): Observable<WorkItem> {
    const uri = this.baseUri + 'api/v1/projects/' + projectCode + '/workitems/' + id;

    return this.httpClient.get<WorkItemResponse>(uri).pipe(
      map(response => response.workItem)
    );
  }

  createWorkItem(projectCode: string, workItemType: string, properties: WorkItemProperty[]): Observable<WorkItemUpdatedResult> {
    const uri = this.baseUri + 'api/v1/projects/' + projectCode + '/workitems';

    const request = {
      projectCode: projectCode,
      workItemType: workItemType,
      properties: properties,
    };

    return this.httpClient.post<any>(uri, request).pipe(
      map(response => response as WorkItemUpdatedResult)
    );
  }

  saveChanges(projectCode: string, id: string, properties: WorkItemProperty[]): Observable<WorkItemUpdatedResult> {
    const uri = this.baseUri + 'api/v1/projects/' + projectCode + '/workitems/' + id;

    const request = {
      projectCode: projectCode,
      workItemId: id,
      comment: "Web App Change",
      properties: properties
    };

    return this.httpClient.post<WorkItemUpdatedResult>(uri, request);
  }

  executeCommand(projectCode: string, id: string, commandName: string): Observable<WorkItemUpdatedResult> {
    const uri = this.baseUri + 'api/v1/projects/' + projectCode + '/workitems/' + id + '/commands';

    const request = {
      projectCode: projectCode,
      workItemId: id,
      command: commandName,
    };

    return this.httpClient.post<WorkItemUpdatedResult>(uri, request);
  }
}
