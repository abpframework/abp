import { APP_INITIALIZER, Injector } from '@angular/core';
import { AbpLanguageService } from '../services/language.service';

export const LPX_LANGUAGE_PROVIDER = {
  provide: APP_INITIALIZER,
  multi: true,
  deps: [Injector],
  useFactory: initLanguage,
};

function initLanguage(injector: Injector) {
  const language = injector.get(AbpLanguageService);
  return () => {
    language.subscribeLanguage();
  };
}
