import { HttpParameterCodec } from '@angular/common/http';

export function getPathName(url: string): string {
  const { pathname } = new URL(url, window.location.origin);
  return pathname;
}

export class WebHttpUrlEncodingCodec implements HttpParameterCodec {
  encodeKey(k: string): string {
    return encodeURIComponent(k);
  }

  encodeValue(v: string): string {
    return encodeURIComponent(v);
  }

  decodeKey(k: string): string {
    return decodeURIComponent(k);
  }

  decodeValue(v: string) {
    return decodeURIComponent(v);
  }
}
