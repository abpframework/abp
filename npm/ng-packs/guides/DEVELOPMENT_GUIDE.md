# ABP Npm Package Development Guide

## Table of Contents

1. [Overview](#overview)
2. [Project Structure](#project-structure)
3. [Module Architecture](#module-architecture)
4. [Component Development](#component-development)
5. [Service Layer](#service-layer)
6. [Routing & Navigation](#routing--navigation)
7. [State Management](#state-management)
8. [Form Handling](#form-handling)
9. [Validation](#validation)
10. [Testing](#testing)
11. [Best Practices](#best-practices)
12. [Common Patterns](#common-patterns)
13. [Troubleshooting](#troubleshooting)

## Overview

This guide provides comprehensive instructions for developing ABP modules following the established patterns and conventions. The guide is based on the SaaS module structure and can be applied to any ABP module development.

### Key Principles

- **Modularity**: Each module should be self-contained with clear boundaries
- **Extensibility**: Modules should support customization through extension points
- **Consistency**: Follow established ABP patterns and conventions
- **Testability**: All components should be easily testable
- **Performance**: Optimize for bundle size and runtime performance

## Project Structure

```
package-name/
├── package.json              # Package metadata and dependencies
├── ng-package.json           # ng-packagr configuration
├── project.json              # Nx workspace configuration
├── tsconfig.json             # TypeScript root config
├── tsconfig.lib.json         # Library-specific TS config
├── tsconfig.lib.prod.json    # Production build config
├── tsconfig.spec.json        # Test configuration
├── jest.config.ts            # Jest test configuration
├── tslint.json              # (optional) Linting rules
├── README.md                 # Package documentation
│
├── src/                      # Main library source code
│   ├── lib/                  # Core library implementation
│   │   ├── components/       # Angular components
│   │   ├── services/         # Business logic services
│   │   ├── models/           # TypeScript interfaces/models
│   │   ├── enums/            # Enumerations
│   │   ├── guards/           # Route guards
│   │   ├── resolvers/        # Route resolvers
│   │   ├── defaults/         # Default configurations
│   │   ├── tokens/           # Dependency injection tokens
│   │   ├── utils/            # Utility functions
│   │   ├── validators/       # Form validators
│   │   ├── [feature].routes.ts
│   └── public-api.ts         # Public exports barrel file
│
├── config/                   # Configuration sub-package (optional)
│   ├── ng-package.json
│   └── src/
│       ├── components/       # Config-specific components
│       ├── providers/         # Route/setting providers
│       ├── services/         # Config services
│       ├── models/           # Config models
│       ├── enums/            # Config enums
│       └── public-api.ts
│
├── proxy/                    # API proxy sub-package (optional)
│   ├── ng-package.json
│   └── src/
│       ├── lib/
│       │   └── proxy/
│       │       ├── [feature]/ # Generated proxy services
│       │       ├── generate-proxy.json
│       │       └── README.md
│       └── public-api.ts
│
├── common/                   # Common/shared sub-package (optional)
│   ├── ng-package.json
│   └── src/
│       ├── enums/
│       ├── tokens/
│       └── public-api.ts
│
└── admin/                    # Admin-specific sub-package (optional)
    ├── ng-package.json
    └── src/
        └── ...
```

### File Naming Conventions

- Use kebab-case for file names: `my-component.component.ts`
- Use PascalCase for class names: `MyComponent`
- Use camelCase for variables and methods: `myVariable`, `myMethod()`
- Use UPPER_SNAKE_CASE for constants: `MY_CONSTANT`

## Package Architecture

### Configuration Options Pattern

```typescript
export interface MyConfigOptions {
  entityActionContributors?: MyEntityActionContributors;
  toolbarActionContributors?: MyToolbarActionContributors;
  entityPropContributors?: MyEntityPropContributors;
  createFormPropContributors?: MyCreateFormPropContributors;
  editFormPropContributors?: MyEditFormPropContributors;
}
```

## Component Development

### Component Structure

```typescript
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ListService } from '@abp/ng.core';
import { MyService } from '../services';

@Component({
  selector: 'app-my-component',
  templateUrl: './my-component.component.html',
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eMyComponents.MyComponent,
    },
  ],
  imports: [],
})
export class MyComponent implements OnInit, OnDestroy {
  // Properties
  data = this.list.getGrid();
  isModalVisible = false;

  public readonly list = inject(ListService);
  private myService = inject(MyService);

  ngOnInit() {
    this.hookToQuery();
  }

  ngOnDestroy() {
    this.list.hookToQuery = () => {};
  }

  // Methods
  onEdit(id: string) {
    // Implementation
  }

  onDelete(id: string) {
    // Implementation
  }

  private hookToQuery() {
    this.list
      .hookToQuery(query => this.myService.getList({ ...query, ...this.filters }))
      .subscribe(res => (this.data = res));
  }
}
```

Example HTML template

```html
<abp-page [title]="'AbpLocalizationKey::SubKey' | abpLocalization" [toolbar]="data.items">
  <div>
    <div class="mt-2 mt-sm-0">
      <abp-advanced-entity-filters [list]="list" localizationSourceName="AbpLocalizationKey">
        <abp-advanced-entity-filters-form>
          <form #filterForm (keyup.enter)="list.get()">
            <!-- ... -->
          </form>
        </abp-advanced-entity-filters-form>
      </abp-advanced-entity-filters>
    </div>
    <div class="card">
      <abp-extensible-table [data]="data.items" [recordsTotal]="data.totalCount" [list]="list" />
    </div>
  </div>
</abp-page>

<abp-modal [(visible)]="isModalVisible" [busy]="modalBusy" (disappear)="form = null">
  <ng-template #abpHeader>
    <h3>
      @if (selected?.id) { {{ 'AbpLocalizationKey::Edit' | abpLocalization }} @if
      (selected.userName) { - {{ selected.userName }} } } @else { {{ 'AbpLocalizationKey::New' |
      abpLocalization }} }
    </h3>
  </ng-template>

  <ng-template #abpBody>
    @if (form) {
    <form [formGroup]="form" id="myForm" (ngSubmit)="save()" validateOnSubmit>
      <a ngbNavLink>{{ 'AbpLocalizationKey::MyInfo' | abpLocalization }}</a>
      <ng-template ngbNavContent>
        <div class="row">
          <abp-extensible-form class="row gap-x2" [selectedRecord]="selected" />
        </div>
      </ng-template>
    </form>
    }
  </ng-template>

  <ng-template #abpFooter>
    <button type="button" class="btn btn-outline-primary" abpClose>
      {{ 'AbpUi::Cancel' | abpLocalization }}
    </button>
    <abp-button iconClass="fa fa-check" buttonType="submit" formName="myForm">
      {{ 'AbpUi::Save' | abpLocalization }}
    </abp-button>
  </ng-template>
</abp-modal>
```

### Component Best Practices

1. **Single Responsibility**: Each component should have one clear purpose
2. **Dependency Injection**: Use constructor injection for services
3. **Lifecycle Management**: Implement `OnInit` and `OnDestroy` when needed
4. **State Management**: Use reactive forms and observables
5. **Error Handling**: Implement proper error boundaries
6. **Accessibility**: Follow ARIA guidelines
7. **Performance**: Use `OnPush` change detection when possible

## Service Layer

### Service Structure

```typescript
import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import { MyDto } from '../models';

@Injectable({
  providedIn: 'root',
})
export class MyService extends RestService {
  protected get url() {
    return 'api/my-endpoint';
  }

  getList(query: any) {
    return this.request<MyDto[]>({
      method: 'GET',
      params: query,
    });
  }

  getById(id: string) {
    return this.request<MyDto>({
      method: 'GET',
      url: `${this.url}/${id}`,
    });
  }

  create(input: Partial<MyDto>) {
    return this.request<MyDto>({
      method: 'POST',
      body: input,
    });
  }

  update(id: string, input: Partial<MyDto>) {
    return this.request<MyDto>({
      method: 'PUT',
      url: `${this.url}/${id}`,
      body: input,
    });
  }

  delete(id: string) {
    return this.request<void>({
      method: 'DELETE',
      url: `${this.url}/${id}`,
    });
  }
}
```

### Service Best Practices

1. **Extend RestService**: Use ABP's base service for HTTP operations
2. **Type Safety**: Use TypeScript interfaces for all data structures
3. **Error Handling**: Implement proper error handling and logging
4. **Caching**: Consider caching strategies for frequently accessed data
5. **Observables**: Use RxJS observables for reactive programming

## Routing & Navigation

### Route Configuration

```typescript
import { Routes } from '@angular/router';
import { Provider } from '@angular/core';
import {
  RouterOutletComponent,
  authGuard,
  permissionGuard,
  ReplaceableRouteContainerComponent,
  ReplaceableComponents,
} from '@abp/ng.core';

export function createRoutes(config: MyConfigOptions = {}): Routes {
  return [
    { path: '', redirectTo: 'my-feature', pathMatch: 'full' },
    {
      path: '',
      component: RouterOutletComponent,
      providers: provideMyContributors(config),
      canActivate: [authGuard, permissionGuard],
      children: [
        {
          path: 'my-feature',
          component: ReplaceableRouteContainerComponent,
          data: {
            requiredPolicy: 'My.Feature',
            replaceableComponent: {
              key: eMyComponents.MyFeature,
              defaultComponent: MyFeatureComponent,
            } as ReplaceableComponents.RouteData<MyFeatureComponent>,
          },
          title: 'My::Feature',
        },
      ],
    },
  ];
}

function provideMyContributors(options: MyConfigOptions = {}): Provider[] {
  return [
    // ... providers
  ];
}
```

### Route Best Practices

1. **Lazy Loading**: Use lazy loading for better performance
2. **Guards**: Implement authentication and authorization guards
3. **Permissions**: Use permission-based route protection
4. **Replaceable Components**: Support component replacement for extensibility
5. **SEO**: Use meaningful route titles and metadata

## State Management

### State Management Patterns

```typescript
// Using ABP's ConfigStateService
import { ConfigStateService } from '@abp/ng.core';

export class MyComponent {
  private configState = inject(ConfigStateService);

  getSettings() {
    return this.configState.getSetting('My.Setting');
  }
}

// Using reactive forms
import { FormBuilder, FormGroup } from '@angular/forms';

export class MyComponent {
  form: FormGroup;
  private fb = inject(FormBuilder);

  constructor() {
    this.form = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
    });
  }
}
```

## Form Handling

### Form Structure

```typescript
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MyService } from '../services';

@Component({
  selector: 'app-my-form',
  template: `
    <form [formGroup]="form" (ngSubmit)="onSubmit()">
      <input formControlName="name" />
      <input formControlName="email" />
      <button type="submit">Submit</button>
    </form>
  `,
  imports: [],
})
export class MyFormComponent {
  form: FormGroup;

  private fb = inject(FormBuilder);
  private myService = inject(MyService);

  constructor() {
    this.form = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
    });
  }

  onSubmit() {
    if (this.form.valid) {
      this.myService.create(this.form.value).subscribe(
        result => {
          // Handle success
        },
        error => {
          // Handle error
        },
      );
    }
  }
}
```

## Validation

### Custom Validators

```typescript
import { Provider } from '@angular/core';
import { AsyncValidatorFn, FormGroup } from '@angular/forms';
import { of } from 'rxjs';

export const MY_VALIDATOR_PROVIDER: Provider = {
  provide: MY_FORM_ASYNC_VALIDATORS_TOKEN,
  multi: true,
  useFactory: myCustomValidator,
};

export function myCustomValidator(): AsyncValidatorFn {
  return (group: FormGroup) => {
    // Validation logic
    const field1 = group?.get('field1');
    const field2 = group?.get('field2');

    if (!field1 || !field2) {
      return of(null);
    }

    if (field1.value && !field2.value) {
      field2.setErrors({ required: true });
    }

    return of(null);
  };
}
```

### Validation Best Practices

1. **Async Validators**: Use for server-side validation
2. **Cross-field Validation**: Validate relationships between fields
3. **Error Messages**: Provide clear, user-friendly error messages
4. **Performance**: Debounce async validators to avoid excessive API calls
5. **Accessibility**: Ensure validation errors are announced to screen readers

## Testing

### Unit Testing Structure

```typescript
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MyComponent } from './my.component';
import { MyService } from '../services';

describe('MyComponent', () => {
  let component: MyComponent;
  let fixture: ComponentFixture<MyComponent>;
  let myService: jasmine.SpyObj<MyService>;

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('MyService', ['getList']);

    await TestBed.configureTestingModule({
      declarations: [MyComponent],
      providers: [{ provide: MyService, useValue: spy }],
    }).compileComponents();

    fixture = TestBed.createComponent(MyComponent);
    component = fixture.componentInstance;
    myService = TestBed.inject(MyService) as jasmine.SpyObj<MyService>;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load data on init', () => {
    // Test implementation
  });
});
```

### Testing Best Practices

1. **Isolation**: Test components in isolation
2. **Mocking**: Mock external dependencies
3. **Coverage**: Aim for high test coverage
4. **Integration Tests**: Test component interactions
5. **E2E Tests**: Test complete user workflows

## Best Practices

### Code Organization

1. **Feature-based Structure**: Organize code by features, not types
2. **Barrel Exports**: Use index files for clean imports
3. **Consistent Naming**: Follow established naming conventions
4. **Documentation**: Document complex logic and public APIs
5. **Type Safety**: Use TypeScript strictly

### Performance

1. **Lazy Loading**: Load modules on demand
2. **Change Detection**: Use OnPush strategy when possible
3. **Memory Management**: Unsubscribe from observables
4. **Bundle Size**: Minimize bundle size through tree shaking
5. **Caching**: Implement appropriate caching strategies

### Security

1. **Input Validation**: Validate all user inputs
2. **XSS Prevention**: Sanitize user-generated content
3. **CSRF Protection**: Use CSRF tokens for state-changing operations
4. **Authorization**: Check permissions at component and service levels
5. **HTTPS**: Use HTTPS in production

## Common Patterns

### Extension Pattern

```typescript
// Define extension tokens
export const MY_ENTITY_ACTION_CONTRIBUTORS = new InjectionToken<EntityActionContributors>(
  'MY_ENTITY_ACTION_CONTRIBUTORS'
);

// Provide default implementations
export const DEFAULT_MY_ENTITY_ACTIONS = {
  [eMyComponents.MyFeature]: DEFAULT_MY_FEATURE_ACTIONS,
};

// Use in module
{
  provide: MY_ENTITY_ACTION_CONTRIBUTORS,
  useValue: options.entityActionContributors || DEFAULT_MY_ENTITY_ACTIONS,
}
```

### Modal Pattern

```typescript
@Injectable({
  providedIn: 'root',
})
export class MyModalService {
  private modalRef: NgbModalRef;

  private modalService = inject(NgbModal);

  show(data?: any): NgbModalRef {
    this.modalRef = this.modalService.open(MyModalComponent, {
      size: 'lg',
      backdrop: 'static',
    });

    if (data) {
      this.modalRef.componentInstance.data = data;
    }

    return this.modalRef;
  }

  close() {
    if (this.modalRef) {
      this.modalRef.close();
    }
  }
}
```

### List Pattern

```typescript
export class MyListComponent {
  data = this.list.getGrid();

  readonly list = inject(ListService);

  ngOnInit() {
    this.hookToQuery();
  }

  private hookToQuery() {
    this.list
      .hookToQuery(query => this.pageService.getList({ ...query, ...this.filters }))
      .subscribe(res => (this.data = res));
  }
}
```

## Troubleshooting

### Common Issues

1. **Module Not Found**: Check import paths and module declarations
2. **Circular Dependencies**: Use forwardRef() or restructure imports
3. **Memory Leaks**: Ensure proper cleanup in ngOnDestroy
4. **Performance Issues**: Use OnPush change detection and memoization
5. **Type Errors**: Ensure proper TypeScript configuration

### Debugging Tips

1. **Angular DevTools**: Use Angular DevTools for component inspection
2. **Console Logging**: Use console.log strategically for debugging
3. **Network Tab**: Monitor API calls in browser dev tools
4. **Error Boundaries**: Implement error boundaries for graceful error handling
5. **Unit Tests**: Use tests to reproduce and fix bugs

### Performance Optimization

1. **Bundle Analysis**: Use webpack-bundle-analyzer to identify large dependencies
2. **Lazy Loading**: Implement route-based code splitting
3. **Tree Shaking**: Ensure unused code is eliminated
4. **Caching**: Implement appropriate caching strategies
5. **Minification**: Ensure proper minification in production builds

---

## Conclusion

This guide provides a comprehensive overview of ABP module development patterns and best practices. Follow these guidelines to create maintainable, extensible, and performant modules that integrate seamlessly with the ABP framework.

Remember to:

- Follow established ABP conventions
- Write comprehensive tests
- Document your code
- Consider performance implications
- Implement proper error handling
- Use TypeScript features effectively

For more information, refer to the official ABP documentation and community resources.
