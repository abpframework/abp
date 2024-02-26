import { SpectatorService, SpyObject, createServiceFactory } from "@ngneat/spectator/jest";
import { RememberMeService } from "../services/remember-me.service";
import { AbpLocalStorageService } from "@abp/ng.core";



describe('RememberMeService', () => {
  const key = 'remember_me';
  let spectator: SpectatorService<RememberMeService>;
  let rememberMeService: RememberMeService;
  let abpLocalStorageService: SpyObject<AbpLocalStorageService>;

  const createService = createServiceFactory({
    service: RememberMeService,
    mocks: [AbpLocalStorageService]
  });


  beforeEach(() => {
    spectator = createService();
    rememberMeService = spectator.inject(RememberMeService);
    abpLocalStorageService = spectator.inject(AbpLocalStorageService);
  });

  it('should be created', () => {
    expect(1).toBe(1);
    expect(rememberMeService).toBeTruthy();
    expect(abpLocalStorageService).toBeTruthy();
  });

  it('should set remember me', () => {
    rememberMeService.set(true);
    expect(abpLocalStorageService.setItem).toHaveBeenCalledWith(key, 'true');
    expect(abpLocalStorageService.setItem).toHaveBeenCalledTimes(1);
  });

  it('should remove remember me', () => {
    rememberMeService.remove();
    expect(abpLocalStorageService.removeItem).toHaveBeenCalledWith(key);
    expect(abpLocalStorageService.removeItem).toBeCalledTimes(1);
  });

  it('if notting has ben setted, it should return false value', () => {
    expect(rememberMeService.get()).toBe(false);
  });

  it('should return true value', () => {
    abpLocalStorageService.getItem.mockReturnValueOnce('true');
    expect(rememberMeService.get()).toBe(true);
  });

  it('should return false value', () => {
    abpLocalStorageService.getItem.mockReturnValueOnce('false');
    expect(rememberMeService.get()).toBe(false);
  });

  it('should return true when parsed token is setted to true', () => {
    const data = { "remember_me": "True" };
    const base64_encoded = btoa(JSON.stringify(data));
    const tokenWithValueTrue = "random." + base64_encoded + ".random";
    expect(rememberMeService.getFromToken(tokenWithValueTrue)).toBe(true);
  });

  it('should return false when value is not setted(undefined)', () => {
    const data = {};
    const base64_encoded = btoa(JSON.stringify(data));
    const tokenWithValueTrue = "random." + base64_encoded + ".random";
    expect(rememberMeService.getFromToken(tokenWithValueTrue)).toBe(false);
  });

});