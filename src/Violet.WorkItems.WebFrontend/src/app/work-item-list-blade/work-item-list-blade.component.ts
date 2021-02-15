import { Input, Optional } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { BladeStackComponent } from '../blades/blade-stack.component';
import { WorkItemDetailBladeComponent } from '../work-item-detail-blade/work-item-detail-blade.component';
import { WorkItem } from '../work-item/work-item.service';

@Component({
  templateUrl: './work-item-list-blade.component.html',
  styles: [
    `:host { min-width:400px; }`
  ]
})
export class WorkItemListBladeComponent implements OnInit {
  @Input() projectCode: string;

  constructor(@Optional() private stack?: BladeStackComponent) { }

  ngOnInit(): void {
  }


  onSelected(workItem: WorkItem): void {
    const componentRef = this.stack.addBladeElementWithContent('workitem-' + workItem.projectCode + '-' + workItem.id, WorkItemDetailBladeComponent, content => {
      content.mode = 'Editing';
      content.projectCode = workItem.projectCode;
      content.workItemId = workItem.id;
      // TODO completed event
    });

  }

  new(): void {
    const componentRef = this.stack.addBladeElementWithContent('workitem-' + this.projectCode + '-NEW', WorkItemDetailBladeComponent, content => {
      content.mode = 'Creation';
      content.projectCode = this.projectCode;
      content.workItemType = 'Bug';
    });

  }

}
