import {
  ABP,
  ApplicationLocalizationConfigurationDto,
  ConfigStateService,
  ExtensionPropertyUiLookupDto,
} from '@abp/ng.core';
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
import {
  createTypeaheadDisplayNameGenerator,
  createTypeaheadOptions,
  getTypeaheadType,
  hasTypeaheadTextSuffix,
} from './typeahead.util';
import { getValidatorsFromProperty } from './validation.util';

function selectObjectExtensions(
  configState: ConfigStateService,
): Observable<ObjectExtensions.ObjectExtensionsDto> {
  return configState.getOne$('objectExtensions');
}

function selectLocalization(
  configState: ConfigStateService,
): Observable<ApplicationLocalizationConfigurationDto> {
  return configState.getOne$('localization');
}

function selectEnums(
  configState: ConfigStateService,
): Observable<Record<string, ObjectExtensions.ExtensionEnumDto>> {
  return selectObjectExtensions(configState).pipe(
    map((extensions: ObjectExtensions.ObjectExtensionsDto) =>
      Object.keys(extensions.enums).reduce((acc, key) => {
        const { fields, localizationResource } = extensions.enums[key];
        acc[key] = {
          fields,
          localizationResource,
          transformed: createEnum(fields),
        };
        return acc;
      }, {} as Record<string, ObjectExtensions.ExtensionEnumDto>),
    ),
  );
}

export function getObjectExtensionEntitiesFromStore(
  configState: ConfigStateService,
  moduleKey: string,
) {
  return selectObjectExtensions(configState).pipe(
    map(extensions => {
      if (!extensions) return null;

      return (extensions.modules[moduleKey] || ({} as ObjectExtensions.ModuleExtensionDto))
        .entities;
    }),
    map(entities => (isUndefined(entities) ? {} : entities)),
    filter<ObjectExtensions.EntityExtensions>(Boolean),
    take(1),
  );
}

export function mapEntitiesToContributors<T = any>(
  configState: ConfigStateService,
  resource: string,
) {
  return pipe(
    switchMap(entities =>
      zip(selectLocalization(configState), selectEnums(configState)).pipe(
        map(([localization, enums]) => {
          const generateDisplayName = createDisplayNameLocalizationPipeKeyGenerator(localization);

          return Object.keys(entities).reduce(
            (acc, key: keyof ObjectExtensions.EntityExtensions) => {
              acc.prop[key] = [];
              acc.createForm[key] = [];
              acc.editForm[key] = [];

              const entity: ObjectExtensions.EntityExtensionDto = entities[key];
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
  generateDisplayName: ObjectExtensions.DisplayNameGeneratorFn,
  resource: string,
  enums: Record<string, ObjectExtensions.ExtensionEnumDto>,
) {
  return (
    properties: ObjectExtensions.EntityExtensionProperties,
    contributors: ObjectExtensions.PropContributors<T>,
    key: string,
  ) => {
    const isExtra = true;
    const generateTypeaheadDisplayName = createTypeaheadDisplayNameGenerator(
      generateDisplayName,
      properties,
    );

    Object.keys(properties).forEach((name: string) => {
      const property = properties[name];
      const propName = name;
      const lookup = property.ui.lookup || ({} as ExtensionPropertyUiLookupDto);
      const type = getTypeaheadType(lookup, name) || getTypeFromProperty(property);
      const generateDN = hasTypeaheadTextSuffix(name)
        ? generateTypeaheadDisplayName
        : generateDisplayName;
      const displayName = generateDN(property.displayName, { name, resource });

      if (property.ui.onTable.isVisible) {
        const sortable = Boolean(property.ui.onTable.isSortable);
        const columnWidth = type === ePropType.Boolean ? 150 : 250;
        const valueResolver =
          type === ePropType.Enum
            ? createEnumValueResolver(property.type, enums[property.type], propName)
            : createExtraPropertyValueResolver<T>(propName);

        const entityProp = new EntityProp<T>({
          type,
          name: propName,
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
        if (type === ePropType.Enum) options = createEnumOptions(propName, enums[property.type]);
        else if (type === ePropType.Typeahead) options = createTypeaheadOptions(lookup);

        const formProp = new FormProp({
          type,
          name: propName,
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

function getTypeFromProperty(property: ObjectExtensions.ExtensionPropertyDto): ePropType {
  return (property.typeSimple.replace(/\?$/, '') as string) as ePropType;
}

function isUndefined(obj: any): obj is undefined {
  return typeof obj === 'undefined';
}
