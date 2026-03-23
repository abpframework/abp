import {
  AfterViewInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  computed,
  inject,
  Injector,
  LOCALE_ID,
  OnDestroy,
  PLATFORM_ID,
  signal,
  TemplateRef,
  TrackByFunction,
  input,
  effect,
  output,
  contentChild,
  viewChild,
} from '@angular/core';
import { AsyncPipe, isPlatformBrowser, NgComponentOutlet, NgTemplateOutlet } from '@angular/common';

import { Observable, filter, map, Subject, debounceTime, distinctUntilChanged } from 'rxjs';

import { NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule, SelectionType, DatatableComponent } from '@swimlane/ngx-datatable';

import {
  ABP,
  ConfigStateService,
  ListService,
  LocalizationPipe,
  PermissionDirective,
  PermissionService,
  TimezoneService,
  UtcToLocalPipe,
} from '@abp/ng.core';
import {
  AbpVisibleDirective,
  NgxDatatableDefaultDirective,
  NgxDatatableListDirective,
} from '@abp/ng.theme.shared';

import { ePropType } from '../../enums/props.enum';
import { EntityActionList } from '../../models/entity-actions';
import { EntityProp, EntityPropList } from '../../models/entity-props';
import { ReadonlyPropData } from '../../models/props';
import { ExtensionsService } from '../../services/extensions.service';
import {
  ENTITY_PROP_TYPE_CLASSES,
  EXTENSIONS_IDENTIFIER,
  PROP_DATA_STREAM,
  ROW_RECORD,
} from '../../tokens/extensions.token';
import { GridActionsComponent } from '../grid-actions/grid-actions.component';
import { ExtensibleTableRowDetailComponent } from './extensible-table-row-detail';
import { RowDetailContext } from '../../models/row-detail';

const DEFAULT_ACTIONS_COLUMN_WIDTH = 150;

@Component({
  exportAs: 'abpExtensibleTable',
  selector: 'abp-extensible-table',
  imports: [
    AbpVisibleDirective,
    NgxDatatableModule,
    GridActionsComponent,
    NgbTooltip,
    NgxDatatableDefaultDirective,
    NgxDatatableListDirective,
    PermissionDirective,
    LocalizationPipe,
    UtcToLocalPipe,
    AsyncPipe,
    NgTemplateOutlet,
    NgComponentOutlet,
  ],
  templateUrl: './extensible-table.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  styles: [
    `
      :host ::ng-deep .ngx-datatable.material .datatable-body .datatable-row-detail {
        background: none;
        padding: 0;
      }
    `,
  ],
})
export class ExtensibleTableComponent<R = any> implements AfterViewInit, OnDestroy {
  readonly #injector = inject(Injector);
  readonly getInjected = this.#injector.get.bind(this.#injector);
  protected readonly cdr = inject(ChangeDetectorRef);
  protected readonly locale = inject(LOCALE_ID);
  protected readonly config = inject(ConfigStateService);
  protected readonly timeZoneService = inject(TimezoneService);
  protected readonly entityPropTypeClasses = inject(ENTITY_PROP_TYPE_CLASSES);
  protected readonly permissionService = inject(PermissionService);
  private platformId = inject(PLATFORM_ID);
  protected isBrowser = isPlatformBrowser(this.platformId);

  // Input signals
  readonly actionsTextInput = input<string | undefined>(undefined, { alias: 'actionsText' });
  readonly dataInput = input<R[]>([], { alias: 'data' });
  readonly list = input.required<ListService>();
  readonly recordsTotal = input.required<number>();
  readonly actionsColumnWidthInput = input<number | undefined>(undefined, {
    alias: 'actionsColumnWidth',
  });
  readonly actionsTemplate = input<TemplateRef<any> | undefined>(undefined);
  readonly selectable = input(false);
  readonly selectionTypeInput = input<SelectionType | keyof typeof SelectionType>(
    SelectionType.multiClick,
    {
      alias: 'selectionType',
    },
  );
  readonly selected = input<any[]>([]);
  readonly infiniteScroll = input(false);
  readonly isLoading = input(false);
  readonly scrollThreshold = input(10);
  readonly tableHeight = input<number | undefined>(undefined);
  readonly rowDetailTemplate = input<TemplateRef<RowDetailContext<R>> | undefined>(undefined);
  readonly rowDetailHeight = input<string | number>('100%');

  // Output signals
  readonly tableActivate = output<any>();
  readonly selectionChange = output<any[]>();
  readonly loadMore = output<void>();
  readonly rowDetailToggle = output<R>();

  // Internal signals
  protected readonly _data = signal<R[]>([]);
  private readonly _actionsColumnWidth = signal<number | undefined>(DEFAULT_ACTIONS_COLUMN_WIDTH);

  readonly rowDetailComponent = contentChild(ExtensibleTableRowDetailComponent);

  readonly table = viewChild.required<DatatableComponent>('table');

  // Computed values
  protected readonly actionsText = computed(() => {
    return this.actionsTextInput() ?? (this.actionList.length >= 1 ? 'AbpUi::Actions' : '');
  });

  protected readonly selectionType = computed(() => {
    const value = this.selectionTypeInput();
    return typeof value === 'string' ? SelectionType[value as keyof typeof SelectionType] : value;
  });

  protected get data(): R[] {
    return this._data();
  }

  protected set data(value: R[]) {
    this._data.set(value);
  }

  protected get effectiveRowDetailTemplate(): TemplateRef<RowDetailContext<R>> | undefined {
    return this.rowDetailComponent()?.template() ?? this.rowDetailTemplate();
  }

