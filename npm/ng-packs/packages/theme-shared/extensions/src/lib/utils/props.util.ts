import { of } from 'rxjs';
import { EXTRA_PROPERTIES_KEY } from '../constants/extra-properties';
import {
  EntityPropContributorCallbacks,
  EntityPropDefaults,
  EntityProps,
  EntityPropsFactory,
} from '../models/entity-props';
import {
  CreateFormPropContributorCallbacks,
  CreateFormPropDefaults,
  CreateFormPropsFactory,
  EditFormPropContributorCallbacks,
  EditFormPropDefaults,
  EditFormPropsFactory,
  FormProps,
} from '../models/form-props';
import { PropContributorCallback, PropData, PropList, PropsFactory } from '../models/props';

export function createExtraPropertyValueResolver<T>(name: string) {
  return (data?: PropData<T>) => of(data.record[EXTRA_PROPERTIES_KEY][name]);
}

export function mergeWithDefaultProps<F extends PropsFactory<any>>(
  extension: F,
  defaultProps: InferredPropDefaults<F>,
  ...contributors: InferredPropContributorCallbacks<F>[]
) {
  Object.keys(defaultProps).forEach((name: string) => {
    const props: InferredProps<F> = extension.get(name);
    props.clearContributors();
    props.addContributor((propList: PropList) => propList.addManyTail(defaultProps[name]));
    contributors.forEach(contributor =>
      (contributor[name] || []).forEach((callback: PropContributorCallback<any>) =>
        props.addContributor(callback),
      ),
    );
  });
}
type InferredPropDefaults<F> = F extends EntityPropsFactory<infer RE>
  ? EntityPropDefaults<RE>
  : F extends CreateFormPropsFactory<infer RCF>
  ? CreateFormPropDefaults<RCF>
  : F extends EditFormPropsFactory<infer REF>
  ? EditFormPropDefaults<REF>
  : never;

type InferredPropContributorCallbacks<F> = F extends EntityPropsFactory<infer RE>
  ? EntityPropContributorCallbacks<RE>
  : F extends CreateFormPropsFactory<infer RCF>
  ? CreateFormPropContributorCallbacks<RCF>
  : F extends EditFormPropsFactory<infer REF>
  ? EditFormPropContributorCallbacks<REF>
  : never;

type InferredProps<F> = F extends EntityPropsFactory<infer RE>
  ? EntityProps<RE>
  : F extends CreateFormPropsFactory<infer RCF>
  ? FormProps<RCF>
  : F extends EditFormPropsFactory<infer REF>
  ? FormProps<REF>
  : never;
