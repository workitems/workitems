import { Input, Optional } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { BladeStackComponent } from '../blades/blade-stack.component';
import { WorkItemDetailBladeComponent } from '../work-item-detail-blade/work-item-detail-blade.component';
import { WorkItemDetailComponent } from '../work-item/work-item-detail/work-item-detail.component';
import { WorkItem } from '../work-item/work-item.service';

@Component({
  selector: 'app-work-item-list-blade',
  templateUrl: './work-item-list-blade.component.html',
  styleUrls: ['./work-item-list-blade.component.css']
})
export class WorkItemListBladeComponent implements OnInit {
  @Input() projectCode: string;

  constructor(@Optional() private stack?: BladeStackComponent) { }

  ngOnInit(): void {
  }


  onSelected(workItem: WorkItem): void {
    const componentRef = this.stack.addBladeElementWithContent(WorkItemDetailBladeComponent);

    componentRef.instance.bladeComponent.mode = 'Editing';
    componentRef.instance.bladeComponent.projectCode = workItem.projectCode;
    componentRef.instance.bladeComponent.workItemId = workItem.id;
    // TODO completed event

    //this.router.navigate(["wi", workItem.projectCode, workItem.id]);
  }

  new(): void {
    const componentRef = this.stack.addBladeElementWithContent(WorkItemDetailBladeComponent);

    componentRef.instance.bladeComponent.mode = 'Creation';
    componentRef.instance.bladeComponent.projectCode = this.projectCode;
    componentRef.instance.bladeComponent.workItemType = 'Bug';

    //this.router.navigate(["wi", this.projectCode, "new"]);
  }

}
