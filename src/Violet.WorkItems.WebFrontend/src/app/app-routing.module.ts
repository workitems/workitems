import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WorkItemDetailPageComponent } from './work-item-detail-page/work-item-detail-page.component';
import { WorkItemListPageComponent } from './work-item-list-page/work-item-list-page.component';

const routes: Routes = [
  {
    path: 'wi/search', component: WorkItemListPageComponent, data: {
      "mode": "Search"
    }
  },
  {
    path: 'wi/:projectCode/new', component: WorkItemDetailPageComponent, data: {
      "mode": "Creation"
    }
  },
  {
    path: 'wi/:projectCode/:workItemId', component: WorkItemDetailPageComponent, data: {
      "mode": "Editing"
    }
  },
  { path: 'wi/:projectCode', component: WorkItemListPageComponent },

];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
