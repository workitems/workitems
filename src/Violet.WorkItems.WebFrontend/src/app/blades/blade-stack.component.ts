import { ComponentFactoryResolver, Type, ViewContainerRef } from '@angular/core';
import { ComponentRef } from '@angular/core';
import { Component, OnInit, ViewChild } from '@angular/core';
import { BladeElementComponent } from './blade-element.component';
import { take } from 'rxjs/operators';

@Component({
  selector: 'blade-stack',
  template: `<ng-container #bladeStackContainer></ng-container>`,
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
  private blades: { [id: string]: ComponentRef<BladeElementComponent<any>>; } = {};

  @ViewChild('bladeStackContainer', { read: ViewContainerRef }) vcRef: ViewContainerRef;

  constructor(private componentFactoryResolver: ComponentFactoryResolver) { }

  ngOnInit(): void {

  }

  addBladeElementWithContent<TComponent>(id: string, component: Type<TComponent>, componentInitializer: (TComponent) => void = undefined): ComponentRef<BladeElementComponent<TComponent>> {
    if (this.blades[id] === undefined) {
      const elementComponentFactory = this.componentFactoryResolver.resolveComponentFactory(BladeElementComponent);

      const bladeElementComponentRef = this.vcRef.createComponent(elementComponentFactory) as ComponentRef<BladeElementComponent<TComponent>>;
      bladeElementComponentRef.changeDetectorRef.detectChanges();

      const contentComponentRef = bladeElementComponentRef.instance.addContent(component);
      bladeElementComponentRef.instance.bladeComponent = contentComponentRef.instance;
      bladeElementComponentRef.instance.closing.pipe(take(1)).subscribe(() => {
        bladeElementComponentRef.destroy();
      });
      bladeElementComponentRef.onDestroy(() => {
        delete this.blades[id];
      });

      if (componentInitializer !== undefined) {
        componentInitializer(contentComponentRef.instance);

        contentComponentRef.changeDetectorRef.detectChanges();
      }

      this.blades[id] = bladeElementComponentRef;

      return bladeElementComponentRef;
    } else {
      return this.blades[id];
    }
  }
}
