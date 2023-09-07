import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { NgFor, NgIf } from '@angular/common';
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { BooksService } from './proxy';
import { RentBookComponent } from './components';

@Component({
  standalone: true,
  selector: 'app-books',
  templateUrl: './books.component.html',
  imports: [NgIf, NgFor, CoreModule, ThemeSharedModule, RentBookComponent],
  providers: [BooksService],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class BooksComponent {
  protected readonly booksService = inject(BooksService);
  protected modalVisible = false;
}
