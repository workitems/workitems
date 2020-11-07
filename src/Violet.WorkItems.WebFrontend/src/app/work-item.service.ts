import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

export interface WorkItemProperty {
  name: string;
  dataType: string;
  value: string;
}

export interface WorkItem {
  projectCode: string;
  id: string;
  workItemType: string;

  properties: { [name: string]: WorkItemProperty }
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
  updatedWorkItem?: WorkItem;
  errors: ErrorMessage[];
}

@Injectable({
  providedIn: 'root'
})
export class WorkItemService {

  private data: WorkItem[] = [
    {
      projectCode: "ACME",
      id: "ACME-1",
      workItemType: "Issue",
      properties: {
        "FirstName": {
          name: "FirstName",
          dataType: "String",
          value: null,
        },
        "LastName": {
          name: "LastName",
          dataType: "String",
          value: null,
        }
      }
    }
  ];

  constructor() { }

  getWorkItems(projectCode: string, filter: string): Observable<WorkItem[]> {
    return null;
  }

  getWorkItem(projectCode: string, id: string): Observable<WorkItem> {
    let queryResult = this.data.filter(wi => wi.projectCode == projectCode && wi.id == id);

    let result = queryResult[0];

    return of(result);
  }

  saveChanges(projectCode: string, id: string, properties: WorkItemProperty[]): Observable<WorkItemUpdatedResult> {
    return null;
  }
}
