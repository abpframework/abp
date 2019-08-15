import { State, Action, StateContext, Selector } from '@ngxs/store';
import { Books } from '../models/books';
import { BooksService } from '../../books/shared/books.service';
import { tap, switchMap } from 'rxjs/operators';
import { GetBooks, CreateUpdateBook, DeleteBook } from '../actions/books.actions';

@State<Books.State>({
  name: 'BooksState',
  defaults: { books: {} } as Books.State,
})
export class BooksState {
  @Selector()
  static getBooks({ books }: Books.State) {
    return books.items || [];
  }

  constructor(private booksService: BooksService) {}

  @Action(GetBooks)
  get({ patchState }: StateContext<Books.State>) {
    return this.booksService.get().pipe(
      tap(books => {
        patchState({
          books,
        });
      }),
    );
  }

  @Action(CreateUpdateBook)
  save({ dispatch }: StateContext<Books.State>, { payload, id }: CreateUpdateBook) {
    let request;

    if (id) {
      request = this.booksService.update(payload, id);
    } else {
      request = this.booksService.create(payload);
    }

    return request.pipe(switchMap(() => dispatch(new GetBooks())));
  }

  @Action(DeleteBook)
  delete({ dispatch }: StateContext<Books.State>, { id }: DeleteBook) {
    return this.booksService.delete(id).pipe(switchMap(() => dispatch(new GetBooks())));
  }
}
