import { Observable, of } from 'rxjs';
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
import {
  InferredProp,
  PropCallback,
  PropContributorCallback,
  PropList,
  PropsFactory,
} from '../models/props';
import { PolicyGroup } from '../models/internal/object-extensions';
import { ObjectExtensions } from '../models/object-extensions';
import { ConfigStateService, PermissionService } from '@abp/ng.core';

export function createExtraPropertyValueResolver<T>(
  name: string,
): PropCallback<T, Observable<any>> {
  return (data?) => of((data.record as { [key: string]: any })[EXTRA_PROPERTIES_KEY][name]);
}

export function mergeWithDefaultProps<F extends PropsFactory<any>>(
  extension: F,
  defaultProps: InferredPropDefaults<F>,
  ...contributors: InferredPropContributorCallbacks<F>[]
) {
  Object.keys(defaultProps).forEach((name: string) => {
    const props: InferredProps<F> = extension.get(name);
    props.clearContributors();
    props.addContributor((propList: PropList<any, InferredProp<any>>) =>
      propList.addManyTail(defaultProps[name]),
    );
    contributors.forEach(contributor =>
      (contributor[name] || []).forEach((callback: PropContributorCallback<any>) =>
        props.addContributor(callback),
      ),
    );
  });
}

function isPolicyMet(
  checkFunction: (item: string) => boolean,
  requiresAll: boolean,
  items?: string[],
): boolean {
  if (!items?.length) {
    return true;
  }
  return requiresAll ? items.every(checkFunction) : items.some(checkFunction);
}

export function checkPolicyProperties(
  properties: ObjectExtensions.EntityExtensionProperties,
  configState: ConfigStateService,
  permissionService?: PermissionService,
) {
  //TODO this check will be removed after configuring every contribution in the row 🪄
  if (!permissionService) return;

  const checkPolicy = (policy: PolicyGroup): boolean => {
    const { permissions, globalFeatures, features } = policy;

    const checks = [
      {
        items: permissions?.permissionNames,
        requiresAll: permissions?.requiresAll,
        check: (item: string) => permissionService.getGrantedPolicy(item),
      },
      {
        items: globalFeatures?.features,
        requiresAll: globalFeatures?.requiresAll,
        check: (item: string) => configState.getGlobalFeatureIsEnabled(item),
      },
      {
        items: features?.features,
        requiresAll: features?.requiresAll,
        check: (item: string) => configState.getFeatureIsEnabled(item),
      },
    ];

    return checks.every(({ items, requiresAll, check }) =>
      isPolicyMet(check, requiresAll ?? false, items),
    );
  };

  Object.entries(properties).forEach(([name, property]) => {
    if (property.policy && !checkPolicy(property.policy)) {
      delete properties[name];
    }
  });
}

type InferredPropDefaults<F> =
  F extends EntityPropsFactory<infer RE>
    ? EntityPropDefaults<RE>
    : F extends CreateFormPropsFactory<infer RCF>
      ? CreateFormPropDefaults<RCF>
      : F extends EditFormPropsFactory<infer REF>
        ? EditFormPropDefaults<REF>
        : never;

type InferredPropContributorCallbacks<F> =
  F extends EntityPropsFactory<infer RE>
    ? EntityPropContributorCallbacks<RE>
    : F extends CreateFormPropsFactory<infer RCF>
      ? CreateFormPropContributorCallbacks<RCF>
      : F extends EditFormPropsFactory<infer REF>
        ? EditFormPropContributorCallbacks<REF>
        : never;

type InferredProps<F> =
  F extends EntityPropsFactory<infer RE>
    ? EntityProps<RE>
    : F extends CreateFormPropsFactory<infer RCF>
      ? FormProps<RCF>
      : F extends EditFormPropsFactory<infer REF>
        ? FormProps<REF>
        : never;

// export function checkPolicyProperties(
//   properties: ObjectExtensions.EntityExtensionProperties,
//   configState: ConfigStateService,
//   permissionService?: PermissionService,
// ) {
//   Object.keys(properties).forEach((name: string) => {
//     const property = properties[name];

//     if (!property.policy) {
//       return;
//     }

//     let isPolicyConstraintMet = false;

//     const { permissions, features, globalFeatures } = property.policy;

//     if (!permissionService) {
//       return;
//     }

//     if (!permissions.permissionNames) {
//       return;
//     }

//     const hasPermission = (permission: string): boolean =>
//       permissionService.getGrantedPolicy(permission);

//     isPolicyConstraintMet = permissions.requiresAll
//       ? permissions.permissionNames.every(hasPermission)
//       : permissions.permissionNames.some(hasPermission);

//     if (!isPolicyConstraintMet) {
//       delete properties[name];
//     }

//     if (!globalFeatures.features) {
//       return;
//     }

//     const hasGlobalFeature = (globalFeature: string): boolean =>
//       configState.getGlobalFeatureIsEnabled(globalFeature);

//     isPolicyConstraintMet = globalFeatures.requiresAll
//       ? globalFeatures.features.every(hasGlobalFeature)
//       : globalFeatures.features.some(hasGlobalFeature);

//     if (!isPolicyConstraintMet) {
//       delete properties[name];
//     }

//     const hasFeature = (feature: string): boolean => configState.getFeatureIsEnabled(feature);

//     isPolicyConstraintMet = features.requiresAll
//       ? features.features.every(hasFeature)
//       : features.features.some(hasFeature);

//     if (!isPolicyConstraintMet) {
//       delete properties[name];
//     }
//   });
// }

// export function checkPolicyProperties(
//   properties: ObjectExtensions.EntityExtensionProperties,
//   configState: ConfigStateService,
//   permissionService?: PermissionService,
// ) {
//   if (!permissionService) return;

//   const isConstraintMet = (
//     items: string[] | undefined,
//     requiresAll: boolean | undefined,
//     checkFunction: (item: string) => boolean,
//   ): boolean =>
//     !items ||
//     items.length === 0 ||
//     (requiresAll ? items.every(checkFunction) : items.some(checkFunction));

//   const policyCheckers = [
//     {
//       getItems: (policy: any) => policy.permissions?.permissionNames,
//       check: (permission: string) => permissionService.getGrantedPolicy(permission),
//     },
//     {
//       getItems: (policy: any) => policy.globalFeatures?.features,
//       check: (feature: string) => configState.getGlobalFeatureIsEnabled(feature),
//     },
//     {
//       getItems: (policy: any) => policy.features?.features,
//       check: (feature: string) => configState.getFeatureIsEnabled(feature),
//     },
//   ];

//   Object.keys(properties).forEach((name: string) => {
//     const { policy } = properties[name];
//     if (!policy) return;

//     const shouldDelete = policyCheckers.some(
//       ({ getItems, check }) => !isConstraintMet(getItems(policy), policy.features.requiresAll, check),
//     );

//     if (shouldDelete) delete properties[name];
//   });
// }
