import { Component, OnInit } from '@angular/core';
import { MyProjectNameService } from '../services/my-project-name.service';

@Component({
  selector: 'lib-my-project-name',
  template: ` <p>my-project-name works!</p> `,
  styles: [],
})
export class MyProjectNameComponent implements OnInit {
  constructor(private service: MyProjectNameService) {}

  ngOnInit(): void {
    this.service.sample().subscribe(console.log);
  }
}
