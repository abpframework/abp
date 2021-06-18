import { Validators } from '@angular/forms';
import { ObjectExtensions } from '../lib/models/object-extensions';
import { getValidatorsFromProperty } from '../lib/utils/validation.util';

describe('Validation Utils', () => {
  describe('#getValidatorsFromProperty', () => {
    it('should return a list of validators derived from property attributes', () => {
      const property = {
        attributes: [
          {
            typeSimple: 'emailAddress',
            config: {},
          },
        ],
      } as ObjectExtensions.ExtensionPropertyDto;

      expect(getValidatorsFromProperty(property)[0]).toBe(Validators.email);
    });
  });
});
