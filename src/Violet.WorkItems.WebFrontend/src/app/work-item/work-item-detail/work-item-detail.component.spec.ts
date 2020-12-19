import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkItemDetailComponent } from './work-item-detail.component';

describe('WorkItemDetailComponent', () => {
  let component: WorkItemDetailComponent;
  let fixture: ComponentFixture<WorkItemDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WorkItemDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkItemDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
