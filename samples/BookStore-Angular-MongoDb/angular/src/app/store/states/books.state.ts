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
  static getBooks(state: Books.State) {
    return state.books.items || [];
  }

  constructor(private booksService: BooksService) {}

  @Action(GetBooks)
  get(ctx: StateContext<Books.State>) {
    return this.booksService.get().pipe(
      tap(booksResponse => {
        ctx.patchState({
          books: booksResponse,
        });
      }),
    );
  }

  @Action(CreateUpdateBook)
  save(ctx: StateContext<Books.State>, action: CreateUpdateBook) {
    let request;

    if (action.id) {
      request = this.booksService.update(action.payload, action.id);
    } else {
      request = this.booksService.create(action.payload);
    }

    return request.pipe(switchMap(() => ctx.dispatch(new GetBooks())));
  }

  @Action(DeleteBook)
  delete(ctx: StateContext<Books.State>, action: DeleteBook) {
    return this.booksService.delete(action.id).pipe(switchMap(() => ctx.dispatch(new GetBooks())));
  }
}
