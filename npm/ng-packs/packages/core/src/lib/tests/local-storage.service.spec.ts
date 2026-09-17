import { TestBed } from '@angular/core/testing';

import { AbpLocalStorageService } from '../services/local-storage.service';

describe('LocalStorageService', () => {
  let service: AbpLocalStorageService;
  let localStorageMock: Storage;

  beforeEach(() => {
    localStorageMock = {
      clear: vi.fn(),
      getItem: vi.fn(),
      key: vi.fn(),
      removeItem: vi.fn(),
      setItem: vi.fn(),
      length: 0,
    };

    vi.stubGlobal('localStorage', localStorageMock);
    TestBed.configureTestingModule({});
    service = TestBed.inject(AbpLocalStorageService);
  });

  afterEach(() => {
    vi.unstubAllGlobals();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should be called getItem', () => {
    service.getItem('test');
    expect(localStorageMock.getItem).toHaveBeenCalledWith('test');
  });

  it('should be called setItem', () => {
    service.setItem('test', 'value');
    expect(localStorageMock.setItem).toHaveBeenCalledWith('test', 'value');
  });

  it('should be called removeItem', () => {
    service.removeItem('test');
    expect(localStorageMock.removeItem).toHaveBeenCalledWith('test');
  });

  it('should be called clear', () => {
    service.clear();
    expect(localStorageMock.clear).toHaveBeenCalled();
  });

  it('should be called key', () => {
    service.key(0);
    expect(localStorageMock.key).toHaveBeenCalledWith(0);
  });

  it('should be called length', () => {
    vi.stubGlobal('localStorage', { ...localStorageMock, length: 1 });
    service.length;
    expect(service.length).toBe(1);
  });
});
