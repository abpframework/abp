import { HttpRequest } from '@angular/common/http';

export class LoaderStart {
  static readonly type = '[Loader] Start';
  constructor(public payload: HttpRequest<any>) {}
}

export class LoaderStop {
  static readonly type = '[Loader] Stop';
  constructor(public payload: HttpRequest<any>) {}
}
