import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'vwi-work-item-nav',
  templateUrl: './work-item-nav.component.html',
  styles: []
})
export class WorkItemNavComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  new() { this.router.navigate(['wi', 'ACME', 'new']); }
  search() { this.router.navigate(['wi', 'search']); }

}
