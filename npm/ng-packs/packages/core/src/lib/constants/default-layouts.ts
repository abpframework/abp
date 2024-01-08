import {eLayoutType, eThemeSharedComponents} from "../enums";

export const DEFAULT_DYNAMIC_LAYOUTS = new Map<string, string>([
  [eLayoutType.application, eThemeSharedComponents.ApplicationLayoutComponent],
  [eLayoutType.account, eThemeSharedComponents.AccountLayoutComponent],
  [eLayoutType.empty, eThemeSharedComponents.EmptyLayoutComponent],
]);
