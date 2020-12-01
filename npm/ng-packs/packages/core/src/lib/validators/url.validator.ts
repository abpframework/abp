import { AbstractControl, ValidatorFn } from '@angular/forms';

export interface UrlError {
  url: true;
}

export function validateUrl(): ValidatorFn {
  return (control: AbstractControl): UrlError | null => {
    if (['', null, undefined].indexOf(control.value) > -1) return null;

    return isValidUrl(control.value) ? null : { url: true };
  };
}

function isValidUrl(value: string): boolean {
  if (/^http(s)?:\/\/[^/]/.test(value) || /^ftp:\/\/[^/]/.test(value)) {
    const a = document.createElement('a');
    a.href = value;
    return !!a.host;
  }

  return false;
}
