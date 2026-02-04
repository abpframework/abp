import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { DynamicFormComponent, FormFieldConfig } from '@abp/ng.components/dynamic-form';
import { FormConfigService } from './form-config.service';

@Component({
    selector: 'app-dynamic-form-page',
    templateUrl: './dynamic-form-page.component.html',
    imports: [DynamicFormComponent],
})
export class DynamicFormPageComponent implements OnInit {
    @ViewChild(DynamicFormComponent, { static: false }) dynamicFormComponent: DynamicFormComponent;
    protected readonly formConfigService = inject(FormConfigService);

    formFields: FormFieldConfig[] = [];

    ngOnInit() {
        this.formConfigService.getFormConfig().subscribe(config => {
            this.formFields = config;
        });
    }

    submit(formData: any) {
        console.log('✅ Form Submitted Successfully!', formData);
        console.table(formData);
        
        // Show success message
        alert('✅ Form submitted successfully! Check the console for details.');
        
        // Reset form after submission
        this.dynamicFormComponent.resetForm();
    }

    cancel() {
        console.log('❌ Form Cancelled');
        alert('Form cancelled');
        this.dynamicFormComponent.resetForm();
    }
}
