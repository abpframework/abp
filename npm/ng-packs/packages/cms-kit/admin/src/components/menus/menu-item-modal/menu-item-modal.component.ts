import {
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  inject,
  Injector,
  input,
  OnInit,
  output,
  signal,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormControl, FormsModule } from '@angular/forms';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { NgbNavModule, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { forkJoin, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { LocalizationPipe } from '@abp/ng.core';
import {
  ExtensibleFormComponent,
  FormPropData,
  generateFormFromProps,
} from '@abp/ng.components/extensible';
import {
  ModalComponent,
  ModalCloseDirective,
  ButtonComponent,
  ToasterService,
} from '@abp/ng.theme.shared';
import { dasharize } from '@abp/ng.cms-kit';
import {
  MenuItemAdminService,
  MenuItemDto,
  MenuItemWithDetailsDto,
  MenuItemCreateInput,
  MenuItemUpdateInput,
  PageLookupDto,
} from '@abp/ng.cms-kit/proxy';

export interface MenuItemModalVisibleChange {
  visible: boolean;
  refresh: boolean;
}

// Constants
const PAGE_LOOKUP_MAX_RESULT = 1000;
const PAGE_SEARCH_MAX_RESULT = 100;
const PAGE_SEARCH_DEBOUNCE_MS = 300;
const TABS = {
  URL: 'url',
  PAGE: 'page',
} as const;

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'abp-menu-item-modal',
  templateUrl: './menu-item-modal.component.html',
  imports: [
    ExtensibleFormComponent,
    LocalizationPipe,
    ReactiveFormsModule,
    CommonModule,
    NgxValidateCoreModule,
    ModalComponent,
    ModalCloseDirective,
    ButtonComponent,
    NgbNavModule,
    NgbDropdownModule,
    FormsModule,
  ],
  styles: [
    `
      .dropdown-toggle::after {
        display: none !important;
      }
    `,
  ],
})
export class MenuItemModalComponent implements OnInit {
  // Injected services
  private readonly menuItemService = inject(MenuItemAdminService);
  private readonly injector = inject(Injector);
  private readonly toasterService = inject(ToasterService);
  private readonly destroyRef = inject(DestroyRef);

  // Inputs/Outputs
  readonly selected = input<MenuItemWithDetailsDto | MenuItemDto>();
  readonly parentId = input<string | null>();
  readonly visible = input<boolean>(true);
  readonly visibleChange = output<MenuItemModalVisibleChange>();

  // Form state
  form: FormGroup;
  readonly activeTab = signal<string>(TABS.URL);

  // Page search state
  readonly pages = signal<PageLookupDto[]>([]);
  readonly selectedPage = signal<PageLookupDto | null>(null);
  readonly pageSearchText = signal('');
  readonly filteredPages = signal<PageLookupDto[]>([]);

  // Search subject for debouncing
  private readonly pageSearchSubject = new Subject<string>();

  get isPageSelected(): boolean {
    return !!this.form?.get('pageId')?.value;
  }

  ngOnInit() {
    this.setupPageSearch();
    this.initializeComponent();
  }