  protected get effectiveRowDetailHeight(): string | number {
    return this.rowDetailComponent()?.rowHeight() ?? this.rowDetailHeight();
  }

  hasAtLeastOnePermittedAction: boolean;

  readonly propList: EntityPropList<R>;

  readonly actionList: EntityActionList<R>;

  readonly trackByFn: TrackByFunction<EntityProp<R>> = (_, item) => item.name;

  // Infinite scroll: debounced load more subject
  private readonly loadMoreSubject = new Subject<void>();
  private readonly loadMoreSubscription = this.loadMoreSubject
    .pipe(debounceTime(100), distinctUntilChanged())
    .subscribe(() => this.triggerLoadMore());

  readonly columnWidths = computed(() => {
    return this.propList.toArray().map(prop => prop.columnWidth);
  });

  constructor() {
    const extensions = this.#injector.get(ExtensionsService);
    const name = this.#injector.get(EXTENSIONS_IDENTIFIER);
    this.propList = extensions.entityProps.get(name).props;
    this.actionList = extensions['entityActions'].get(name)
      .actions as unknown as EntityActionList<R>;

    this.hasAtLeastOnePermittedAction =
      this.permissionService.filterItemsByPolicy(
        this.actionList.toArray().map(action => ({ requiredPolicy: action.permission })),
      ).length > 0;

    // Watch actionsColumnWidth input
    effect(() => {
      const width = this.actionsColumnWidthInput();
      this._actionsColumnWidth.set(width ? Number(width) : undefined);
    });

    // Watch data input changes
    effect(() => {
      const dataValue = this.dataInput();
      if (!dataValue) return;

      if (dataValue.length < 1) {
        this.list().totalCount = this.recordsTotal();
      }

      this._data.set(dataValue.map((record, index) => this.prepareRecord(record, index)));
    });
  }

  private prepareRecord(record: any, index: number): any {
    this.propList.forEach(prop => {
      const propData = { getInjected: this.getInjected, record, index } as ReadonlyPropData;
      const value = this.getContent(prop.value, propData);

      const propKey = `_${prop.value.name}`;
      record[propKey] = {
        visible: prop.value.visible(propData),
        value,
      };
      if (prop.value.component) {
        record[propKey].injector = Injector.create({
          providers: [
            {
              provide: PROP_DATA_STREAM,
              useValue: value,
            },
            {
              provide: ROW_RECORD,
              useValue: record,
            },
          ],
          parent: this.#injector,
        });
        record[propKey].component = prop.value.component;
      }
    });

    return record;
  }

  private getIcon(value: boolean) {
    return value
      ? '<div class="text-success"><i class="fa fa-check" aria-hidden="true"></i></div>'
      : '<div class="text-danger"><i class="fa fa-times" aria-hidden="true"></i></div>';
  }

  private getEnum(rowValue: any, list: Array<ABP.Option<any>>) {
    if (!list || list.length < 1) return rowValue;
    const { key } = list.find(({ value }) => value === rowValue) || {};
    return key;
  }

  getContent(prop: EntityProp<R>, data: ReadonlyPropData): Observable<string> {
    return prop.valueResolver(data).pipe(
      map(value => {
        switch (prop.type) {
          case ePropType.Boolean:
            return this.getIcon(value);
          case ePropType.Enum:
            return this.getEnum(value, prop.enumList || []);
          default:
            return value;
          // More types can be handled in the future
        }
      }),
    );
  }

  isVisibleActions(rowData: any): boolean {
    const actions = this.actionList.toArray();
    const visibleActions = actions.filter(action => {
      const { visible, permission } = action;

      let isVisible = true;
      let hasPermission = true;

      if (visible) {
        isVisible = visible({ record: rowData, getInjected: this.getInjected });
      }

      if (permission) {
        hasPermission = this.permissionService.getGrantedPolicy(permission);
      }

      return isVisible && hasPermission;
    });

    return visibleActions.length > 0;
  }

  onSelect({ selected }: { selected: any[] }) {
    const selectedValue = this.selected();
    selectedValue.splice(0, selectedValue.length);
    selectedValue.push(...selected);
    this.selectionChange.emit(selected);
  }

  onScroll(scrollEvent: Event): void {
    if (!this.shouldHandleScroll()) {
      return;
    }

    const target = scrollEvent.target as HTMLElement;
    if (!target) {
      return;
    }

    if (this.isNearScrollBottom(target)) {
      this.loadMoreSubject.next();
    }
  }

  private shouldHandleScroll(): boolean {
    return this.infiniteScroll() && !this.isLoading();
  }

  private isNearScrollBottom(element: HTMLElement): boolean {
    const { offsetHeight, scrollTop, scrollHeight } = element;
    return offsetHeight + scrollTop >= scrollHeight - this.scrollThreshold();
  }

  private triggerLoadMore(): void {
    this.loadMore.emit();
  }

  getTableHeight() {
    if (!this.infiniteScroll()) return 'auto';

    const tableHeight = this.tableHeight();
    return tableHeight ? `${tableHeight}px` : 'auto';
  }

  toggleExpandRow(row: R): void {
    const table = this.table();
    if (table && table.rowDetail) {
      table.rowDetail.toggleExpandRow(row);
    }
    this.rowDetailToggle.emit(row);
  }

  ngAfterViewInit(): void {
    if (!this.infiniteScroll()) {
      this.list()
        ?.requestStatus$?.pipe(filter(status => status === 'loading'))
        .subscribe(() => {
          this._data.set([]);
          this.cdr.markForCheck();
        });
    }
  }

  ngOnDestroy(): void {
    this.loadMoreSubscription.unsubscribe();
  }
}
