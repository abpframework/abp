import { FormControl } from '@angular/forms';
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
