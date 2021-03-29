import {
  Compiler,
  Injector,
  ModuleWithProviders,
  NgModuleFactory,
  NgModuleRef,
  StaticProvider,
  Type,
} from '@angular/core';
import { filter, map } from 'rxjs/operators';
import { ConfigStateService } from '../services/config-state.service';

export class LazyModuleFactory<T> extends NgModuleFactory<T> {
  get moduleType(): Type<T> {
    return this.moduleWithProviders.ngModule;
  }

  constructor(private moduleWithProviders: ModuleWithProviders<T>) {
    super();
  }

  create(parentInjector: Injector | null): NgModuleRef<T> {
    const injector = Injector.create({
      parent: parentInjector,
      providers: this.moduleWithProviders.providers as StaticProvider[],
    });

    const compiler = injector.get(Compiler);
    const factory = compiler.compileModuleSync(this.moduleType);

    return factory.create(injector);
  }
}

export function featuresFactory(
  configState: ConfigStateService,
  featureKeys: string[],
  mapFn: (features) => any = features => features,
) {
  return configState.getFeatures$(featureKeys).pipe(filter(Boolean), map(mapFn));
}
