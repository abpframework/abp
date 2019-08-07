import { Books } from '../models';

export class BooksGet {
  static readonly type = '[Books] Get';
}

export class BooksSave {
  static readonly type = '[Books] Save';
  constructor(public payload: Books.SaveRequest, public id?: string) {}
}
