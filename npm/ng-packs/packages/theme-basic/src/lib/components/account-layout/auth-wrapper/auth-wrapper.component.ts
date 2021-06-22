import { Component, OnInit } from '@angular/core';
import { AuthWrapperService } from '@abp/ng.account-core';

@Component({
  selector: 'abp-auth-wrapper',
  templateUrl: './auth-wrapper.component.html',
  providers: [AuthWrapperService],
})
export class AuthWrapperComponent implements OnInit {
  constructor(public service: AuthWrapperService) {}

  ngOnInit(): void {}
}
