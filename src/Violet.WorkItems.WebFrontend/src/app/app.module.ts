import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { OAuthModule } from 'angular-oauth2-oidc';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

import { WorkItemModule } from './work-item/work-item.module';

import { WorkItemListPageComponent } from './work-item-list-page/work-item-list-page.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { WorkItemNavComponent } from './work-item-nav/work-item-nav.component';
import { BladeElementComponent } from './blades/blade-element.component';
import { BladeStackComponent } from './blades/blade-stack.component';
import { BladeHostComponent } from './blades/blade-host.component';
import { WorkItemListBladeComponent } from './work-item-list-blade/work-item-list-blade.component';
import { WorkItemDetailBladeComponent } from './work-item-detail-blade/work-item-detail-blade.component';
import { WorkItemDetailPageComponent } from './work-item-detail-page/work-item-detail-page.component';

@NgModule({
  declarations: [
    AppComponent,
    WorkItemListPageComponent,
    WorkItemNavComponent,
    BladeElementComponent,
    BladeStackComponent,
    BladeHostComponent,
    WorkItemListBladeComponent,
    WorkItemDetailBladeComponent,
    WorkItemDetailPageComponent,
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

    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
