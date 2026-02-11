import {AuthService, LocalizationPipe} from '@abp/ng.core';
import { Component, inject } from '@angular/core';
import {NgTemplateOutlet} from "@angular/common";
import {DynamicFormComponent, FormFieldConfig} from "@abp/ng.components/dynamic-form";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  imports: [NgTemplateOutlet, LocalizationPipe, DynamicFormComponent]
})
export class HomeComponent {
  private authService = inject(AuthService);

  formFields: FormFieldConfig[] = [
    {
      key: 'firstName',
      type: 'text',
      label: 'First Name',
      placeholder: 'Enter first name',
      value: 'erdemc',
      required: true,
      validators: [
        { type: 'required', message: 'First name is required' },
        { type: 'minLength', value: 2, message: 'Minimum 2 characters required' }
      ],
      gridSize: 6,
      order: 1
    },
    {
      key: 'lastName',
      type: 'text',
      label: 'Last Name',
      placeholder: 'Enter last name',
      required: true,
      validators: [
        { type: 'required', message: 'Last name is required' }
      ],
      gridSize: 12,
      order: 3
    },
    {
      key: 'email',
      type: 'email',
      label: 'Email Address',
      placeholder: 'Enter email',
      required: true,
      validators: [
        { type: 'required', message: 'Email is required' },
        { type: 'email', message: 'Please enter a valid email' }
      ],
      gridSize: 6,
      order: 2
    },
    {
      key: 'userType',
      type: 'select',
      label: 'User Type',
      required: true,
      options: [
        { key: 'admin', value: 'Administrator' },
        { key: 'user', value: 'Regular User' },
        { key: 'guest', value: 'Guest User' }
      ],
      validators: [
        { type: 'required', message: 'Please select user type' }
      ],
      order: 4
    },
    {
      key: 'adminNotes',
      type: 'textarea',
      label: 'Admin Notes',
      placeholder: 'Enter admin-specific notes',
      conditionalLogic: [
        {
          dependsOn: 'userType',
          condition: 'equals',
          value: 'admin',
          action: 'show'
        }
      ],
      order: 5
    }
  ];

  get hasLoggedIn(): boolean {
    return this.authService.isAuthenticated;
  }

  login() {
    this.authService.navigateToLogin();
  }
}
