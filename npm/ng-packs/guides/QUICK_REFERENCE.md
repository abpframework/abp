# ABP Package Development - Quick Reference

## Essential Commands

### Build & Test

```bash
# Open a terminal on ng-packs directory
cd /Users/sumeyyekurtulus/Desktop/volosoft/GITHUB/abp/npm/ng-packs

# Build the package
yarn nx build package-name --skip-nx-cache

# Run tests
yarn nx test package-name --test-file test-file.spec.ts

# Lint code
yarn run lint

# Build for production
yarn nx build package-name --configuration=production
```

### Development

```bash
# Open a terminal on ng-packs directory
cd /Users/sumeyyekurtulus/Desktop/volosoft/GITHUB/abp/npm/ng-packs

# Start development server
yarn start

# Watch for changes
yarn run watch

# Generate component
ng generate component my-component

# Generate service
ng generate service my-service
```

## File Structure Quick Reference

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
├── proxy/                    # API proxy sub-package (optional) [Do not touch while making generation]
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

## Common Patterns

### Component Definition

```typescript
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
export class MyComponent implements OnInit {
  public readonly list = inject(ListService);
  private myComponentService = inject(MyComponentService);

  data = this.list.getGrid();

  ngOnInit() {
    this.hookToQuery();
  }

  private hookToQuery() {
    this.list
      .hookToQuery(query => this.myComponentService.getList({ ...query, ...this.filters }))
      .subscribe(res => (this.data = res));
  }
}
```

### Default Extension Points

```typescript
export const DEFAULT_MY_ENTITY_ACTIONS = EntityAction.createMany<MyElementDto>([
  {
    text: 'AbpPackage::MyElement',
    action: data => {
      const { piece } = data.record;
      if (!piece) {
        return;
      }

      const router = data.getInjected(Router);
      router.navigate(['/package/piece']);
    },
    permission: 'AbpPackage.MyElement',
  },
]);
```

```typescript
export const DEFAULT_MY_ENTITY_PROPS = EntityProp.createMany<MyElementDto>([
  {
    type: ePropType.PropType,
    name: 'propName',
    displayName: 'AbpPackage::PropDisplayName',
    sortable: true,
    columnWidth: 200,
  },
]);
```

```typescript
export const DEFAULT_MY_CREATE_FORM_PROPS = FormProp.createMany<MyElementDto>([
  {
    type: ePropType.PropType,
    name: 'propName',
    displayName: 'AbpPackage::PropDisplayName',
    id: 'propID',
  },
]);
export const DEFAULT_MY_EDIT_FORM_PROPS = DEFAULT_MY_CREATE_FORM_PROPS.filter(
  prop => prop.name !== 'propName',
);
```

```typescript
export const DEFAULT_MY_TOOLBAR_COMPONENTS = ToolbarComponent.createMany<MyElementDto[]>([
  {
    permission: 'AbpPermissionKey',
    component: MyComponent,
  },
]);

export const DEFAULT_MY_TOOLBAR_ACTIONS = ToolbarAction.createMany<MyElementDto[]>([
  {
    text: 'AbpPackage::Text',
    action: data => {
      const component = data.getInjected(MyComponent);
      component.onAdd();
    },
    permission: 'AbpPermissionKey',
    icon: 'fa fa-plus',
  },
]);

export const DEFAULT_USERS_TOOLBAR_ALL = [
  ...DEFAULT_USERS_TOOLBAR_COMPONENTS,
  ...DEFAULT_USERS_TOOLBAR_ACTIONS,
];
```

### Route Definition

```typescript
export function createRoutes(config: MyConfigOptions = {}): Routes {
  return [
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
            },
          },
        },
      ],
    },
  ];
}

function provideMyContributors(options: MyConfigOptions = {}): Provider[] {
  return [
    {
      provide: MY_ENTITY_ACTION_CONTRIBUTORS,
      useValue: options.entityActionContributors,
    },
    {
      provide: MY_TOOLBAR_ACTION_CONTRIBUTORS,
      useValue: options.toolbarActionContributors,
    },
    {
      provide: MY_ENTITY_PROP_CONTRIBUTORS,
      useValue: options.entityPropContributors,
    },
    {
      provide: MY_CREATE_FORM_PROP_CONTRIBUTORS,
      useValue: options.createFormPropContributors,
    },
    {
      provide: MY_EDIT_FORM_PROP_CONTRIBUTORS,
      useValue: options.editFormPropContributors,
    },
  ];
}
```

