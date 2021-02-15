import { ComponentFactoryResolver, ComponentRef, EventEmitter, Optional, Type, ViewChild, ViewContainerRef } from '@angular/core';
import { Output } from '@angular/core';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'blade-element',
  template: `
    <ng-container #bladeElementContainer></ng-container>
    <button class="close-icon" (click)="close()" mat-icon-button color="warn" aria-label="Close">
      <mat-icon>close</mat-icon>
    </button>
  `,
  styles: [
    `:host { 
        position:relative;
        height:100%;
        min-width:200px;
        box-sizing: border-box;
        flex-grow:0; 
        flex-shrink:0;
        padding:8px;
        padding-right:48px;
        
        display: inline-flex;
        flex-direction: column;

        overflow-y:auto;
        
        margin-right:4px;
        background-color:whitesmoke;
      }`,
    //':host { padding-top:88px; }',
    //`.blade-head { position:absolute;top:0px;left:0px;width:100%;height:80px; margin-bottom:8px; }`,
    '.close-icon { position:absolute;top:4px;right:4px }'
    //, ':host { background-color:green; }'
  ],
  host: {
    'class': 'mat-elevation-z2'
  }
})
export class BladeElementComponent<TComponent> implements OnInit {
  @ViewChild('bladeElementContainer', { read: ViewContainerRef }) vcRef: ViewContainerRef;

  @Output() closing = new EventEmitter<any>();

  bladeComponent: TComponent;

  constructor(private componentFactoryResolver: ComponentFactoryResolver) { }

  ngOnInit(): void {
  }

  addContent<TComponent>(component: Type<TComponent>): ComponentRef<TComponent> {
    const contentComponentFactory = this.componentFactoryResolver.resolveComponentFactory(component);

    const contentComponentRef = this.vcRef.createComponent(contentComponentFactory);

    return contentComponentRef;
  }

  close(): void {
    this.closing.emit(null);
  }
}
