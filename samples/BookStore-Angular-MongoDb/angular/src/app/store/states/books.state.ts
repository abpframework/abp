import { State, Action, StateContext } from "@ngxs/store";
import { BooksGet } from "../actions/books.actions";
import { Books } from "../models/books";
import { BooksService } from "src/app/shared/services/books.service";
import { tap } from "rxjs/operators";

@State<Books.State>({
  name: "BooksState",
  defaults: {} as Books.State
})
export class BooksState {
  constructor(private booksService: BooksService) {}

  @Action(BooksGet)
  booksAction(
    { getState, patchState }: StateContext<Books.State>,
    { payload }: BooksGet
  ) {
    return this.booksService.get().pipe(
      tap(data => {
        patchState({
          data
        });
      })
    );
  }
}
