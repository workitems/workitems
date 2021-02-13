import { Component, Input, OnInit } from '@angular/core';
import { WorkItemDescriptor, WorkItemPropertyDescriptor } from '../descriptor-manager.service';
import { WorkItemProperty } from '../work-item.service';
import { PropertyComponent } from './property';

@Component({
  template: `
    <div style="margin-bottom:10px">
      <mat-form-field appearance="standard" style="width:100%;">
        <mat-label>{{propertyDescriptor.label}}</mat-label>
        <input matInput placeholder="{{propertyDescriptor.hint}}" [(ngModel)]="propertyValue.value" [disabled]="propertyDescriptor.isEditable ? null : 'disabled'">
        <mat-icon matSuffix>sentiment_very_satisfied</mat-icon>
        <mat-hint>{{propertyDescriptor.description}}</mat-hint>
      </mat-form-field>
    </div>
  `,
  styles: [
  ]
})
export class SingleLineTextPropertyComponent implements OnInit, PropertyComponent {

  constructor() { }
  @Input() workItemDescriptor: WorkItemDescriptor;
  @Input() propertyDescriptor: WorkItemPropertyDescriptor;
  @Input() propertyValue: WorkItemProperty;

  ngOnInit(): void {
  }

}
