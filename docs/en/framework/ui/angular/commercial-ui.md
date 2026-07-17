```json
//[doc-seo]
{
    "Description": "Learn how to use ABP Commercial Angular date range controls, standalone UI configuration and the public testing entrypoint."
}
```

# Commercial UI Components

The `@volo/abp.commercial.ng.ui` package provides shared ABP Commercial Angular controls in addition to the separately documented [lookup components](./lookup-components.md) and [entity filters](./entity-filters.md). The package is included in ABP Commercial Angular application templates.

## Date Range Controls

`DateRangePickerComponent` and `DatetimeRangePickerComponent` are Angular form controls. `startDateProp` and `endDateProp` specify the two properties updated in the bound model:

```ts
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  DateRangePickerModule,
  DatetimeRangePickerComponent,
} from '@volo/abp.commercial.ng.ui';

@Component({
  selector: 'app-report-range',
  templateUrl: './report-range.component.html',
  imports: [
    FormsModule,
    DateRangePickerModule,
    DatetimeRangePickerComponent,
  ],
})
export class ReportRangeComponent {
  dateRange: {
    startDate: string | Date | null;
    endDate: string | Date | null;
  } = {
    startDate: null,
    endDate: null,
  };
}
```

```html
<abp-date-range-picker
  [(ngModel)]="dateRange"
  startDateProp="startDate"
  endDateProp="endDate"
  labelText="Reports::DateRange"
/>
```

Use `abp-datetime-range-picker` with the same inputs when the model also needs start and end times.

## Standalone Configuration

Register commercial UI configuration in the application providers. The following example enables flag icons:

```ts
import { ApplicationConfig } from '@angular/core';
import {
  provideCommercialUiConfig,
  withEnableFlagIcon,
} from '@volo/abp.commercial.ng.ui/config';

export const appConfig: ApplicationConfig = {
  providers: [
    provideCommercialUiConfig(
      withEnableFlagIcon(true),
    ),
  ],
};
```

Calling `provideCommercialUiConfig()` also registers the shared profile-picture, impersonation and tenant-switching providers. Flag icons are disabled unless `withEnableFlagIcon(true)` is supplied.

## Testing

`CommercialUiTestingModule` imports and exports `BaseCommercialUiModule`, making its declarations available from the public testing entrypoint. Its `withConfig()` method returns the module registration without adding test doubles or providers:

```ts
import { TestBed } from '@angular/core/testing';
import { CommercialUiTestingModule } from '@volo/abp.commercial.ng.ui/testing';

await TestBed.configureTestingModule({
  imports: [CommercialUiTestingModule.withConfig()],
}).compileComponents();
```
