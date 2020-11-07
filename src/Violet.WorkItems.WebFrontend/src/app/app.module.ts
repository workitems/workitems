import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { ButtonModule } from 'primeng/button';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { CheckboxModule } from 'primeng/checkbox';
import { InputTextModule } from 'primeng/inputtext';
import { SplitButtonModule } from 'primeng/splitbutton';

import { WorkItemDetailComponent } from './work-item-detail/work-item-detail.component';
import { SingleLineTextPropertyComponent } from './properties/single-line-text-property.component';
import { WorkItemPropertyComponent } from './properties/work-item-property.component';
import { WorkItemPropertyDirective } from './properties/work-item-property.directive';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    WorkItemDetailComponent,
    SingleLineTextPropertyComponent,
    WorkItemPropertyComponent,
    WorkItemPropertyDirective
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,

    ButtonModule,
    BreadcrumbModule,
    CheckboxModule,
    InputTextModule,
    SplitButtonModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
