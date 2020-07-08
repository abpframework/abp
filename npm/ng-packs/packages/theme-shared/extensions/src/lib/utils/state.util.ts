import { ABP, ApplicationConfiguration } from '@abp/ng.core';
import { createSelector, Store } from '@ngxs/store';
import { Observable, pipe, zip } from 'rxjs';
import { filter, map, switchMap, take } from 'rxjs/operators';
import { ePropType } from '../enums/props.enum';
import { EntityProp, EntityPropList } from '../models/entity-props';
import { FormProp, FormPropList } from '../models/form-props';
import { ObjectExtensions } from '../models/object-extensions';
import { PropCallback } from '../models/props';
import { createEnum, createEnumOptions, createEnumValueResolver } from './enum.util';
import { createDisplayNameLocalizationPipeKeyGenerator } from './localization.util';
import { createExtraPropertyValueResolver } from './props.util';
import { getValidatorsFromProperty } from './validation.util';

const selectConfig = (state: any) => state.ConfigState;

const selectObjectExtensions = createSelector(
  [selectConfig],
  (config: ObjectExtensions.Config) => config.objectExtensions,
);

const selectLocalization = createSelector(
  [selectConfig],
  (config: ApplicationConfiguration.Response) => config.localization,
);

const selectEnums = createSelector(
  [selectObjectExtensions, selectLocalization],
  (extensions: ObjectExtensions.Item) =>
    Object.keys(extensions.enums).reduce((acc, key) => {
      const { fields, localizationResource } = extensions.enums[key];
      acc[key] = {
        fields,
        localizationResource,
        transformed: createEnum(fields),
      };
      return acc;
    }, {} as ObjectExtensions.Enums),
);

const createObjectExtensionEntitiesSelector = (moduleKey: ModuleKey) =>
  createSelector([selectObjectExtensions], (extensions: ObjectExtensions.Item) => {
    if (!extensions) return null;

    return (extensions.modules[moduleKey] || ({} as ObjectExtensions.Module)).entities;
  });

export function getObjectExtensionEntitiesFromStore(store: Store, moduleKey: ModuleKey) {
  return store.select(createObjectExtensionEntitiesSelector(moduleKey)).pipe(
    map(entities => (isUndefined(entities) ? {} : entities)),
    filter<ObjectExtensions.Entities>(Boolean),
    take(1),
  );
}

export function mapEntitiesToContributors<T = any>(store: Store, resource: string) {
  return pipe(
    switchMap(entities =>
      zip(store.select(selectLocalization), store.select(selectEnums)).pipe(
        map(([localization, enums]) => {
          const generateDisplayName = createDisplayNameLocalizationPipeKeyGenerator(localization);

          return Object.keys(entities).reduce(
            (acc, key: keyof ObjectExtensions.Entities) => {
              acc.prop[key] = [];
              acc.createForm[key] = [];
              acc.editForm[key] = [];

              const entity: ObjectExtensions.Entity = entities[key];
              if (!entity) return acc;

              const properties = entity.properties;
              if (!properties) return acc;

              const mapPropertiesToContributors = createPropertiesToContributorsMapper<T>(
                generateDisplayName,
                resource,
                enums,
              );

              return mapPropertiesToContributors(properties, acc, key);
            },
            {
              prop: {},
              createForm: {},
              editForm: {},
            } as ObjectExtensions.PropContributors,
          );
        }),
      ),
    ),
    take(1),
  );
}

function createPropertiesToContributorsMapper<T = any>(
  generateDisplayName: DisplayNameGeneratorFn,
  resource: string,
  enums: Record<string, ObjectExtensions.Enum>,
) {
  return (
    properties: Record<string, ObjectExtensions.Property>,
    contributors: ObjectExtensions.PropContributors<T>,
    key: string,
  ) => {
    const isExtra = true;

    Object.keys(properties).forEach((name: string) => {
      const property = properties[name];
      const type = getTypeFromProperty(property);
      const displayName = generateDisplayName(property.displayName, { name, resource });

      if (property.ui.onTable.isVisible) {
        const sortable = Boolean(property.ui.onTable.isSortable);
        const columnWidth = type === ePropType.Boolean ? 150 : 250;
        const valueResolver =
          type === ePropType.Enum
            ? createEnumValueResolver(property.type, enums[property.type], name)
            : createExtraPropertyValueResolver<T>(name);

        const entityProp = new EntityProp<T>({
          type,
          name,
          displayName,
          sortable,
          columnWidth,
          valueResolver,
          isExtra,
        });

        const contributor = (propList: EntityPropList<T>) => propList.addTail(entityProp);
        contributors.prop[key].push(contributor);
      }

      const isOnCreateForm = property.ui.onCreateForm.isVisible;
      const isOnEditForm = property.ui.onEditForm.isVisible;

      if (isOnCreateForm || isOnEditForm) {
        const defaultValue = property.defaultValue;
        const validators = () => getValidatorsFromProperty(property);
        let options: PropCallback<any, Observable<ABP.Option<any>[]>>;
        if (type === ePropType.Enum) options = createEnumOptions(name, enums[property.type]);

        const formProp = new FormProp({
          type,
          name,
          displayName,
          options,
          defaultValue,
          validators,
          isExtra,
        });

        const formContributor = (propList: FormPropList<T>) => propList.addTail(formProp);

        if (isOnCreateForm) contributors.createForm[key].push(formContributor);
        if (isOnEditForm) contributors.editForm[key].push(formContributor);
      }
    });

    return contributors;
  };
}

function getTypeFromProperty(property: ObjectExtensions.Property): ePropType {
  return (property.typeSimple.replace(/\?$/, '') as string) as ePropType;
}

function isUndefined(obj: any): obj is undefined {
  return typeof obj === 'undefined';
}

type DisplayNameGeneratorFn = ReturnType<typeof createDisplayNameLocalizationPipeKeyGenerator>;
type ModuleKey = keyof ObjectExtensions.Modules;
