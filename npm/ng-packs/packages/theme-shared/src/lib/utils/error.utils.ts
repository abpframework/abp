import { LocalizationParam } from '@abp/ng.core';
import { DEFAULT_ERROR_LOCALIZATIONS, DEFAULT_ERROR_MESSAGES } from '../constants/default-errors';

export function getErrorFromRequestBody(body: { details?: string; message?: string } | undefined) {
  let message: LocalizationParam;
  let title: LocalizationParam;

  if (body.details) {
    message = body.details;
    title = body.message;
  } else if (body.message) {
    title = {
      key: DEFAULT_ERROR_LOCALIZATIONS.defaultError.title,
      defaultValue: DEFAULT_ERROR_MESSAGES.defaultError.title,
    };
    message = body.message;
  } else {
    message = {
      key: DEFAULT_ERROR_LOCALIZATIONS.defaultError.title,
      defaultValue: DEFAULT_ERROR_MESSAGES.defaultError.title,
    };
    title = '';
  }

  return { message, title };
}
