import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkItemListPageComponent } from './work-item-list-page.component';

describe('WorkItemListPageComponent', () => {
  let component: WorkItemListPageComponent;
  let fixture: ComponentFixture<WorkItemListPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WorkItemListPageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkItemListPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
