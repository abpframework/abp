import { UntypedFormControl, UntypedFormGroup } from '@angular/forms';
import { DateTimeAdapter, DateAdapter, TimeAdapter } from '@abp/ng.theme.shared';

import { EXTRA_PROPERTIES_KEY } from '../constants/extra-properties';
import { ePropType } from '../enums/props.enum';
import { FormPropList } from '../models/form-props';
import { PropData } from '../models/props';
import { ExtensionsService } from '../services/extensions.service';
import { EXTENSIONS_IDENTIFIER } from '../tokens/extensions.token';

export function generateFormFromProps<R = any>(data: PropData<R>) {
  const extensions = data.getInjected(ExtensionsService<R>);
  const identifier = data.getInjected(EXTENSIONS_IDENTIFIER);

  const form = new UntypedFormGroup({});
  const extraForm = new UntypedFormGroup({});
  form.addControl(EXTRA_PROPERTIES_KEY, extraForm);

  const record = data.record || {};
  const type = JSON.stringify(record) === '{}' ? 'create' : 'edit';
  const props: FormPropList<R> = extensions[`${type}FormProps`].get(identifier).props;
  const extraProperties = record[EXTRA_PROPERTIES_KEY] || {};

  props.forEach(({ value: prop }) => {
    const name = prop.name;
    const isExtraProperty = prop.isExtra || name in extraProperties;

    let value = undefined;

    if (isExtraProperty) {
      value = extraProperties[name];
    } else if (name in record) {
      value = record[name];
    }

    if (typeof value === 'undefined') value = prop.defaultValue;

    if (value) {
      let adapter: DateAdapter | TimeAdapter | DateTimeAdapter;
      switch (prop.type) {
        case ePropType.Date:
          adapter = new DateAdapter();
          value = adapter.toModel(adapter.fromModel(value));
          break;
        case ePropType.Time:
          adapter = new TimeAdapter();
          value = adapter.toModel(adapter.fromModel(value));
          break;
        case ePropType.DateTime:
          adapter = new DateTimeAdapter();
          value = adapter.toModel(adapter.fromModel(value) as any);
          break;
        default:
          break;
      }
    }

    const formControl = new UntypedFormControl(value, {
      asyncValidators: prop.asyncValidators(data),
      validators: prop.validators(data),
    });

    (isExtraProperty ? extraForm : form).addControl(name, formControl);
  });

  return form;
}
