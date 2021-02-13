import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BladeStackComponent } from '../blades/blade-stack.component';
import { WorkItemDetailBladeComponent } from '../work-item-detail-blade/work-item-detail-blade.component';

@Component({
  selector: 'app-work-item-detail-page',
  templateUrl: './work-item-detail-page.component.html',
  styleUrls: ['./work-item-detail-page.component.css']
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
    const componentRef = this.stack.addBladeElementWithContent(WorkItemDetailBladeComponent);

    componentRef.instance.bladeComponent.projectCode = this.projectCode;
    componentRef.instance.bladeComponent.mode = this.mode;
    componentRef.instance.bladeComponent.workItemId = this.workItemId;
    componentRef.instance.bladeComponent.workItemType = this.workItemType;
  }

}
