import { Environment } from '@abp/ng.core';
import {
  EnvironmentProviders,
  makeEnvironmentProviders,
  Provider,
} from '@angular/core';
import { LOGO_APP_NAME_TOKEN, LOGO_URL_TOKEN } from '../tokens/logo.token';

export enum LogoFeatureKind {
  Options,
}

export interface LogoFeature<KindT extends LogoFeatureKind> {
  ɵkind: KindT;
  ɵproviders: (Provider | EnvironmentProviders)[];
}

function makeLogoFeature<KindT extends LogoFeatureKind>(
  kind: KindT,
  providers: (Provider | EnvironmentProviders)[],
): LogoFeature<KindT> {
  return {
    ɵkind: kind,
    ɵproviders: providers,
  };
}

export function withEnvironmentOptions(
  options = {} as Environment,
): LogoFeature<LogoFeatureKind.Options> {
  const { name, logoUrl } = options.application || {};

  return makeLogoFeature(LogoFeatureKind.Options, [
    {
      provide: LOGO_URL_TOKEN,
      useValue: logoUrl || '',
    },
    {
      provide: LOGO_APP_NAME_TOKEN,
      useValue: name || 'ProjectName',
    },
  ]);
}

export function provideLogo(
  ...features: LogoFeature<LogoFeatureKind>[]
): EnvironmentProviders {
  const providers: (Provider | EnvironmentProviders)[] = [];
  features.forEach(({ ɵproviders }) => providers.push(...ɵproviders));
  return makeEnvironmentProviders(providers);
}
