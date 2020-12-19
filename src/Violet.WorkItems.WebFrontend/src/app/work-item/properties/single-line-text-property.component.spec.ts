import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SingleLineTextPropertyComponent } from './single-line-text-property.component';

describe('SingleLineTextPropertyComponent', () => {
  let component: SingleLineTextPropertyComponent;
  let fixture: ComponentFixture<SingleLineTextPropertyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SingleLineTextPropertyComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SingleLineTextPropertyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
