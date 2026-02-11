export const DEFAULT_ERROR_MESSAGES = {
  defaultError: {
    title: 'An error has occurred!',
    details: 'Error detail not sent by server.',
  },
  defaultError401: {
    title: 'You are not authenticated!',
    details: 'You should be authenticated (sign in) in order to perform this operation.',
  },
  defaultError403: {
    title: 'You are not authorized!',
    details: 'You are not allowed to perform this operation.',
  },
  defaultError404: {
    title: 'Resource not found!',
    details: 'The resource requested could not found on the server.',
  },
  defaultError500: {
    title: 'Internal server error',
    details: 'Error detail not sent by server.',
  },
};

export const DEFAULT_ERROR_LOCALIZATIONS = {
  defaultError: {
    title: 'AbpUi::DefaultErrorMessage',
    details: 'AbpUi::DefaultErrorMessageDetail',
  },
  defaultError401: {
    title: 'AbpUi::DefaultErrorMessage401',
    details: 'AbpUi::DefaultErrorMessage401Detail',
  },
  defaultError403: {
    title: 'AbpUi::DefaultErrorMessage403',
    details: 'AbpUi::DefaultErrorMessage403Detail',
  },
  defaultError404: {
    title: 'AbpUi::DefaultErrorMessage404',
    details: 'AbpUi::DefaultErrorMessage404Detail',
  },
  defaultError500: {
    title: 'AbpUi::500Message',
    details: 'AbpUi::DefaultErrorMessage',
  },
};

export const CUSTOM_HTTP_ERROR_HANDLER_PRIORITY = Object.freeze({
  veryLow: -99,
  low: -9,
  normal: 0,
  high: 9,
  veryHigh: 99,
});

export const HTTP_ERROR_STATUS = {
  '401': 'AbpUi::401Message',
  '403': 'AbpUi::403Message',
  '404': 'AbpUi::404Message',
  '500': 'AbpUi::500Message',
};

export const HTTP_ERROR_DETAIL = {
  '401': 'AbpUi::DefaultErrorMessage401Detail',
  '403': 'AbpUi::DefaultErrorMessage403Detail',
  '404': 'AbpUi::DefaultErrorMessage404Detail',
  '500': 'AbpUi::DefaultErrorMessage',
};
