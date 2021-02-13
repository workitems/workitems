import { Component, Input, OnInit, Optional } from '@angular/core';
import { Router } from '@angular/router';
import { BladeElementComponent } from '../blades/blade-element.component';
import { WorkItem } from '../work-item/work-item.service';

@Component({
  templateUrl: './work-item-detail-blade.component.html',
  styles: [
    `:host { min-width:400px; }`
  ]
})
export class WorkItemDetailBladeComponent implements OnInit {
  @Input() mode: string;
  @Input() projectCode: string;
  @Input() workItemType: string;
  @Input() workItemId: string;

  constructor(private router: Router, @Optional() private bladeElement?: BladeElementComponent<WorkItemDetailBladeComponent>) { }

  ngOnInit(): void {
  }

  onCompleted(event: WorkItem) {
    //this.router.navigate(["wi", event.projectCode, event.id]);
    this.bladeElement.close();
  }
}
