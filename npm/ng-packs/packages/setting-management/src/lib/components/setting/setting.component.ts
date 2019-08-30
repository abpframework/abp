import { Component, OnInit } from '@angular/core';
import { SettingTab, fade } from '@abp/ng.theme.shared';
import { InitialService } from '../services/initial.service';

@Component({
  selector: 'abp-setting',
  templateUrl: './setting.component.html',
})
export class SettingComponent implements OnInit {
  settings: SettingTab[];

  selected = {} as SettingTab;

  constructor(private initialService: InitialService) {}

  ngOnInit() {
    this.settings = this.initialService.settings;
    this.selected = this.settings[0];
  }
}
