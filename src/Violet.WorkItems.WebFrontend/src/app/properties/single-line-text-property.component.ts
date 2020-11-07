import { Component, Input, OnInit } from '@angular/core';
import { WorkItemDescriptor, WorkItemPropertyDescriptor } from '../descriptor-manager.service';
import { WorkItemProperty } from '../work-item.service';
import { PropertyComponent } from './property';

@Component({
  template: `
    <div class="p-field p-grid">
      <label for="i1" class="p-col-fixed" style="width:100px">{{propertyDescriptor.label}}</label>
      <div class="p-col p-inputgroup">
          <span class="p-inputgroup-addon"><i class="pi pi-user"></i></span>
          <input id="i1" type="text" pInputText placeholder="{{propertyDescriptor.hint}}" [(ngModel)]="propertyValue.value">
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
