import { FormControl } from '@angular/forms';
import { validateRange } from '../validators';
import { validateCreditCard } from '../validators/credit-card.validator';
import { validateRequired } from '../validators/required.validator';

describe('Validators', () => {
  describe('Credit Card Validator', () => {
    const error = { creditCardNumber: true };

    test.each`
      input                                       | expected
      ${'0'}                                      | ${error}
      ${'5105105105105100' /* Mastercard */}      | ${null}
      ${'5105105105105101' /* Mastercard */}      | ${error}
      ${'5105 1051 0510 5100'}                    | ${null}
      ${'5105-1051-0510-5100'}                    | ${null}
      ${'5105 - 1051 - 0510 - 5100'}              | ${null}
      ${'4111111111111111' /*Visa*/}              | ${null}
      ${'4111111111111112' /*Visa*/}              | ${error}
      ${'4012888888881881' /* Visa */}            | ${null}
      ${'4012888888881882' /* Visa */}            | ${error}
      ${'4222222222222' /* Visa */}               | ${null}
      ${'4222222222223' /* Visa */}               | ${error}
      ${'378282246310005' /* American Express */} | ${null}
      ${'378282246310006' /* American Express */} | ${error}
      ${'6011111111111117' /* Discover */}        | ${null}
      ${'6011111111111118' /* Discover */}        | ${error}
    `('should return $expected when input is $input', ({ input, expected }) => {
      const control = new FormControl(input, [validateCreditCard()]);
      control.markAsDirty({ onlySelf: true });
      control.updateValueAndValidity({ onlySelf: true, emitEvent: false });

      expect(control.errors).toEqual(expected);
    });

    it('should return null when control is pristine', () => {
      const invalidNumber = '5105105105105101';
      const control = new FormControl(invalidNumber, [validateCreditCard()]);
      // control is not dirty

      expect(control.valid).toBe(true);
    });
  });

  describe('Range Validator', () => {
    test.each`
      input        | options                       | expected
      ${null}      | ${undefined}                  | ${{ min: 0, max: Infinity }}
      ${undefined} | ${undefined}                  | ${{ min: 0, max: Infinity }}
      ${''}        | ${undefined}                  | ${{ min: 0, max: Infinity }}
      ${0}         | ${undefined}                  | ${null}
      ${Infinity}  | ${undefined}                  | ${null}
      ${null}      | ${{ minimum: 0 }}             | ${{ min: 0, max: Infinity }}
      ${undefined} | ${{ minimum: 0 }}             | ${{ min: 0, max: Infinity }}
      ${''}        | ${{ minimum: 0 }}             | ${{ min: 0, max: Infinity }}
      ${0}         | ${{ minimum: 0 }}             | ${null}
      ${2}         | ${{ minimum: 3, maximum: 5 }} | ${{ min: 3, max: 5 }}
      ${3}         | ${{ minimum: 3, maximum: 5 }} | ${null}
      ${5}         | ${{ minimum: 3, maximum: 5 }} | ${null}
      ${6}         | ${{ minimum: 3, maximum: 5 }} | ${{ min: 3, max: 5 }}
    `(
      'should return $expected when input is $input and options are $options',
      ({ input, options, expected }) => {
        const control = new FormControl(input, [validateRange(options)]);
        control.markAsDirty({ onlySelf: true });
        control.updateValueAndValidity({ onlySelf: true, emitEvent: false });

        expect(control.errors).toEqual(expected);
      },
    );

    it('should return null when control is pristine', () => {
      const invalidUrl = '';
      const control = new FormControl(invalidUrl, [validateRange({ minimum: 3 })]);
      // control is not dirty

      expect(control.valid).toBe(true);
    });
  });

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
