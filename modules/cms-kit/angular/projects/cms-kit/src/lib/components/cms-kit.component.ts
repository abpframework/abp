import { Component, OnInit } from '@angular/core';
import { CmsKitService } from '../services/cms-kit.service';

@Component({
  selector: 'lib-cms-kit',
  template: ` <p>cms-kit works!</p> `,
  styles: [],
})
export class CmsKitComponent implements OnInit {
  constructor(private service: CmsKitService) {}

  ngOnInit(): void {
    this.service.sample().subscribe(console.log);
  }
}
