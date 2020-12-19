import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ButtonModule } from 'primeng/button';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { CheckboxModule } from 'primeng/checkbox';
import { InputTextModule } from 'primeng/inputtext';
import { SplitButtonModule } from 'primeng/splitbutton';
import { TableModule } from 'primeng/table';

import { WorkItemDetailComponent } from './work-item-detail/work-item-detail.component';
import { SingleLineTextPropertyComponent } from './properties/single-line-text-property.component';
import { WorkItemPropertyComponent } from './properties/work-item-property.component';
import { WorkItemPropertyDirective } from './properties/work-item-property.directive';
import { WorkItemListComponent } from './work-item-list/work-item-list.component';
import { FormsModule } from '@angular/forms';

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

    ButtonModule,
    BreadcrumbModule,
    CheckboxModule,
    InputTextModule,
    SplitButtonModule,
    TableModule
  ],
  exports: [
    WorkItemDetailComponent,
    WorkItemListComponent
  ]
})
export class WorkItemModule { }
