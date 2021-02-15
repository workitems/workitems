import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'blade-host',
  template: `<ng-content></ng-content>`,
  styles: [
    `:host { 
        position:relative; 
        height:100%; 
        width:100%; 
        box-sizing: border-box; 
        
        display:flex; 
        flex-wrap:nowrap;
        
        background-color:darkgray;
      }`
    // , ':host { background-color:red; }'
  ]
})
export class BladeHostComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
