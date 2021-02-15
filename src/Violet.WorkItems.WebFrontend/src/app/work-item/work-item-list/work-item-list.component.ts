import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DescriptorManagerService } from '../descriptor-manager.service';
import { WorkItem, WorkItemProperty, WorkItemService } from '../work-item.service';

@Component({
  selector: 'vwi-work-item-list',
  templateUrl: './work-item-list.component.html',
  styleUrls: ['./work-item-list.component.css']
})
export class WorkItemListComponent implements OnInit, AfterViewInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  @Input() projectCode: string;
  @Output() selected = new EventEmitter<WorkItem>();
  displayedColumns: string[] = ["id", "type", "title"];
  dataSource: MatTableDataSource<WorkItem>;

  constructor(private workItemService: WorkItemService, private descriptorManagerService: DescriptorManagerService) { }

  ngOnInit(): void {
    this.workItemService.getWorkItems(this.projectCode, "").subscribe(items => {
      this.dataSource = new MatTableDataSource(items);

      if (this.paginator != null) {
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      }
    });
  }
  ngAfterViewInit() {
    if (this.dataSource != null) {
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    }
  }

  getProperty(workItem: WorkItem, name: string): WorkItemProperty {
    return workItem.properties.filter(p => p.name == name)[0];
  }

  onSelection(workItem: WorkItem): void {
    this.selected.emit(workItem);
  }
}
