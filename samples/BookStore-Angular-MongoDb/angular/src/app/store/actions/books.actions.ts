import { Books } from '../models';

export class GetBooks {
  static readonly type = '[Books] Get';
}

export class CreateUpdateBook {
  static readonly type = '[Books] Create Update Book';
  constructor(public payload: Books.CreateUpdateBookInput, public id?: string) {}
}

export class DeleteBook {
  static readonly type = '[Books] Delete';
  constructor(public id: string) {}
}
