import { createHostFactory, SpectatorHost } from '@ngneat/spectator/jest';
import { AbpFormFieldComponent } from './abp-form-field.component';
import { AbpFormFieldLabelComponent } from './abp-form-field-label.component';
import { AbpFormFieldHintComponent } from './abp-form-field-hint.component';
import { AbpFormFieldErrorComponent } from './abp-form-field-error.component';

describe('AbpFormFieldComponent', () => {
  let spectator: SpectatorHost<AbpFormFieldComponent>;
  const createHost = createHostFactory({
    component: AbpFormFieldComponent,
    declarations: [
      AbpFormFieldLabelComponent,
      AbpFormFieldHintComponent,
      AbpFormFieldErrorComponent,
    ],
  });

  describe('Basic rendering', () => {
    it('should create', () => {
      spectator = createHost(`
        <abp-form-field>
          <abp-form-field-label>Test Label</abp-form-field-label>
        </abp-form-field>
      `);
      expect(spectator.component).toBeTruthy();
    });

    it('should render with default container class', () => {
      spectator = createHost(`
        <abp-form-field>
          <abp-form-field-label>Test Label</abp-form-field-label>
        </abp-form-field>
      `);
      expect(spectator.hostElement).toHaveClass('mb-3');
      expect(spectator.hostElement).toHaveClass('d-block');
    });

    it('should apply custom container class', () => {
      spectator = createHost(`
        <abp-form-field containerClass="custom-class">
          <abp-form-field-label>Test Label</abp-form-field-label>
        </abp-form-field>
      `);
      expect(spectator.hostElement).toHaveClass('custom-class');
    });
  });

  describe('Label integration', () => {
    it('should render label component', () => {
      spectator = createHost(`
        <abp-form-field>
          <abp-form-field-label for="test-input">Test Label</abp-form-field-label>
        </abp-form-field>
      `);
      const label = spectator.query('label');
      expect(label).toBeTruthy();
      expect(label).toHaveText('Test Label');
    });

    it('should bind label for attribute', () => {
      spectator = createHost(`
        <abp-form-field>
          <abp-form-field-label for="test-input">Test Label</abp-form-field-label>
        </abp-form-field>
      `);
      const label = spectator.query('label');
      expect(label).toHaveAttribute('for', 'test-input');
    });
  });

  describe('Content projection', () => {
    it('should project abp-input', () => {
      spectator = createHost(`
        <abp-form-field>
          <abp-form-field-label>Test Label</abp-form-field-label>
          <input type="text" id="test-input" />
        </abp-form-field>
      `);
      const input = spectator.query('input');
      expect(input).toBeTruthy();
    });

    it('should project hint component', () => {
      spectator = createHost(`
        <abp-form-field>
          <abp-form-field-label>Test Label</abp-form-field-label>
          <abp-form-field-hint>Test hint</abp-form-field-hint>
        </abp-form-field>
      `);
      const hint = spectator.query('abp-form-field-hint');
      expect(hint).toBeTruthy();
    });

    it('should project error component', () => {
      spectator = createHost(`
        <abp-form-field>
          <abp-form-field-label>Test Label</abp-form-field-label>
          <abp-form-field-error>Test error</abp-form-field-error>
        </abp-form-field>
      `);
      const error = spectator.query('abp-form-field-error');
      expect(error).toBeTruthy();
    });
  });

  describe('Host binding', () => {
    it('should have correct host classes', () => {
      spectator = createHost(`
        <abp-form-field>
          <abp-form-field-label>Test Label</abp-form-field-label>
        </abp-form-field>
      `);
      expect(spectator.hostElement).toHaveClass('d-block');
      expect(spectator.hostElement).toHaveClass('mb-3');
    });

    it('should combine default and custom classes', () => {
      spectator = createHost(`
        <abp-form-field containerClass="mt-4">
          <abp-form-field-label>Test Label</abp-form-field-label>
        </abp-form-field>
      `);
      expect(spectator.hostElement).toHaveClass('d-block');
      expect(spectator.hostElement).toHaveClass('mb-3');
      expect(spectator.hostElement).toHaveClass('mt-4');
    });
  });

  describe('Export as', () => {
    it('should be accessible via exportAs', () => {
      spectator = createHost(`
        <abp-form-field #formField="abpFormField">
          <abp-form-field-label>Test Label</abp-form-field-label>
        </abp-form-field>
      `);
      const formField = spectator.queryHost('abp-form-field');
      expect(formField).toBeTruthy();
    });
  });
});
