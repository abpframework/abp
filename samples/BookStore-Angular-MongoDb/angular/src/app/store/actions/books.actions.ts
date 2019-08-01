export class BooksGet {
  static readonly type = '[Books] Get';
  constructor(public readonly payload?: any) { }
}
