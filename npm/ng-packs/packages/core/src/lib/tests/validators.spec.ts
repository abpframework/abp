import { FormControl, Validators } from '@angular/forms';
import { AbpValidators, validateMinAge, validateRange } from '../validators';
import { validateCreditCard } from '../validators/credit-card.validator';
import { validateRequired } from '../validators/required.validator';
import { validateStringLength } from '../validators/string-length.validator';
import { validateUrl } from '../validators/url.validator';

describe('Validators', () => {
  describe('Credit Card Validator', () => {
    const error = { creditCard: true };

    test.each`
      input                                       | expected
      ${undefined}                                | ${null}
      ${null}                                     | ${null}
      ${''}                                       | ${null}
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
  });

  describe('Email Validator', () => {
    it('should return email validator of Angular', () => {
      expect(AbpValidators.emailAddress()).toBe(Validators.email);
    });
  });

  describe('Min Age Validator', () => {
    const date = new Date();
    const year = date.getFullYear();
    const month = date.getMonth() + 1;
    const day = date.getDate();
    const sameDay21yAgo = `${year - 21}-${month}-${day}`;
    const nextDay21yAgo = `${year - 21}-${month}-${day + 1}`;

    test.each`
      input            | options        | expected
      ${null}          | ${undefined}   | ${null}
      ${undefined}     | ${undefined}   | ${null}
      ${''}            | ${undefined}   | ${null}
      ${0}             | ${undefined}   | ${null}
      ${Infinity}      | ${undefined}   | ${{ minAge: { age: 18 } }}
      ${sameDay21yAgo} | ${undefined}   | ${null}
      ${sameDay21yAgo} | ${{ age: 21 }} | ${null}
      ${nextDay21yAgo} | ${{ age: 21 }} | ${{ minAge: { age: 21 } }}
    `(
      'should return $expected when input is $input and options are $options',
      ({ input, options, expected }) => {
        const control = new FormControl(input, [validateMinAge(options)]);
        control.markAsDirty({ onlySelf: true });
        control.updateValueAndValidity({ onlySelf: true, emitEvent: false });

        expect(control.errors).toEqual(expected);
      },
    );
  });

  describe('Range Validator', () => {
    test.each`
      input        | options                       | expected
      ${null}      | ${undefined}                  | ${null}
      ${undefined} | ${undefined}                  | ${null}
      ${''}        | ${undefined}                  | ${null}
      ${0}         | ${undefined}                  | ${null}
      ${Infinity}  | ${undefined}                  | ${null}
      ${'-1'}      | ${{ minimum: 0 }}             | ${{ range: { min: 0, max: Infinity } }}
      ${-1}        | ${{ minimum: 0 }}             | ${{ range: { min: 0, max: Infinity } }}
      ${2}         | ${{ minimum: 3, maximum: 5 }} | ${{ range: { min: 3, max: 5 } }}
      ${3}         | ${{ minimum: 3, maximum: 5 }} | ${null}
      ${5}         | ${{ minimum: 3, maximum: 5 }} | ${null}
      ${6}         | ${{ minimum: 3, maximum: 5 }} | ${{ range: { min: 3, max: 5 } }}
    `(
      'should return $expected when input is $input and options are $options',
      ({ input, options, expected }) => {
        const control = new FormControl(input, [validateRange(options)]);
        control.markAsDirty({ onlySelf: true });
        control.updateValueAndValidity({ onlySelf: true, emitEvent: false });

        expect(control.errors).toEqual(expected);
      },
    );
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
  });

  describe('String Length Validator', () => {
    test.each`
      input        | options                 | expected
      ${null}      | ${undefined}            | ${null}
      ${undefined} | ${undefined}            | ${null}
      ${''}        | ${undefined}            | ${null}
      ${'ab'}      | ${{ minimumLength: 3 }} | ${{ minlength: { requiredLength: 3 } }}
      ${'abp'}     | ${{ minimumLength: 3 }} | ${null}
      ${'abp'}     | ${{ maximumLength: 2 }} | ${{ maxlength: { requiredLength: 2 } }}
      ${'abp'}     | ${{ maximumLength: 3 }} | ${null}
    `(
      'should return $expected when input is $input and options are $options',
      ({ input, options, expected }) => {
        const control = new FormControl(input, [validateStringLength(options)]);
        control.markAsDirty({ onlySelf: true });
        control.updateValueAndValidity({ onlySelf: true, emitEvent: false });

        expect(control.errors).toEqual(expected);
      },
    );
  });

  describe('Url Validator', () => {
    const error = { url: true };

    test.each`
      input                     | expected
      ${undefined}              | ${null}
      ${null}                   | ${null}
      ${''}                     | ${null}
      ${'http://x'}             | ${null}
      ${'http:///x'}            | ${error}
      ${'https://x'}            | ${null}
      ${'https:///x'}           | ${error}
      ${'ftp://x'}              | ${null}
      ${'ftp:///x'}             | ${error}
      ${'http://x.com'}         | ${null}
      ${'http://x.photography'} | ${null}
      ${'http://www.x.org'}     | ${null}
      ${'http://sub.x.gov.tr'}  | ${null}
      ${'x'}                    | ${error}
      ${'x.com'}                | ${error}
      ${'www.x.org'}            | ${error}
      ${'sub.x.gov.tr'}         | ${error}
    `('should return $expected when input is $input', ({ input, expected }) => {
      const control = new FormControl(input, [validateUrl()]);
      control.markAsDirty({ onlySelf: true });
      control.updateValueAndValidity({ onlySelf: true, emitEvent: false });

      expect(control.errors).toEqual(expected);
    });
  });
});
