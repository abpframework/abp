import { HttpContextToken } from '@angular/common/http';

export const IS_EXTERNAL_REQUEST = new HttpContextToken<boolean>(() => false);
