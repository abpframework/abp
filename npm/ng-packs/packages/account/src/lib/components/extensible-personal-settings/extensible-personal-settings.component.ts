import {Component, Injector, OnInit} from '@angular/core';
import {EXTENSIONS_IDENTIFIER, FormPropData, generateFormFromProps} from "@abp/ng.theme.shared/extensions";
import { eAccountComponents } from '../../enums/components';
import {FormBuilder, FormGroup} from "@angular/forms";
import {ProfileService} from "@abp/ng.account.core/proxy";

@Component({
  selector: 'abp-extensible-personal-settings',
  templateUrl: './extensible-personal-settings.component.html',
  styleUrls: ['./extensible-personal-settings.component.scss'],
  providers:[
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eAccountComponents.PersonalSettings,
    },
  ]
})
export class ExtensiblePersonalSettingsComponent implements  OnInit {
  selected = {a: 1};// hacky for triggering 'edit' mode of extensible-form

  form: FormGroup;

  constructor(
    private profileService: ProfileService,
    protected fb: FormBuilder,
    protected injector: Injector,
  ) {}


  buildForm() {
    const data = new FormPropData(this.injector, this.selected);
    this.form = generateFormFromProps(data);
  }

  ngOnInit(): void {
    this.buildForm()
  }

  save() {

  }
}
