import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WorkItemListPageComponent } from './work-item-list-page/work-item-list-page.component';
import { WorkItemPageComponent } from './work-item-page/work-item-page.component';

const routes: Routes = [
  {
    path: 'wi/:projectCode/new', component: WorkItemPageComponent, data: {
      "mode": "new"
    }
  },
  {
    path: 'wi/:projectCode/:workItemId', component: WorkItemPageComponent, data: {
      "mode": "edit"
    }
  },
  { path: 'wi/:projectCode', component: WorkItemListPageComponent },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
