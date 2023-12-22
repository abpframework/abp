export const DEFAULT_VALIDATION_BLUEPRINTS = {
  creditCard: 'AbpValidation::ThisFieldIsNotAValidCreditCardNumber.',
  email: 'AbpValidation::ThisFieldIsNotAValidEmailAddress.',
  invalid: 'AbpValidation::ThisFieldIsNotValid.',
  max: 'AbpValidation::ThisFieldMustBeLessOrEqual{0}[{{ max }}]',
  maxlength:
    'AbpValidation::ThisFieldMustBeAStringOrArrayTypeWithAMaximumLengthOf{0}[{{ requiredLength }}]',
  min: 'AbpValidation::ThisFieldMustBeGreaterThanOrEqual{0}[{{ min }}]',
  minlength:
    'AbpValidation::ThisFieldMustBeAStringOrArrayTypeWithAMinimumLengthOf{0}[{{ requiredLength }}]',
  ngbDate: 'AbpValidation::ThisFieldIsNotValid.',
  passwordMismatch: 'AbpIdentity::Volo.Abp.Identity:PasswordConfirmationFailed',
  range: 'AbpValidation::ThisFieldMustBeBetween{0}And{1}[{{ min }},{{ max }}]',
  required: 'AbpValidation::ThisFieldIsRequired.',
  url: 'AbpValidation::ThisFieldIsNotAValidFullyQualifiedHttpHttpsOrFtpUrl',
  passwordRequiresLower: 'AbpIdentity::Volo.Abp.Identity:PasswordRequiresLower',
  passwordRequiresUpper: 'AbpIdentity::Volo.Abp.Identity:PasswordRequiresUpper',
  passwordRequiresDigit: 'AbpIdentity::Volo.Abp.Identity:PasswordRequiresDigit',
  passwordRequiresNonAlphanumeric: 'AbpIdentity::Volo.Abp.Identity:PasswordRequiresNonAlphanumeric',
  usernamePattern: 'AbpIdentity::Volo.Abp.Identity:InvalidUserName[{{ actualValue }}]',
  customMessage: '{{ customMessage }}'
};