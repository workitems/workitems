import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkItemPropertyComponent } from './work-item-property.component';

describe('WorkItemPropertyComponent', () => {
  let component: WorkItemPropertyComponent;
  let fixture: ComponentFixture<WorkItemPropertyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WorkItemPropertyComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkItemPropertyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
