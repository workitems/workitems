import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { WorkItem } from '../work-item/work-item.service';

@Component({
  selector: 'app-work-item-list-page',
  templateUrl: './work-item-list-page.component.html',
  styleUrls: ['./work-item-list-page.component.css']
})
export class WorkItemListPageComponent implements OnInit {
  projectCode: string;

  constructor(
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(pm => {
      this.projectCode = pm.get("projectCode");
    });
  }

  onSelected(workItem: WorkItem): void {
    this.router.navigate(["wi", workItem.projectCode, workItem.id]);
  }

  new(): void {
    this.router.navigate(["wi", this.projectCode, "new"]);
  }

}
