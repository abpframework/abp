import { registerLocaleForEsBuild, storeLocaleData } from '../../../locale/src/utils/register-locale';

describe('registerLocaleForEsBuild', () => {
  it('should always return a promise for unsupported locales', async () => {
    const errorHandlerFn = vi.fn(({ resolve }) => resolve({ default: null }));

    const result = registerLocaleForEsBuild({ errorHandlerFn })('xx');

    expect(result).toBeInstanceOf(Promise);
    await expect(result).resolves.toEqual({ default: null });
    expect(errorHandlerFn).toHaveBeenCalledWith(
      expect.objectContaining({ locale: 'xx', error: expect.any(Error) }),
    );
  });

  it('should load supported locales', async () => {
    await expect(registerLocaleForEsBuild()('ko')).resolves.toEqual(
      expect.objectContaining({ default: expect.anything() }),
    );
  });

  it('should resolve stored locale data for unsupported locales', async () => {
    const locale = 'xx-extra';
    const data = { hello: 'world' };
    storeLocaleData(data, locale);

    await expect(registerLocaleForEsBuild()(locale)).resolves.toEqual({ default: data });
  });
});
