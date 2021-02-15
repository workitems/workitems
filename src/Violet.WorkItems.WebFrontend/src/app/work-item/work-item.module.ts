import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table'

import { WorkItemDetailComponent } from './work-item-detail/work-item-detail.component';
import { SingleLineTextPropertyComponent } from './properties/single-line-text-property.component';
import { WorkItemPropertyComponent } from './properties/work-item-property.component';
import { WorkItemPropertyDirective } from './properties/work-item-property.directive';
import { WorkItemListComponent } from './work-item-list/work-item-list.component';
import { FormsModule } from '@angular/forms';
import { MatSortModule } from '@angular/material/sort';

@NgModule({
  declarations: [
    WorkItemDetailComponent,
    SingleLineTextPropertyComponent,
    WorkItemPropertyComponent,
    WorkItemPropertyDirective,
    WorkItemListComponent
  ],
  imports: [
    CommonModule,
    FormsModule,

    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatPaginatorModule,
    MatSortModule,
    MatTableModule
  ],
  exports: [
    WorkItemDetailComponent,
    WorkItemListComponent
  ]
})
export class WorkItemModule { }
