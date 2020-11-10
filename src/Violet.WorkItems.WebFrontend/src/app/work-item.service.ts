import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';

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
  workItem?: WorkItem;
  errors: ErrorMessage[];
}

@Injectable({
  providedIn: 'root'
})
export class WorkItemService {

  private baseUri: string = 'https://localhost:5001/';

  constructor(private httpClient: HttpClient) { }

  createTemplate(projectCode: string, workItemType: string): Observable<WorkItem> {
    const uri = this.baseUri + 'api/v1/projects/' + projectCode + '/types/' + workItemType + '/new';

    return this.httpClient.get<any>(uri).pipe(
      map(response => response.workItem as WorkItem)
    );
  }

  getWorkItems(projectCode: string, filter: string): Observable<WorkItem[]> {
    return null;
  }

  getWorkItem(projectCode: string, id: string): Observable<WorkItem> {
    // let queryResult = this.data.filter(wi => wi.projectCode == projectCode && wi.id == id);

    // let result = queryResult[0];

    // return of(result);
    return null;
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
    return null;
  }
}
