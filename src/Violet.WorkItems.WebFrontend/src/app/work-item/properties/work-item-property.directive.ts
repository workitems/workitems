import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[propertyHost]'
})
export class WorkItemPropertyDirective {

  constructor(public viewContainerRef: ViewContainerRef) { }

}
