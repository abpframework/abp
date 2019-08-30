import { SettingTab } from '@abp/ng.theme.shared';
import { Injectable, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import snq from 'snq';

@Injectable()
export class InitialService {
  public settings: SettingTab[];

  constructor(private router: Router) {
    this.settings = this.router.config
      .map(route => snq(() => route.data.routes.settings))
      .filter(settings => settings && settings.length)
      .reduce((acc, val) => [...acc, ...val], [])
      .sort((a, b) => a.order - b.order);
  }
}
