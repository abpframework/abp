import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  inject,
  Injector,
  Input,
  LOCALE_ID,
  OnChanges,
  Output,
  SimpleChanges,
  TemplateRef,
  TrackByFunction,
} from '@angular/core';
import { AsyncPipe, formatDate, NgComponentOutlet, NgTemplateOutlet } from '@angular/common';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';

import {
  ABP,
  ConfigStateService,
  getShortDateFormat,
  getShortDateShortTimeFormat,
  getShortTimeFormat,
  ListService,
  LocalizationModule,
  PermissionDirective,
  PermissionService,
} from '@abp/ng.core';
import {
  AbpVisibleDirective,
  NgxDatatableDefaultDirective,
  NgxDatatableListDirective,
} from '@abp/ng.theme.shared';

import { ePropType } from '../../enums/props.enum';
import { EntityActionList } from '../../models/entity-actions';
import { EntityProp, EntityPropList } from '../../models/entity-props';
import { PropData } from '../../models/props';
import { ExtensionsService } from '../../services/extensions.service';
import {
  ENTITY_PROP_TYPE_CLASSES,
  EXTENSIONS_IDENTIFIER,
  PROP_DATA_STREAM,
} from '../../tokens/extensions.token';
import { GridActionsComponent } from '../grid-actions/grid-actions.component';

const DEFAULT_ACTIONS_COLUMN_WIDTH = 150;

@Component({
  exportAs: 'abpExtensibleTable',
  selector: 'abp-extensible-table',
  standalone: true,
  imports: [
    AbpVisibleDirective,
    NgxDatatableModule,
    GridActionsComponent,
    NgbTooltip,
    NgxDatatableDefaultDirective,
    NgxDatatableListDirective,
    PermissionDirective,
    LocalizationModule,
    AsyncPipe,
    NgTemplateOutlet,
    NgComponentOutlet,
  ],
  templateUrl: './extensible-table.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ExtensibleTableComponent<R = any> implements OnChanges {
  protected _actionsText!: string;
  @Input()
  set actionsText(value: string) {
    this._actionsText = value;
  }

  get actionsText(): string {
    return this._actionsText ?? (this.actionList.length > 1 ? 'AbpUi::Actions' : '');
  }

  @Input() data!: R[];
  @Input() list!: ListService;
  @Input() recordsTotal!: number;

  @Input() set actionsColumnWidth(width: number) {
    this.setColumnWidths(width ? Number(width) : undefined);
  }

  @Input() actionsTemplate?: TemplateRef<any>;

  @Output() tableActivate = new EventEmitter();

  hasAtLeastOnePermittedAction: boolean;

  readonly columnWidths!: number[];

  readonly propList: EntityPropList<R>;

  readonly actionList: EntityActionList<R>;

  readonly trackByFn: TrackByFunction<EntityProp<R>> = (_, item) => item.name;

  locale = inject(LOCALE_ID);
  private config = inject(ConfigStateService);
  entityPropTypeClasses = inject(ENTITY_PROP_TYPE_CLASSES);
  #injector = inject(Injector);
  getInjected = this.#injector.get.bind(this.#injector);
  permissionService = this.#injector.get(PermissionService);

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
    this.setColumnWidths(DEFAULT_ACTIONS_COLUMN_WIDTH);
  }

  private setColumnWidths(actionsColumn: number | undefined) {
    const widths = [actionsColumn];
    this.propList.forEach(({ value: prop }) => {
      widths.push(prop.columnWidth);
    });
    (this.columnWidths as any) = widths;
  }

  private getDate(value: Date | undefined, format: string | undefined) {
    return value && format ? formatDate(value, format, this.locale) : '';
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

  getContent(prop: EntityProp<R>, data: PropData): Observable<string> {
    return prop.valueResolver(data).pipe(
      map(value => {
        switch (prop.type) {
          case ePropType.Boolean:
            return this.getIcon(value);
          case ePropType.Date:
            return this.getDate(value, getShortDateFormat(this.config));
          case ePropType.Time:
            return this.getDate(value, getShortTimeFormat(this.config));
          case ePropType.DateTime:
            return this.getDate(value, getShortDateShortTimeFormat(this.config));
          case ePropType.Enum:
            return this.getEnum(value, prop.enumList || []);
          default:
            return value;
          // More types can be handled in the future
        }
      }),
    );
  }

  ngOnChanges({ data }: SimpleChanges) {
    if (!data?.currentValue) return;

    if (data.currentValue.length < 1) {
      this.list.totalCount = this.recordsTotal;
    }

    this.data = data.currentValue.map((record: any, index: number) => {
      this.propList.forEach(prop => {
        const propData = { getInjected: this.getInjected, record, index } as any;
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
            ],
            parent: this.#injector,
          });
          record[propKey].component = prop.value.component;
        }
      });

      return record;
    });
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
}
