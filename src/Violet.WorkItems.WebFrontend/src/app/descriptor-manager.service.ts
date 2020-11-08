import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { WorkItem } from './work-item.service';

export interface WorkItemDescriptor {
  properties: WorkItemPropertyDescriptor[];
  commands: WorkItemCommandDescriptor[];
}

export interface WorkItemPropertyDescriptor {
  name: string;
  label: string;
  hint: string;
  description: string;
  dataType: string;
  propertyType: "SingleRaw" | "SingleValueFromProvider" | "MultipleValueFromProvider";
  widgetType: string;
  isVisible: boolean;
  isEditable: boolean;
}

export interface WorkItemCommandDescriptor {
  name: string;
  type: string;
  label: string;
}

@Injectable({
  providedIn: 'root'
})
export class DescriptorManagerService {

  constructor(private httpClient: HttpClient) { }

  getTemplateDescriptors(projectCode: string, workItemType: string): Observable<WorkItemDescriptor> {
    const uri = 'https://localhost:5001/api/v1/projects/' + projectCode + '/types/' + workItemType + '/descriptor';

    return this.httpClient.get<WorkItemDescriptor>(uri);
  }

  getCurrentDescriptor(workItem: WorkItem): Observable<WorkItemDescriptor> {
    const uri = 'https://localhost:5001/api/v1/projects/' + workItem.projectCode + '/workitems/' + workItem.id + '/descriptor';

    return this.httpClient.get<WorkItemDescriptor>(uri);
  }
}
