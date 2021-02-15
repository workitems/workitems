import { Component, Input, OnInit, Optional, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { BladeElementComponent } from '../blades/blade-element.component';
import { BladeStackComponent } from '../blades/blade-stack.component';
import { WorkItemListBladeComponent } from '../work-item-list-blade/work-item-list-blade.component';
import { WorkItemDetailComponent } from '../work-item/work-item-detail/work-item-detail.component';
import { WorkItem } from '../work-item/work-item.service';

@Component({
  templateUrl: './work-item-detail-blade.component.html',
  styles: [
    `:host { min-width:400px; }`
  ]
})
export class WorkItemDetailBladeComponent implements OnInit {
  @ViewChild("detailCreation") detailCreation: WorkItemDetailComponent;
  @ViewChild("detailEditing") detailEditing: WorkItemDetailComponent;

  @Input() mode: string;
  @Input() projectCode: string;
  @Input() workItemType: string;
  @Input() workItemId: string;

  constructor(private router: Router, @Optional() private stack?: BladeStackComponent, @Optional() private bladeElement?: BladeElementComponent<WorkItemDetailBladeComponent>) { }

  ngOnInit(): void {
  }

  onCompleted(event: WorkItem) {
    //this.router.navigate(["wi", event.projectCode, event.id]);
    this.bladeElement.close();
  }

  gotoProject(): void {
    const componentRef = this.stack.addBladeElementWithContent('project-list-' + this.projectCode, WorkItemListBladeComponent, content => {
      content.projectCode = this.projectCode;
    });
  }

  save(): void {
    if (this.mode == "Creation") {
      this.detailCreation.save();
    } else {
      this.detailEditing.save();
    }
  }
}
