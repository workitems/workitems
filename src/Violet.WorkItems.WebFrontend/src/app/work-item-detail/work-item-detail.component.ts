import { Component, Input, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { DescriptorManagerService, WorkItemCommandDescriptor, WorkItemPropertyDescriptor } from '../descriptor-manager.service';
import { WorkItem, WorkItemService } from '../work-item.service';

@Component({
  selector: 'vwi-work-item-detail',
  templateUrl: './work-item-detail.component.html',
  styleUrls: ['./work-item-detail.component.css']
})
export class WorkItemDetailComponent implements OnInit {

  @Input() projectCode: string;
  @Input() id: string;

  items: MenuItem[];
  home: MenuItem;

  workItem: WorkItem;
  propertyDescriptors: WorkItemPropertyDescriptor[];
  commandDescriptors: WorkItemCommandDescriptor[];

  constructor(private workItemService: WorkItemService, private descriptorManagerService: DescriptorManagerService) { }

  ngOnInit(): void {
    this.items = [];
    this.home = { icon: 'pi pi-home', routerLink: '/' };

    this.workItemService.getWorkItem(this.projectCode, this.id)
      .subscribe(wi => {
        this.descriptorManagerService.getCurrentPropertyDescriptors(wi).subscribe(descriptors => {
          this.workItem = wi;
          this.propertyDescriptors = descriptors;

          this.items = [
            { label: wi.projectCode, url: '/wi/' + wi.projectCode },
            { label: wi.id, url: '/wi/' + wi.projectCode + '/' + wi.id }
          ];
        });

        this.descriptorManagerService.getCurrentCommands(wi).subscribe(commands => {
          this.commandDescriptors = commands;
        });
      });
  }

  save(): void { }

}
