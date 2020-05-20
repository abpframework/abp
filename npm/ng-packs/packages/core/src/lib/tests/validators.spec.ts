import { FormControl } from '@angular/forms';
import { validateRequired } from '../validators/required.validator';

describe('Validators', () => {
  describe('Required Validator', () => {
    const error = { required: true };

    test.each`
      input        | options                         | expected
      ${0}         | ${undefined}                    | ${null}
      ${false}     | ${undefined}                    | ${null}
      ${null}      | ${undefined}                    | ${error}
      ${undefined} | ${undefined}                    | ${error}
      ${''}        | ${undefined}                    | ${error}
      ${''}        | ${{}}                           | ${error}
      ${''}        | ${{ allowEmptyStrings: false }} | ${error}
      ${''}        | ${{ allowEmptyStrings: true }}  | ${null}
    `(
      'should return $expected when input is $input and options are $options',
      ({ input, options, expected }) => {
        const control = new FormControl(input, [validateRequired(options)]);
        control.markAsDirty({ onlySelf: true });
        control.updateValueAndValidity({ onlySelf: true, emitEvent: false });

        expect(control.errors).toEqual(expected);
      },
    );

    it('should return null when control is pristine', () => {
      const invalidUrl = '';
      const control = new FormControl(invalidUrl, [validateRequired()]);
      // control is not dirty

      expect(control.valid).toBe(true);
    });
  });
});
