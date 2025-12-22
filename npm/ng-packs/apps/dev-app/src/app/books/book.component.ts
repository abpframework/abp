import { LocalizationPipe, PermissionService } from '@abp/ng.core';
import {
    ButtonComponent,
    ModalCloseDirective,
    ModalComponent,
    ToasterService,
} from '@abp/ng.theme.shared';
import { ResourcePermissionManagementComponent } from '@abp/ng.permission-management';
import {
    ChangeDetectionStrategy,
    Component,
    inject,
    OnInit,
    signal,
    computed,
} from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { finalize } from 'rxjs';

import { BookService, BookDto, CreateUpdateBookDto } from '../proxy/books';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';

@Component({
    selector: 'app-book',
    templateUrl: './book.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        CommonModule,
        ReactiveFormsModule,
        LocalizationPipe,
        CurrencyPipe,
        DatePipe,
        ModalComponent,
        ModalCloseDirective,
        ButtonComponent,
        ResourcePermissionManagementComponent,
    ],
})
export class BookComponent implements OnInit {
    protected readonly bookService = inject(BookService);
    protected readonly fb = inject(FormBuilder);
    protected readonly toasterService = inject(ToasterService);
    protected readonly confirmationService = inject(ConfirmationService);
    protected readonly permissionService = inject(PermissionService);

    // Signals
    books = signal<BookDto[]>([]);
    totalCount = signal(0);
    loading = signal(false);
    modalBusy = signal(false);

    // Pagination
    currentPage = signal(0);
    maxResultCount = 10;

    totalPages = computed(() => Math.ceil(this.totalCount() / this.maxResultCount));
    pages = computed(() => {
        const total = this.totalPages();
        return Array.from({ length: Math.min(total, 5) }, (_, i) => i);
    });

    // Modal state
    isModalOpen = false;
    isEditMode = false;
    selectedBookId = '';
    selectedBookName = '';
    form!: FormGroup;

    // Permission modal
    isPermissionModalOpen = false;
    resourceName = 'MyCompanyName.MyProjectName.Books.Book';

    // Book types
    bookTypes = [
        { value: 'Adventure', label: 'Adventure' },
        { value: 'Biography', label: 'Biography' },
        { value: 'Dystopia', label: 'Dystopia' },
        { value: 'Fantastic', label: 'Fantastic' },
        { value: 'Horror', label: 'Horror' },
        { value: 'Science', label: 'Science' },
        { value: 'ScienceFiction', label: 'Science Fiction' },
        { value: 'Poetry', label: 'Poetry' },
    ];

    // Permissions
    get canCreate(): boolean {
        return this.permissionService.getGrantedPolicy('MyProjectName.Books.Create');
    }

    get canEdit(): boolean {
        return this.permissionService.getGrantedPolicy('MyProjectName.Books.Edit');
    }

    get canDelete(): boolean {
        return this.permissionService.getGrantedPolicy('MyProjectName.Books.Delete');
    }

    get canManagePermissions(): boolean {
        return this.permissionService.getGrantedPolicy('MyProjectName.Books.ManagePermissions');
    }

    ngOnInit(): void {
        this.loadBooks();
        this.buildForm();
    }

    buildForm(): void {
        this.form = this.fb.group({
            name: ['', [Validators.required, Validators.maxLength(128)]],
            bookType: ['', [Validators.required]],
            author: [''],
            price: [0, [Validators.required, Validators.min(0)]],
            publishDate: ['', [Validators.required]],
        });
    }

    loadBooks(): void {
        this.loading.set(true);
        this.bookService
            .getList({
                skipCount: this.currentPage() * this.maxResultCount,
                maxResultCount: this.maxResultCount,
            })
            .pipe(finalize(() => this.loading.set(false)))
            .subscribe({
                next: result => {
                    this.books.set(result.items || []);
                    this.totalCount.set(result.totalCount || 0);
                },
                error: () => {
                    this.toasterService.error('::ErrorLoadingBooks');
                },
            });
    }

    changePage(page: number): void {
        if (page < 0 || page >= this.totalPages()) return;
        this.currentPage.set(page);
        this.loadBooks();
    }

    openCreateModal(): void {
        this.isEditMode = false;
        this.selectedBookId = '';
        this.form.reset();
        this.isModalOpen = true;
    }

    openEditModal(book: BookDto): void {
        this.isEditMode = true;
        this.selectedBookId = book.id;
        this.form.patchValue({
            name: book.name,
            bookType: book.bookType,
            author: book.author,
            price: book.price,
            publishDate: book.publishDate?.split('T')[0] || '',
        });
        this.isModalOpen = true;
    }

    openPermissionModal(book: BookDto): void {
        this.selectedBookId = book.id;
        this.selectedBookName = book.name;
        // Use setTimeout to ensure the values are set before modal opens
        setTimeout(() => {
            this.isPermissionModalOpen = true;
        });
    }

    save(): void {
        if (this.form.invalid) return;

        this.modalBusy.set(true);
        const input: CreateUpdateBookDto = this.form.value;

        const request$ = this.isEditMode
            ? this.bookService.update(this.selectedBookId, input)
            : this.bookService.create(input);

        request$.pipe(finalize(() => this.modalBusy.set(false))).subscribe({
            next: () => {
                this.isModalOpen = false;
                this.toasterService.success('::SuccessfullySaved');
                this.loadBooks();
            },
            error: () => {
                this.toasterService.error('::ErrorSavingBook');
            },
        });
    }

    deleteBook(book: BookDto): void {
        this.confirmationService
            .warn('::AreYouSureToDelete', '::AreYouSure', { messageLocalizationParams: [book.name] })
            .subscribe((status: Confirmation.Status) => {
                if (status === Confirmation.Status.confirm) {
                    this.bookService.delete(book.id).subscribe({
                        next: () => {
                            this.toasterService.success('::SuccessfullyDeleted');
                            this.loadBooks();
                        },
                        error: () => {
                            this.toasterService.error('::ErrorDeletingBook');
                        },
                    });
                }
            });
    }
}
