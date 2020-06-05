import { HttpRequest } from '@angular/common/http';

export class StartLoader {
  static readonly type = '[Loader] Start';
  constructor(public payload: HttpRequest<any>) {}
}

export class StopLoader {
  static readonly type = '[Loader] Stop';
  constructor(public payload: HttpRequest<any>) {}
}
