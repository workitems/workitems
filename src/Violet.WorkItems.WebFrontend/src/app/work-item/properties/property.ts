
import { WorkItemDescriptor, WorkItemPropertyDescriptor } from '../descriptor-manager.service';
import { WorkItemProperty } from '../work-item.service';

export interface PropertyComponent {
    workItemDescriptor: WorkItemDescriptor;
    propertyDescriptor: WorkItemPropertyDescriptor;

    propertyValue: WorkItemProperty;
}