import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { WorkItem } from '../work-item/work-item.service';

@Component({
  selector: 'app-work-item-page',
  templateUrl: './work-item-page.component.html',
  styleUrls: ['./work-item-page.component.css']
})
export class WorkItemPageComponent implements OnInit {
  mode: string;
  projectCode: string;
  workItemType: string;
  workItemId: string;

  items: MenuItem[];
  home: MenuItem;

  constructor(
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.items = [];
    this.home = { icon: 'pi pi-home', routerLink: '/' };

    this.route.data.subscribe(d => {
      this.mode = d["mode"];
    });
    this.route.paramMap.subscribe(pm => {
      this.workItemId = pm.get("workItemId");
      this.projectCode = pm.get("projectCode");

      this.items = [
        { label: this.projectCode, url: '/wi/' + this.projectCode },
        { label: this.workItemId, url: '/wi/' + this.projectCode + '/' + this.workItemId }
      ];
    });
    this.route.queryParamMap.subscribe(qp => {
      this.workItemType = qp.get("type") ?? "Bug"; // TODO: Default Bug
    })
  }

  onClosed() {
    this.router.navigate(["wi", this.projectCode]);
  }
  onCompleted(event: WorkItem) {
    this.router.navigate(["wi", event.projectCode, event.id]);
  }
}
