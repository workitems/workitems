import { Component, Input, OnInit, Output, Query } from '@angular/core';
import { WorkItemQuery } from '../query-ast';

@Component({
  selector: 'vwi-work-item-search-box',
  templateUrl: './work-item-search-box.component.html',
  styleUrls: ['./work-item-search-box.component.css']
})
export class WorkItemSearchBoxComponent implements OnInit {
  @Input() @Output() query: WorkItemQuery;

  constructor() { }

  ngOnInit(): void {
  }

}
