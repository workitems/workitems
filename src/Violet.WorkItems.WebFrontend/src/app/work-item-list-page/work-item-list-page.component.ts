import { AfterViewInit, Component, Input, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BladeStackComponent } from '../blades/blade-stack.component';
import { WorkItemListBladeComponent } from '../work-item-list-blade/work-item-list-blade.component';

@Component({
  template: `
    <blade-host>
      <vwi-work-item-nav></vwi-work-item-nav>
      <blade-stack #stack></blade-stack>
    </blade-host>
  `,
  styles: []
})
export class WorkItemListPageComponent implements OnInit, AfterViewInit {
  @Input() projectCode: string;
  @Input() mode: "ProjectSearch" | "Search" = "ProjectSearch";

  @ViewChild('stack') stack: BladeStackComponent;

  constructor(
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(pm => {
      this.projectCode = pm.get("projectCode");
    });
  }

  ngAfterViewInit(): void {
    const componentRef = this.stack.addBladeElementWithContent('project-list-' + this.projectCode, WorkItemListBladeComponent, content => {
      content.projectCode = this.projectCode;
    });
  }
}