  /**
   * Sets up debounced page search functionality
   */
  private setupPageSearch(): void {
    this.pageSearchSubject
      .pipe(
        debounceTime(PAGE_SEARCH_DEBOUNCE_MS),
        distinctUntilChanged(),
        switchMap(searchText => {
          if (!searchText?.trim()) {
            // Show all pages when search is cleared
            return this.menuItemService.getPageLookup({ maxResultCount: PAGE_LOOKUP_MAX_RESULT });
          }
          return this.menuItemService.getPageLookup({
            filter: searchText.trim(),
            maxResultCount: PAGE_SEARCH_MAX_RESULT,
          });
        }),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe({
        next: result => {
          this.filteredPages.set(result.items || []);
        },
        error: () => {
          this.filteredPages.set([]);
        },
      });
  }

  /**
   * Initializes the component based on create or edit mode
   */
  private initializeComponent(): void {
    const selectedItem = this.selected();

    if (selectedItem?.id) {
      this.loadMenuItemForEdit(selectedItem.id);
    } else {
      this.loadPagesForCreate();
      this.buildForm();
    }
  }

  /**
   * Loads menu item data and pages for edit mode
   */
  private loadMenuItemForEdit(menuItemId: string): void {
    forkJoin({
      menuItem: this.menuItemService.get(menuItemId),
      pages: this.menuItemService.getPageLookup({ maxResultCount: PAGE_LOOKUP_MAX_RESULT }),
    })
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: ({ menuItem, pages }) => {
          this.pages.set(pages.items || []);
          this.filteredPages.set(pages.items || []);
          this.buildForm(menuItem);
          this.initializePageSelection(menuItem);
        },
        error: () => {
          this.toasterService.error('AbpUi::ErrorMessage');
        },
      });
  }

  /**
   * Loads pages for create mode
   */
  private loadPagesForCreate(): void {
    this.menuItemService
      .getPageLookup({ maxResultCount: PAGE_LOOKUP_MAX_RESULT })
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: result => {
          this.pages.set(result.items || []);
          this.filteredPages.set(result.items || []);
        },
        error: () => {
          this.toasterService.error('AbpUi::ErrorMessage');
        },
      });
  }

  /**
   * Initializes page selection when editing a menu item with a page
   */
  private initializePageSelection(menuItem: MenuItemWithDetailsDto | MenuItemDto): void {
    if (menuItem.pageId) {
      this.activeTab.set(TABS.PAGE);
      const page = this.pages().find(p => p.id === menuItem.pageId) || null;
      this.selectedPage.set(page);

      const url = page ? this.generateUrlFromPage(page) : menuItem.url || '';
      this.form.patchValue({ pageId: menuItem.pageId, url }, { emitEvent: false });
      this.pageSearchText.set(page?.title || '');
    } else if (menuItem.url) {
      this.activeTab.set(TABS.URL);
      this.form.patchValue({ url: menuItem.url, pageId: null }, { emitEvent: false });
    }
  }

  /**
   * Generates a URL from a page's slug or title
   */
  private generateUrlFromPage(page: PageLookupDto): string {
    if (!page) return '';

    const source = page.slug || page.title;
    if (!source) return '';

    return '/' + dasharize(source);
  }

  /**
   * Handles page search input changes
   */
  onPageSearchChange(searchText: string): void {
    this.pageSearchText.set(searchText);
    if (!searchText?.trim()) {
      this.filteredPages.set(this.pages());
      return;
    }
    this.pageSearchSubject.next(searchText);
  }

  /**
   * Handles dropdown open event
   */
  onDropdownOpen(): void {
    if (!this.pageSearchText()?.trim()) {
      this.filteredPages.set(this.pages());
    }
  }

  /**
   * Handles page selection from dropdown
   */
  selectPage(page: PageLookupDto): void {
    if (!page) return;

    this.selectedPage.set(page);
    const url = this.generateUrlFromPage(page);

    this.form.patchValue({ pageId: page.id, url }, { emitEvent: false });
    this.pageSearchText.set(page.title || '');
  }

  /**
   * Clears the selected page
   */
  clearPageSelection(): void {
    this.form.patchValue({ pageId: null }, { emitEvent: false });
    this.selectedPage.set(null);
    this.pageSearchText.set('');
    this.filteredPages.set(this.pages());
  }

  /**
   * Builds the reactive form for menu item creation/editing
   */
  private buildForm(menuItem?: MenuItemWithDetailsDto | MenuItemDto): void {
    const data = new FormPropData(this.injector, menuItem || {});
    const baseForm = generateFormFromProps(data);
    const parentId = this.parentId() || menuItem?.parentId || null;

    this.form = new FormGroup({
      ...baseForm.controls,
      url: new FormControl(menuItem?.url || ''),
      pageId: new FormControl(menuItem?.pageId || null),
      parentId: new FormControl(parentId),
    });

    this.loadAvailableOrder(parentId, menuItem?.id);
  }

  /**
   * Loads the available menu order for new menu items
   */
  private loadAvailableOrder(parentId: string | null, menuItemId?: string): void {
    if (menuItemId) return;

    const order$ = parentId
      ? this.menuItemService.getAvailableMenuOrder(parentId)
      : this.menuItemService.getAvailableMenuOrder();

    order$.pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
      next: order => {
        this.form.patchValue({ order });
      },
    });
  }

  /**
   * Handles modal visibility changes
   */
  onVisibleChange(visible: boolean, refresh = false): void {
    this.visibleChange.emit({ visible, refresh });
  }

  /**
   * Handles tab changes
   */
  onTabChange(activeId: string): void {
    this.activeTab.set(activeId);
  }

  /**
   * Handles URL input changes - clears page selection if URL is manually entered
   */
  onUrlInput(): void {
    const urlValue = this.form.get('url')?.value;
    if (urlValue && this.form.get('pageId')?.value) {
      this.clearPageSelection();
      if (this.activeTab() === TABS.PAGE) {
        this.activeTab.set(TABS.URL);
      }
    }
  }

  /**
   * Saves the menu item (create or update)
   */
  save(): void {
    if (!this.form.valid) {
      return;
    }

    const formValue = this.prepareFormValue();
    const selectedItem = this.selected();
    const isEditMode = !!selectedItem?.id;

    const observable$ = isEditMode
      ? this.updateMenuItem(selectedItem.id, formValue, selectedItem as MenuItemWithDetailsDto)
      : this.createMenuItem(formValue);

    observable$.pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
      next: () => {
        this.onVisibleChange(false, true);
        this.toasterService.success('AbpUi::SavedSuccessfully');
      },
      error: () => {
        this.toasterService.error('AbpUi::ErrorMessage');
      },
    });
  }

  /**
   * Prepares form value ensuring mutual exclusivity between pageId and url
   */
  private prepareFormValue(): Partial<MenuItemCreateInput | MenuItemUpdateInput> {
    const formValue = { ...this.form.value };

    if (formValue.pageId) {
      const page = this.pages().find(p => p.id === formValue.pageId);
      if (page) {
        formValue.url = this.generateUrlFromPage(page);
      }
    } else if (formValue.url) {
      formValue.pageId = null;
    }

    return {
      ...formValue,
      url: formValue.url || undefined,
      pageId: formValue.pageId || undefined,
    };
  }

  /**
   * Creates a new menu item
   */
  private createMenuItem(formValue: Partial<MenuItemCreateInput>) {
    const createInput: MenuItemCreateInput = formValue as MenuItemCreateInput;
    return this.menuItemService.create(createInput);
  }

  /**
   * Updates an existing menu item
   */
  private updateMenuItem(
    id: string,
    formValue: Partial<MenuItemUpdateInput>,
    selectedItem: MenuItemWithDetailsDto,
  ) {
    const updateInput: MenuItemUpdateInput = {
      ...formValue,
      concurrencyStamp: selectedItem.concurrencyStamp,
    } as MenuItemUpdateInput;
    return this.menuItemService.update(id, updateInput);
  }
}
