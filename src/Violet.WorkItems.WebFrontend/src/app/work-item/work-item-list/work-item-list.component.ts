import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { DescriptorManagerService } from '../descriptor-manager.service';
import { WorkItem, WorkItemProperty, WorkItemService } from '../work-item.service';

@Component({
  selector: 'vwi-work-item-list',
  templateUrl: './work-item-list.component.html',
  styleUrls: ['./work-item-list.component.css']
})
export class WorkItemListComponent implements OnInit {
  @Input() projectCode: string;
  @Output() selected = new EventEmitter<WorkItem>();
  workItems: WorkItem[];
  displayedColumns: string[] = ["id", "type", "title"]

  constructor(private workItemService: WorkItemService, private descriptorManagerService: DescriptorManagerService) { }

  ngOnInit(): void {
    this.workItemService.getWorkItems(this.projectCode, "").subscribe(items => {
      console.log(items);
      this.workItems = items;
    });
  }

  getProperty(workItem: WorkItem, name: string): WorkItemProperty {
    return workItem.properties.filter(p => p.name == name)[0];
  }

  onSelection(workItem: WorkItem): void {
    this.selected.emit(workItem);
  }
}
