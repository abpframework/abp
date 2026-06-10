import { TestBed } from '@angular/core/testing';

import { AbpLocalStorageService } from '../services/local-storage.service';

describe('LocalStorageService', () => {
  let service: AbpLocalStorageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AbpLocalStorageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should be called getItem', () => {
    const spy = vi.spyOn(service, 'getItem');
    service.getItem('test');
    expect(spy).toHaveBeenCalled();
  });

  it('should be called setItem', () => {
    const spy = vi.spyOn(service, 'setItem');
    service.setItem('test', 'value');
    expect(spy).toHaveBeenCalled();
  });

  it('should be called removeItem', () => {
    const spy = vi.spyOn(service, 'removeItem');
    service.removeItem('test');
    expect(spy).toHaveBeenCalled();
  });

  it('should be called clear', () => {
    const spy = vi.spyOn(service, 'clear');
    service.clear();
    expect(spy).toHaveBeenCalled();
  });

  it('should be called key', () => {
    const spy = vi.spyOn(service, 'key');
    service.key(0);
    expect(spy).toHaveBeenCalled();
  });

  it('should be called length', () => {
    const spy = vi.spyOn(service, 'length', 'get');
    service.length;
    expect(spy).toHaveBeenCalled();
  });
});
