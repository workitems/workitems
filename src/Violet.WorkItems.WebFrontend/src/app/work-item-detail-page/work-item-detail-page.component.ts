import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BladeStackComponent } from '../blades/blade-stack.component';
import { WorkItemDetailBladeComponent } from '../work-item-detail-blade/work-item-detail-blade.component';

@Component({
  template: `
    <blade-host>
      <vwi-work-item-nav></vwi-work-item-nav>
      <blade-stack #stack></blade-stack>
    </blade-host>
  `,
  styles: []
})
export class WorkItemDetailPageComponent implements OnInit, AfterViewInit {
  mode: string;
  projectCode: string;
  workItemId: string;
  workItemType: string;

  @ViewChild('stack') stack: BladeStackComponent;

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.data.subscribe(d => {
      this.mode = d["mode"] ?? "Creation";
      console.log(this.mode);
    });
    this.route.paramMap.subscribe(pm => {
      this.workItemId = pm.get("workItemId");
      this.projectCode = pm.get("projectCode");
    });
    this.route.queryParamMap.subscribe(qp => {
      this.workItemType = qp.get("type") ?? "Bug"; // TODO: Default Bug
    });
  }


  ngAfterViewInit(): void {
    const componentRef = this.stack.addBladeElementWithContent('workitem-' + this.projectCode + '-' + this.workItemId, WorkItemDetailBladeComponent, content => {
      content.projectCode = this.projectCode;
      content.mode = this.mode;
      content.workItemId = this.workItemId;
      content.workItemType = this.workItemType;
    });

  }

}