```typescript
export const APP_ROUTES: Routes = [
  {
    path: 'my-route',
    loadChildren: () => import('my-package').then(c => c.createRoutes()),
  },
];
```

### Provider Definitions

```typescript
export const MY_ROUTE_PROVIDERS = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

export function configureRoutes() {
  const routes = inject(RoutesService);
  routes.add([
    {
      path: '/my-route',
      name: eMyRouteNames.Route,
      parentName: eThemeSharedRouteNames.Route,
      order: 2,
      layout: eLayoutType.application,
      iconClass: 'fa fa-id-card-o',
      requiredPolicy: eMyPolicyNames.Route,
    },
    {
      path: '/my-route/my-sub-route',
      name: eMyRouteNames.MySubRoute,
      parentName: eMyRouteNames.Route,
      order: 1,
      requiredPolicy: eMyPolicyNames.MySubRoute,
    },
  ]);
}
```

```typescript
export function provideMyConfig() {
  return makeEnvironmentProviders([MY_ROUTE_PROVIDERS]);
}
```

```typescript
export const appConfig: ApplicationConfig = {
  providers: [provideMyConfig()],
};
```

## Validation Patterns

### Custom Validator

```typescript
export const MY_VALIDATOR_PROVIDER: Provider = {
  provide: MY_FORM_ASYNC_VALIDATORS_TOKEN,
  multi: true,
  useFactory: myCustomValidator,
};

export function myCustomValidator(): AsyncValidatorFn {
  return (group: FormGroup) => {
    // Validation logic
    return of(null);
  };
}
```

## Testing Patterns

### Component Test

```typescript
describe('MyComponent', () => {
  let component: MyComponent;
  let fixture: ComponentFixture<MyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MyComponent],
      providers: [{ provide: MyService, useValue: jasmine.createSpyObj('MyService', ['getList']) }],
    }).compileComponents();

    fixture = TestBed.createComponent(MyComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
```

## Configuration Options

### Module Configuration

```typescript
export interface MyConfigOptions {
  entityActionContributors?: MyEntityActionContributors;
  toolbarActionContributors?: MyToolbarActionContributors;
  entityPropContributors?: MyEntityPropContributors;
  createFormPropContributors?: MyCreateFormPropContributors;
  editFormPropContributors?: MyEditFormPropContributors;
}
```

## Common Imports

### Common Services

```typescript
import { ListService, ConfigStateService } from '@abp/ng.core';
import { RestService } from '@abp/ng.core';
import { EntityAction, FormProp } from '@abp/ng.components/extensible';
```

### Common Guards & Components

```typescript
import { authGuard, permissionGuard } from '@abp/ng.core';
import { RouterOutletComponent, ReplaceableRouteContainerComponent } from '@abp/ng.core';
```

## Naming Conventions

| Type              | Convention                 | Example                     |
| ----------------- | -------------------------- | --------------------------- |
| Files             | kebab-case                 | `my-component.component.ts` |
| Classes           | PascalCase                 | `MyComponent`               |
| Variables/Methods | camelCase                  | `myVariable`, `myMethod()`  |
| Constants         | UPPER_SNAKE_CASE           | `MY_CONSTANT`               |
| Interfaces        | PascalCase with 'I' prefix | `IMyInterface`              |
| Enums             | PascalCase with 'e' prefix | `eMyEnum`                   |

## Best Practices Checklist

- [ ] Follow single responsibility principle
- [ ] Use dependency injection
- [ ] Implement proper error handling
- [ ] Write unit tests
- [ ] Use TypeScript strictly
- [ ] Follow ABP conventions
- [ ] Document public APIs
- [ ] Optimize for performance
- [ ] Implement proper validation
- [ ] Use reactive forms
- [ ] Handle lifecycle properly
- [ ] Implement proper security

## Common Issues & Solutions

### Module Not Found

- Check import paths
- Verify module declarations
- Ensure proper exports

### Circular Dependencies

- Use `forwardRef()`
- Restructure imports
- Move shared code to separate modules

### Memory Leaks

- Unsubscribe from observables
- Implement `OnDestroy`
- Use `takeUntil` operator

### Performance Issues

- Use `OnPush` change detection
- Implement lazy loading
- Optimize bundle size

## Useful Resources

- [ABP Documentation](https://docs.abp.io)
- [Angular Documentation](https://angular.io/docs)
- [TypeScript Handbook](https://www.typescriptlang.org/docs)
- [RxJS Documentation](https://rxjs.dev)
- [ABP Community](https://community.abp.io)
