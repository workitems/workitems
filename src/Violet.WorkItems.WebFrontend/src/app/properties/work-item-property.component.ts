import { Component, ComponentFactoryResolver, Input, OnInit, Type, ViewChild } from '@angular/core';
import { WorkItemDescriptor, WorkItemPropertyDescriptor } from '../descriptor-manager.service';
import { WorkItemProperty } from '../work-item.service';
import { PropertyComponent } from './property';
import { SingleLineTextPropertyComponent } from './single-line-text-property.component';
import { WorkItemPropertyDirective } from './work-item-property.directive';

@Component({
  selector: 'vwi-work-item-property',
  template: `
    <ng-template propertyHost></ng-template>
  `,
  styles: [
  ]
})
export class WorkItemPropertyComponent implements OnInit {
  @Input() workItemDescriptor: WorkItemDescriptor;
  @Input() propertyDescriptor: WorkItemPropertyDescriptor;
  @Input() propertyValue: WorkItemProperty;

  @ViewChild(WorkItemPropertyDirective, { static: true }) propertyHost: WorkItemPropertyDirective;

  constructor(private componentFactoryResolver: ComponentFactoryResolver) { }

  ngOnInit(): void {
    const type = this.determinePropertyComponent();

    this.loadPropertyComponent(type);
  }

  private determinePropertyComponent(): Type<PropertyComponent> {
    let result: Type<PropertyComponent> = null;

    if (this.propertyDescriptor.dataType == "string" && this.propertyDescriptor.propertyType == "SingleRaw") {
      result = SingleLineTextPropertyComponent;
    }

    return result;
  }

  private loadPropertyComponent(component: Type<PropertyComponent>) {
    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(component);

    const viewContainerRef = this.propertyHost.viewContainerRef;
    viewContainerRef.clear();

    const componentRef = viewContainerRef.createComponent<PropertyComponent>(componentFactory);
    componentRef.instance.workItemDescriptor = this.workItemDescriptor;
    componentRef.instance.propertyDescriptor = this.propertyDescriptor;
    componentRef.instance.propertyValue = this.propertyValue;
  }

}
