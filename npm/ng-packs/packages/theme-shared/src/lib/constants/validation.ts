export const DEFAULT_VALIDATION_BLUEPRINTS = {
  creditCard: 'AbpValidation::ThisFieldIsNotAValidCreditCardNumber.',
  email: 'AbpValidation::ThisFieldIsNotAValidEmailAddress.',
  invalid: 'AbpValidation::ThisFieldIsNotValid.',
  max: 'AbpValidation::ThisFieldMustBeBetween{0}And{1}[{{ min }},{{ max }}]',
  maxlength:
    'AbpValidation::ThisFieldMustBeAStringOrArrayTypeWithAMaximumLengthOf{0}[{{ requiredLength }}]',
  min: 'AbpValidation::ThisFieldMustBeBetween{0}And{1}[{{ min }},{{ max }}]',
  minlength:
    'AbpValidation::ThisFieldMustBeAStringOrArrayTypeWithAMinimumLengthOf{0}[{{ requiredLength }}]',
  ngbDate: 'AbpValidation::ThisFieldIsNotValid.',
  passwordMismatch: 'AbpIdentity::Volo.Abp.Identity:PasswordConfirmationFailed',
  range: 'AbpValidation::ThisFieldMustBeBetween{0}And{1}[{{ min }},{{ max }}]',
  required: 'AbpValidation::ThisFieldIsRequired.',
  url: 'AbpValidation::ThisFieldIsNotAValidFullyQualifiedHttpHttpsOrFtpUrl',
};
