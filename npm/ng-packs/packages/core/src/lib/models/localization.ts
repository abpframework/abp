export interface LocalizationWithDefault {
  key: string;
  defaultValue: string;
}

export type LocalizationParam = string | LocalizationWithDefault;
