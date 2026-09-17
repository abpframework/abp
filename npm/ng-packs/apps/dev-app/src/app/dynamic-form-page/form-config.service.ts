import { Injectable } from '@angular/core';
import { FormFieldConfig } from '@abp/ng.components/dynamic-form';
import { Observable, of } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class FormConfigService {
    getFormConfig(): Observable<FormFieldConfig[]> {
        const formConfig: FormFieldConfig[] = [
            // Section 1: Basic Text Inputs
            {
                key: 'firstName',
                label: 'First Name',
                type: 'text',
                placeholder: 'Enter your first name',
                required: true,
                gridSize: 6,
                order: 1,
                validators: [{ type: 'required', message: 'First name is required' }],
            },
            {
                key: 'lastName',
                label: 'Last Name',
                type: 'text',
                placeholder: 'Enter your last name',
                required: true,
                gridSize: 6,
                order: 2,
                validators: [{ type: 'required', message: 'Last name is required' }],
            },

            // Section 2: Email & Password
            {
                key: 'email',
                label: 'Email Address',
                type: 'email',
                placeholder: 'example@domain.com',
                required: true,
                gridSize: 6,
                order: 3,
                validators: [
                    { type: 'required', message: 'Email is required' },
                    { type: 'email', message: 'Invalid email address' },
                ],
            },
            {
                key: 'password',
                label: 'Password',
                type: 'password',
                placeholder: 'Enter a strong password',
                required: true,
                gridSize: 6,
                order: 4,
                minLength: 8,
                maxLength: 50,
                validators: [
                    { type: 'required', message: 'Password is required' },
                    { type: 'minLength', value: 8, message: 'Password must be at least 8 characters' },
                ],
            },

            // Section 3: Contact Information
            {
                key: 'phone',
                label: 'Phone Number',
                type: 'tel',
                placeholder: '555-123-4567',
                gridSize: 6,
                order: 5,
                pattern: '[0-9]{3}-[0-9]{3}-[0-9]{4}',
            },
            {
                key: 'website',
                label: 'Website',
                type: 'url',
                placeholder: 'https://example.com',
                gridSize: 6,
                order: 6,
            },

            // Section 4: Numbers & Dates
            {
                key: 'age',
                label: 'Age',
                type: 'number',
                placeholder: 'Enter your age',
                required: true,
                gridSize: 4,
                order: 7,
                min: 18,
                max: 100,
                validators: [
                    { type: 'required', message: 'Age is required' },
                    { type: 'min', value: 18, message: 'You must be at least 18 years old' },
                ],
            },
            {
                key: 'birthdate',
                label: 'Birth Date',
                type: 'date',
                required: true,
                gridSize: 4,
                order: 8,
                max: new Date().toISOString().split('T')[0],
                validators: [{ type: 'required', message: 'Birth date is required' }],
            },
            {
                key: 'appointmentTime',
                label: 'Appointment Date & Time',
                type: 'datetime-local',
                gridSize: 4,
                order: 9,
                min: new Date().toISOString().slice(0, 16),
            },

            // Section 5: Select & Radio
            {
                key: 'country',
                label: 'Country',
                type: 'select',
                required: true,
                gridSize: 6,
                order: 10,
                options: {
                    defaultValues: [
                        { key: 'usa', value: 'United States' },
                        { key: 'uk', value: 'United Kingdom' },
                        { key: 'canada', value: 'Canada' },
                        { key: 'germany', value: 'Germany' },
                        { key: 'france', value: 'France' },
                    ],
                },
                validators: [{ type: 'required', message: 'Country is required' }],
            },
            {
                key: 'gender',
                label: 'Gender',
                type: 'radio',
                required: true,
                gridSize: 6,
                order: 11,
                options: {
                    defaultValues: [
                        { key: 'male', value: 'Male' },
                        { key: 'female', value: 'Female' },
                        { key: 'other', value: 'Other' },
                        { key: 'prefer-not-to-say', value: 'Prefer not to say' },
                    ],
                },
                validators: [{ type: 'required', message: 'Gender is required' }],
            },

            // Section 6: Conditional Field (shown when country is USA)
            {
                key: 'state',
                label: 'State (USA Only)',
                type: 'select',
                gridSize: 6,
                order: 12,
                options: {
                    defaultValues: [
                        { key: 'ca', value: 'California' },
                        { key: 'ny', value: 'New York' },
                        { key: 'tx', value: 'Texas' },
                        { key: 'fl', value: 'Florida' },
                    ],
                },
                conditionalLogic: [
                    { dependsOn: 'country', condition: 'equals', value: 'usa', action: 'show' },
                ],
            },

            // Section 7: Time & Range
            {
                key: 'preferredTime',
                label: 'Preferred Contact Time',
                type: 'time',
                gridSize: 6,
                order: 13,
                step: '900', // 15 minutes
            },
            {
                key: 'experienceLevel',
                label: 'Experience Level (0-10)',
                type: 'range',
                gridSize: 6,
                order: 14,
                min: 0,
                max: 10,
                step: 1,
                value: 5,
            },

            // Section 8: Color & File
            {
                key: 'favoriteColor',
                label: 'Favorite Color',
                type: 'color',
                gridSize: 6,
                order: 15,
                value: '#007bff',
            },
            {
                key: 'profilePicture',
                label: 'Profile Picture',
                type: 'file',
                gridSize: 6,
                order: 16,
                accept: 'image/*',
                multiple: false,
            },

            // Section 9: Textarea & Checkbox
            {
                key: 'bio',
                label: 'Biography',
                type: 'textarea',
                placeholder: 'Tell us about yourself...',
                gridSize: 12,
                order: 17,
                maxLength: 500,
            },
            {
                key: 'newsletter',
                label: 'Subscribe to newsletter',
                type: 'checkbox',
                gridSize: 6,
                order: 18,
            },
            {
                key: 'terms',
                label: 'I agree to the terms and conditions',
                type: 'checkbox',
                required: true,
                gridSize: 6,
                order: 19,
                validators: [{ type: 'requiredTrue', message: 'You must agree to the terms' }],
            },

            // Section 10: NESTED FORM - Phone Numbers (Array)
            {
                key: 'phoneNumbers',
                type: 'array',
                label: 'Phone Numbers',
                gridSize: 12,
                order: 20,
                minItems: 1,
                maxItems: 5,
                children: [
                    {
                        key: 'type',
                        type: 'select',
                        label: 'Type',
                        gridSize: 4,
                        required: true,
                        options: {
                            defaultValues: [
                                { key: 'mobile', value: 'Mobile' },
                                { key: 'home', value: 'Home' },
                                { key: 'work', value: 'Work' },
                                { key: 'other', value: 'Other' }
                            ]
                        },
                        validators: [{ type: 'required', message: 'Phone type is required' }],
                    },
                    {
                        key: 'number',
                        type: 'tel',
                        label: 'Number',
                        gridSize: 8,
                        required: true,
                        placeholder: '555-123-4567',
                        validators: [{ type: 'required', message: 'Phone number is required' }],
                    },
                ]
            },

            // Section 11: NESTED FORM - Work Experience (Array with nested group)
            {
                key: 'workExperience',
                type: 'array',
                label: 'Work Experience',
                gridSize: 12,
                order: 21,
                minItems: 0,
                maxItems: 10,
                children: [
                    {
                        key: 'company',
                        type: 'text',
                        label: 'Company Name',
                        gridSize: 6,
                        required: true,
                        validators: [{ type: 'required', message: 'Company name is required' }],
                    },
                    {
                        key: 'position',
                        type: 'text',
                        label: 'Position',
                        gridSize: 6,
                        required: true,
                        validators: [{ type: 'required', message: 'Position is required' }],
                    },
                    {
                        key: 'startDate',
                        type: 'date',
                        label: 'Start Date',
                        gridSize: 6,
                        required: true,
                        validators: [{ type: 'required', message: 'Start date is required' }],
                    },
                    {
                        key: 'endDate',
                        type: 'date',
                        label: 'End Date',
                        gridSize: 6,
                    },
                    {
                        key: 'currentJob',
                        type: 'checkbox',
                        label: 'Currently working here',
                        gridSize: 12,
                    },
                    {
                        key: 'description',
                        type: 'textarea',
                        label: 'Job Description',
                        placeholder: 'Describe your responsibilities...',
                        gridSize: 12,
                        maxLength: 500,
                    },
                ]
            },

            // Section 12: NESTED FORM - Address Group (Group type)
            {
                key: 'address',
                type: 'group',
                label: 'Address Information',
                gridSize: 12,
                order: 22,
                children: [
                    {
                        key: 'street',
                        type: 'text',
                        label: 'Street Address',
                        gridSize: 8,
                        placeholder: '123 Main St',
                    },
                    {
                        key: 'apartment',
                        type: 'text',
                        label: 'Apt/Suite',
                        gridSize: 4,
                        placeholder: 'Apt 4B',
                    },
                    {
                        key: 'city',
                        type: 'text',
                        label: 'City',
                        gridSize: 6,
                        required: true,
                        validators: [{ type: 'required', message: 'City is required' }],
                    },
                    {
                        key: 'zipCode',
                        type: 'text',
                        label: 'ZIP Code',
                        gridSize: 6,
                        required: true,
                        pattern: '[0-9]{5}',
                        validators: [{ type: 'required', message: 'ZIP code is required' }],
                    },
                ]
            },
        ];

        return of(formConfig);
    }
}
