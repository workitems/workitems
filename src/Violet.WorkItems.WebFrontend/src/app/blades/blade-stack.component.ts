import { ComponentFactoryResolver, Type, ViewContainerRef } from '@angular/core';
import { ComponentRef } from '@angular/core';
import { Component, OnInit, ViewChild } from '@angular/core';
import { BladeElementComponent } from './blade-element.component';
import { take } from 'rxjs/operators';

@Component({
  selector: 'blade-stack',
  template: `<ng-container #stackElements></ng-container>`,
  styles: [
    `:host { 
      position:relative;
      width:100%;
      height:100%; 
      box-sizing: border-box; 
      
      display:flex; 
      flex-direction: row; 
      flex-wrap: nowrap; 
      align-items: stretch; 
      overflow-x: auto;

      background-color:darkgray;
    }`
    //, ':host { background-color:orange; }'
  ]
})
export class BladeStackComponent implements OnInit {

  @ViewChild('stackElements', { read: ViewContainerRef }) vcRef: ViewContainerRef;

  constructor(private componentFactoryResolver: ComponentFactoryResolver) { }

  ngOnInit(): void {

  }

  addBladeElementWithContent<TComponent>(component: Type<TComponent>, componentInitializer: (TComponent) => void = undefined): ComponentRef<BladeElementComponent<TComponent>> {
    const elementComponentFactory = this.componentFactoryResolver.resolveComponentFactory(BladeElementComponent);

    const bladeElementComponentRef = this.vcRef.createComponent(elementComponentFactory) as ComponentRef<BladeElementComponent<TComponent>>;
    bladeElementComponentRef.changeDetectorRef.detectChanges();

    const contentComponentRef = bladeElementComponentRef.instance.addContent(component);
    bladeElementComponentRef.instance.bladeComponent = contentComponentRef.instance;
    bladeElementComponentRef.instance.closing.pipe(take(1)).subscribe(() => bladeElementComponentRef.destroy());

    if (componentInitializer !== undefined) {
      componentInitializer(contentComponentRef.instance);

      contentComponentRef.changeDetectorRef.detectChanges();
    }

    return bladeElementComponentRef;
  }

}
