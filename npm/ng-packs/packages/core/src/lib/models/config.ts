import { ApplicationConfiguration } from './application-configuration';
import { ABP } from './common';
import { Environment as IEnvironment } from './environment';
import {
  LocalizationParam as ILocalizationParam,
  LocalizationWithDefault as ILocalizationWithDefault,
} from './localization';

export namespace Config {
  /**
   * @deprecated Use ApplicationConfiguration.Response instead. To be deleted in v5.0.
   */
  export type State = ApplicationConfiguration.Response & ABP.Root & { environment: IEnvironment };

  export type Environment = IEnvironment;

  /**
   * @deprecated Use ApplicationInfo interface instead. To be deleted in v5.0.
   */
  export interface Application {
    name: string;
    baseUrl?: string;
    logoUrl?: string;
  }

  /**
   * @deprecated Use ApiConfig interface instead. To be deleted in v5.0.
   */
  export type ApiConfig = {
    [key: string]: string;
    url: string;
  } & Partial<{
    rootNamespace: string;
  }>;

  /**
   * @deprecated Use Apis interface instead. To be deleted in v5.0.
   */
  export interface Apis {
    [key: string]: ApiConfig;
    default: ApiConfig;
  }

  export type LocalizationWithDefault = ILocalizationWithDefault;

  export type LocalizationParam = ILocalizationParam;

  /**
   * @deprecated Use customMergeFn type instead. To be deleted in v5.0.
   */
  export type customMergeFn = (
    localEnv: Partial<Config.Environment>,
    remoteEnv: any,
  ) => Config.Environment;

  /**
   * @deprecated Use RemoteEnv interface instead. To be deleted in v5.0.
   */
  export interface RemoteEnv {
    url: string;
    mergeStrategy: 'deepmerge' | 'overwrite' | customMergeFn;
    method?: string;
    headers?: ABP.Dictionary<string>;
  }
}
