import { AfterViewInit, Component, Input, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BladeStackComponent } from '../blades/blade-stack.component';
import { WorkItemListBladeComponent } from '../work-item-list-blade/work-item-list-blade.component';

@Component({
  selector: 'app-work-item-list-page',
  templateUrl: './work-item-list-page.component.html',
  styleUrls: ['./work-item-list-page.component.css']
})
export class WorkItemListPageComponent implements OnInit, AfterViewInit {
  @Input() projectCode: string;

  @ViewChild('stack') stack: BladeStackComponent;

  constructor(
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(pm => {
      this.projectCode = pm.get("projectCode");
    });
  }

  ngAfterViewInit(): void {
    const componentRef = this.stack.addBladeElementWithContent(WorkItemListBladeComponent);

    componentRef.instance.bladeComponent.projectCode = this.projectCode;
  }
}
