import { Component, Input, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { DescriptorManagerService, WorkItemCommandDescriptor, WorkItemPropertyDescriptor } from '../descriptor-manager.service';
import { WorkItem, WorkItemProperty, WorkItemService } from '../work-item.service';

@Component({
  selector: 'vwi-work-item-detail',
  templateUrl: './work-item-detail.component.html',
  styleUrls: ['./work-item-detail.component.css']
})
export class WorkItemDetailComponent implements OnInit {

  @Input() projectCode: string = '';
  @Input() id: string = '';
  @Input() workItemType: string = '';

  mode: "Creation" | "Editing";

  items: MenuItem[];
  home: MenuItem;

  workItem: WorkItem;
  propertyDescriptors: WorkItemPropertyDescriptor[];
  commandDescriptors: WorkItemCommandDescriptor[];

  constructor(private workItemService: WorkItemService, private descriptorManagerService: DescriptorManagerService) { }

  ngOnInit(): void {
    this.items = [];
    this.home = { icon: 'pi pi-home', routerLink: '/' };

    this.mode = this.id == '' ? "Creation" : "Editing";

    if (this.mode == "Editing") {
      this.workItemService.getWorkItem(this.projectCode, this.id)
        .subscribe(wi => {
          this.descriptorManagerService.getCurrentDescriptor(wi)
            .subscribe(descriptors => {
              this.workItem = wi;
              this.propertyDescriptors = descriptors.properties;
              this.commandDescriptors = descriptors.commands;

              console.log(wi, descriptors);

              this.items = [
                { label: wi.projectCode, url: '/wi/' + wi.projectCode },
                { label: wi.id, url: '/wi/' + wi.projectCode + '/' + wi.id }
              ];
            });
        });
    } else if (this.mode == "Creation" && this.workItemType !== '') {
      this.workItemService.createTemplate(this.projectCode, this.workItemType)
        .subscribe(wi => {
          this.descriptorManagerService.getTemplateDescriptors(this.projectCode, this.workItemType)
            .subscribe(descriptors => {
              this.workItem = wi;
              this.propertyDescriptors = descriptors.properties;
              this.commandDescriptors = descriptors.commands;

              console.log(wi, descriptors);


              this.items = [
                { label: wi.projectCode, url: '/wi/' + wi.projectCode },
                { label: wi.id, url: '/wi/' + wi.projectCode + '/' + wi.id }
              ];
            });
        });
    }
  }

  save(): void {
    if (this.mode == "Editing") {

    } else if (this.mode == "Creation") {
      const properties = this.getPropertiesForCreation();
      this.workItemService.createWorkItem(this.workItem.projectCode, this.workItem.workItemType, properties).subscribe(wi => {
        console.log("Created: ", wi);

        this.id == wi.workItem.id;
        this.mode == "Editing";

        this.ngOnInit();

      });
    }
  }

  propertyWithName(propertyName: string): WorkItemProperty {
    return this.workItem.properties.filter(p => p.name == propertyName)[0]
  }

  getPropertiesForCreation(): WorkItemProperty[] {
    return this.workItem.properties.filter(p => this.propertyDescriptors.filter(pd => pd.name == p.name && pd.isEditable == true).length > 0);
  }
}
