import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { OAuthModule } from 'angular-oauth2-oidc';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { ButtonModule } from 'primeng/button';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { CheckboxModule } from 'primeng/checkbox';
import { InputTextModule } from 'primeng/inputtext';
import { SplitButtonModule } from 'primeng/splitbutton';
import { TableModule } from 'primeng/table';

import { WorkItemModule } from './work-item/work-item.module';

import { WorkItemPageComponent } from './work-item-page/work-item-page.component';
import { WorkItemListPageComponent } from './work-item-list-page/work-item-list-page.component';

@NgModule({
  declarations: [
    AppComponent,
    WorkItemPageComponent,
    WorkItemListPageComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    AppRoutingModule,
    HttpClientModule,
    WorkItemModule,

    OAuthModule.forRoot({
      resourceServer: {
        allowedUrls: ['https://localhost:5001/'],
        sendAccessToken: true
      }
    }),

    ButtonModule,
    BreadcrumbModule,
    CheckboxModule,
    InputTextModule,
    SplitButtonModule,
    TableModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
