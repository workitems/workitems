import { ComponentRef, EventEmitter, Optional, ViewChild, ViewContainerRef } from '@angular/core';
import { Output } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { BladeHostComponent } from './blade-host.component';
import { BladeStackComponent } from './blade-stack.component';

@Component({
  selector: 'blade-element',
  template: `
    <ng-content></ng-content>
    <button class="close-icon" (click)="close()" mat-icon-button color="warn" aria-label="Close">
      <mat-icon>close</mat-icon>
    </button>
  `,
  styles: [
    `:host { 
        position:relative;
        height:100%;
        min-width:200px; 
        box-sizing: border-box; flex-grow:1; 
        padding:8px;
        
        display: flex;
        flex-direction: column;

        overflow-y:auto;
        background-color:whitesmoke;
      }`,
    //':host { padding-top:88px; }',
    //`.blade-head { position:absolute;top:0px;left:0px;width:100%;height:80px; margin-bottom:8px; }`,
    '.close-icon { position:absolute;top:4px;right:4px }'
    //, ':host { background-color:green; }'
  ],
  host: {
    'class': 'mat-elevation-z4'
  }
})
export class BladeElementComponent<TComponent> implements OnInit {

  @Output() closing = new EventEmitter<any>();

  bladeComponent: TComponent;

  constructor(@Optional() private host?: BladeHostComponent, @Optional() private stack?: BladeStackComponent) { }

  ngOnInit(): void {
  }

  close(): void {
    this.closing.emit(null);
  }
}
