import { Injector, ModuleWithProviders, NgModule } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { LazyModuleFactory } from '../utils/factory-utils';

@NgModule()
class Module {
  static forChild(): ModuleWithProviders<Module> {
    return {
      ngModule: Module,
      providers: [{ provide: 'foo', useValue: 'bar' }],
    };
  }
}

describe('LazyModuleFactory', () => {
  const factory = new LazyModuleFactory(Module.forChild());

  describe('#moduleType', () => {
    it('should return the ngModule property of given ModuleWithProviders', () => {
      expect(factory.moduleType).toBe(Module);
    });
  });

  describe('#create', () => {
    it('should return an instance of NgModuleRef_', () => {
      TestBed.configureTestingModule({});

      const injector = TestBed.inject(Injector);
      const moduleRef = factory.create(injector);

      expect('componentFactoryResolver' in moduleRef).toBe(true);
      expect('destroy' in moduleRef).toBe(true);
      expect('injector' in moduleRef).toBe(true);
      expect('instance' in moduleRef).toBe(true);
      expect('onDestroy' in moduleRef).toBe(true);
    });
  });
});
