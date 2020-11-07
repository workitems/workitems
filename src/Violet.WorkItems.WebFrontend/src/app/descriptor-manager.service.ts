import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { WorkItem } from './work-item.service';

export interface WorkItemDescriptor {
  properties: PropertyDescriptor[];
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

  constructor() { }

  getCurrentPropertyDescriptors(workItem: WorkItem): Observable<WorkItemPropertyDescriptor[]> {

    let propertyDescriptors: WorkItemPropertyDescriptor[] = [
      {
        name: "FirstName",
        label: "Firstname",
        hint: "e.g. Thomas",
        description: null,
        dataType: "string",
        isEditable: true,
        isVisible: true,
        propertyType: "SingleRaw",
        widgetType: null
      },
      {
        name: "LastName",
        label: "Lastname",
        hint: "e.g. T.",
        description: null,
        dataType: "string",
        isEditable: true,
        isVisible: true,
        propertyType: "SingleRaw",
        widgetType: null
      },
    ];

    return of(propertyDescriptors);
  }

  getCurrentCommands(workItem: WorkItem): Observable<WorkItemCommandDescriptor[]> {
    let commandDescriptors: WorkItemCommandDescriptor[] = [
      {
        name: "foo",
        type: "foo",
        label: "Approve"
      },
      {
        name: "bar",
        type: "bar",
        label: "Na!"
      }
    ];

    return of(commandDescriptors);
  }
}
