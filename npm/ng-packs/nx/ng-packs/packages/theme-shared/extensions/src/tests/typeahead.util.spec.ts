import { ExtensionPropertyUiLookupDto } from '@abp/ng.core';
import { of } from 'rxjs';
import { createTypeaheadOptions } from '../lib/utils/typeahead.util';

const lookup: ExtensionPropertyUiLookupDto = {
  url: 'url',
  resultListPropertyName: 'list',
  displayPropertyName: 'text',
  valuePropertyName: 'id',
  filterParamName: 'filter',
};

describe('Typeahead Utils', () => {
  describe('#createTypeaheadOptions', () => {
    it('should return observable empty array when search text does not exist', async () => {
      const list = await createTypeaheadOptions(null)(null, null).toPromise();
      expect(list).toEqual([]);
    });

    it('should call request method of RestService with lookup url, filter param and search text', async () => {
      const data = createData([]);
      const service = data.getInjected();
      await createTypeaheadOptions(lookup)(data, 'x').toPromise();
      expect(service.request).toHaveBeenCalledTimes(1);
      expect(service.request).toHaveBeenCalledWith(
        {
          method: 'GET',
          url: 'url',
          params: {
            filter: 'x',
          },
        },
        { apiName: 'Default' },
      );
    });

    it('should return options based on given lookup data', async () => {
      const data = createData([
        {
          text: 'foo',
          id: 'bar',
        },
        {
          text: 'baz',
          id: 'qux',
        },
      ]);

      const options = await createTypeaheadOptions(lookup)(data, 'x').toPromise();
      expect(options).toEqual([
        {
          key: 'foo',
          value: 'bar',
        },
        {
          key: 'baz',
          value: 'qux',
        },
      ]);
    });
  });
});

function createData(list: { text: string; id: string }[]): any {
  const service = { request: jest.fn(() => of({ list })) };

  return {
    getInjected: () => service,
    index: 0,
    record: null,
  };
}
