import { State, Action, StateContext, Selector } from "@ngxs/store";
import {
  GetBooks,
  CreateUpdateBook,
  DeleteBook
} from "../actions/books.actions";
import { Books } from "../models/books";
import { BooksService } from "../../books/shared/books.service";
import { tap } from "rxjs/operators";

@State<Books.State>({
  name: "BooksState",
  defaults: { books: {} } as Books.State
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
          books: booksResponse
        });
      })
    );
  }

  @Action(CreateUpdateBook)
  save(ctx: StateContext<Books.State>, action: CreateUpdateBook) {
    if (action.id) {
      return this.booksService.update(action.payload, action.id);
    } else {
      return this.booksService.create(action.payload);
    }
  }

  @Action(DeleteBook)
  delete(ctx: StateContext<Books.State>, action: DeleteBook) {
    return this.booksService.delete(action.id);
  }
}
