import { Component, Input, OnInit } from '@angular/core';
import { WorkItemDescriptor, WorkItemPropertyDescriptor } from '../descriptor-manager.service';
import { WorkItemProperty } from '../work-item.service';
import { PropertyComponent } from './property';

@Component({
  template: `
    <div class="p-field">
      <label for="i1" class="p-m-0">{{propertyDescriptor.label}} <small>{{propertyDescriptor.description}}</small></label>
      <div class="p-col p-inputgroup p-p-0">
          <span class="p-inputgroup-addon"><i class="pi pi-user"></i></span>
          <input id="i1" type="text" pInputText placeholder="{{propertyDescriptor.hint}}" [(ngModel)]="propertyValue.value" [disabled]="propertyDescriptor.isEditable ? null : 'disabled'">
      </div>
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
