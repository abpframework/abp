```json
//[doc-seo]
{
    "Description": "Learn how to unit test your ABP Angular UI applications with preconfigured Vitest and TestBed, plus ABP-specific testing topics."
}
```

# Unit Testing Angular UI

ABP Angular UI is tested like any other Angular application. So, [the guide here](https://angular.dev/guide/testing) applies to ABP too. That said, we would like to point out some **unit testing topics specific to ABP Angular applications**.

## Test Stack

The application template you download is preconfigured for unit testing. You can add a `*.spec.ts` file and run `yarn test` without adding extra test infrastructure.

| Package / API | Purpose |
| --- | --- |
| [Vitest](https://vitest.dev/) | Test runner and assertion library. |
| [jsdom](https://github.com/jsdom/jsdom) | Browser-like DOM environment for component tests. |
| `@angular/core/testing` (`TestBed`) | The standard testing utilities of Angular for components, services, and pipes. |
| `@abp/ng.core/testing` | ABP testing module and helpers that replace real ABP services with mocks. |
| `@abp/ng.theme.shared/testing` | Testing module for shared theme features such as validation. |

ABP Angular packages in the [framework repository](https://github.com/abpframework/abp/tree/dev/npm/ng-packs) use the same Vitest setup. Library tests there also use [`@ngneat/spectator/vitest`](https://github.com/ngneat/spectator) for HTTP and component tests, but the application template uses `TestBed` directly.

## Configuration

The test target in _angular.json_ uses Angular's built-in Vitest builder:

```json
// angular.json

"test": {
  "builder": "@angular/build:unit-test"
}
```

Spec files are compiled with _tsconfig.spec.json_, which enables Vitest globals:

```json
// tsconfig.spec.json

{
  "compilerOptions": {
    "types": ["vitest/globals"]
  },
  "include": ["src/**/*.spec.ts"]
}
```

You do not need a _karma.conf.js_ file. Angular CLI wires Vitest and jsdom for you.

## Running Tests

Run tests in watch mode:

```bash
yarn test
```

Run tests once, which is useful for CI:

```bash
ng test --watch=false
```

Vitest exits with a non-zero status code when a test fails, so the command above works in pipelines.

## Basics

An over-simplified spec file looks like this:

```ts
import { CoreTestingModule } from "@abp/ng.core/testing";
import { ThemeSharedTestingModule } from "@abp/ng.theme.shared/testing";
import { ComponentFixture, TestBed } from "@angular/core/testing";
import { NgxValidateCoreModule } from "@ngx-validate/core";
import { AuthService } from "@abp/ng.core";
import { vi } from "vitest";
import { MyComponent } from "./my.component";

describe("MyComponent", () => {
  let fixture: ComponentFixture<MyComponent>;
  let mockAuthService: { isAuthenticated: boolean; navigateToLogin: ReturnType<typeof vi.fn> };

  beforeEach(async () => {
    mockAuthService = {
      isAuthenticated: false,
      navigateToLogin: vi.fn(),
    };

    await TestBed.configureTestingModule({
      imports: [
        CoreTestingModule.withConfig(),
        ThemeSharedTestingModule.withConfig(),
        NgxValidateCoreModule,
        MyComponent,
      ],
      providers: [
        {
          provide: AuthService,
          useValue: mockAuthService,
        },
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MyComponent);
    fixture.detectChanges();
  });

  it("should be initiated", () => {
    expect(fixture.componentInstance).toBeTruthy();
  });
});
```

If you take a look at the imports, you will notice that we have prepared some testing modules to replace built-in ABP modules. This is necessary for providing mocks for some features which otherwise would break your tests. Please remember to **use testing modules** and **call their `withConfig` static method**.

Current templates use standalone components, so put the component under test in the `imports` array instead of `declarations`.

If your application uses `@abp/ng.theme.basic`, also import `ThemeBasicTestingModule.withConfig()` from `@abp/ng.theme.basic/testing`.

### Mocking Dependencies

Use Vitest mocks instead of Jasmine spies:

```ts
import { vi } from "vitest";

const deleteSpy = vi.fn().mockReturnValue(of(null));
fixture.componentInstance.service.delete = deleteSpy;

expect(deleteSpy).toHaveBeenCalledWith("some-id");
```

The template's `home.component.spec.ts` is a good reference for mocking ABP services and asserting DOM behavior with `TestBed`.

### Configuring Permission Results

`CoreTestingModule.withConfig()` replaces `PermissionService` with `MockPermissionService`. The mock grants every policy by default, which keeps unrelated permission checks from hiding components in a test.

Use `grantPolicies` when a spec needs a specific authorization state. Policies not included in the array are denied:

```ts
import { PermissionService } from '@abp/ng.core';
import { MockPermissionService } from '@abp/ng.core/testing';
import { TestBed } from '@angular/core/testing';

let permissionService: MockPermissionService;

beforeEach(() => {
  permissionService = TestBed.inject(PermissionService) as MockPermissionService;
});

it('shows the create action only for the create policy', () => {
  permissionService.grantPolicies(['BookStore.Books.Create']);

  fixture.detectChanges();

  expect(fixture.nativeElement.querySelector('[data-testid="create-book"]')).toBeTruthy();
  expect(fixture.nativeElement.querySelector('[data-testid="delete-book"]')).toBeFalsy();
});
```

Call `grantAllPolicies()` to restore the grant-all behavior in a test that previously selected individual policies.

## Tips

### Clearing DOM After Each Spec

Tests run in jsdom, not a real browser. Components attached to `document.body` — such as modals, confirmation dialogs, and toasts — may not be removed automatically between specs.

We have prepared a simple function with which you can clear leftover DOM elements after each test:

```ts
// other imports
import { clearPage } from "@abp/ng.core/testing";

describe("MyComponent", () => {
  let fixture: ComponentFixture<MyComponent>;

  afterEach(() => clearPage(fixture));

  // specs here
});
```

Please use it when you test features that render into the document body. Otherwise you may end up with multiple copies of modals, confirmation boxes, and similar elements.

### Waiting

Some components, modals in particular, work off the change-detection cycle. In other words, you cannot reach DOM elements inserted by these components immediately after opening them. Similarly, inserted elements are not immediately destroyed upon closing them.

For this purpose, we have prepared a `wait` function:

```ts
// other imports
import { wait } from "@abp/ng.core/testing";

describe("MyComponent", () => {
  let fixture: ComponentFixture<MyComponent>;

  it("should open a modal", async () => {
    const openModalBtn = fixture.nativeElement.querySelector('[role="button"]');
    openModalBtn.click();

    await wait(fixture);

    const modal = fixture.nativeElement.ownerDocument.querySelector('[role="dialog"]');
    expect(modal).toBeTruthy();
  });
});
```

The `wait` function takes a second parameter, i.e. timeout (default: `0`). Try not to use it though. Using a timeout bigger than `0` is usually a signal that something is not quite right.

### Angular Testing Library

Although you can test your code with Angular TestBed, you may find [Angular Testing Library](https://testing-library.com/docs/angular-testing-library/intro) a good alternative. It is not included in the application template by default, but you can add `@testing-library/angular` and `@testing-library/user-event` if you prefer that style.

The ABP testing modules work the same way with Testing Library:

```ts
import { CoreTestingModule } from "@abp/ng.core/testing";
import { ThemeSharedTestingModule } from "@abp/ng.theme.shared/testing";
import { ComponentFixture } from "@angular/core/testing";
import { NgxValidateCoreModule } from "@ngx-validate/core";
import { render, screen } from "@testing-library/angular";
import { MyComponent } from "./my.component";

describe("MyComponent", () => {
  let fixture: ComponentFixture<MyComponent>;

  beforeEach(async () => {
    const result = await render(MyComponent, {
      imports: [
        CoreTestingModule.withConfig(),
        ThemeSharedTestingModule.withConfig(),
        NgxValidateCoreModule,
      ],
      providers: [
        /* mock providers here */
      ],
    });

    fixture = result.fixture;
  });

  it("should be initiated", () => {
    expect(fixture.componentInstance).toBeTruthy();
  });
});
```

The **queries in Angular Testing Library follow practices for maintainable tests**, the user event package provides a **human-like interaction** with the DOM, and the library in general has **a clear API** that simplifies component testing. Please find some useful links below:

- [Queries](https://testing-library.com/docs/dom-testing-library/api-queries)
- [User Event](https://testing-library.com/docs/ecosystem-user-event)
- [Examples](https://github.com/testing-library/angular-testing-library/tree/main/apps/example-app/src/app/examples)

When you use Testing Library with modals or confirmation dialogs, combine it with `clearPage` and `wait` from `@abp/ng.core/testing` as shown above.

## Testing Example

Here is an example based on the application template's `home.component.spec.ts`. It shows how to mock an ABP service and assert component state and DOM output:

```ts
import { CoreTestingModule } from "@abp/ng.core/testing";
import { ThemeSharedTestingModule } from "@abp/ng.theme.shared/testing";
import { ComponentFixture, TestBed } from "@angular/core/testing";
import { NgxValidateCoreModule } from "@ngx-validate/core";
import { AuthService } from "@abp/ng.core";
import { vi } from "vitest";
import { HomeComponent } from "./home.component";

describe("HomeComponent", () => {
  let fixture: ComponentFixture<HomeComponent>;
  let mockAuthService: { isAuthenticated: boolean; navigateToLogin: ReturnType<typeof vi.fn> };

  beforeEach(async () => {
    mockAuthService = {
      isAuthenticated: false,
      navigateToLogin: vi.fn(),
    };

    await TestBed.configureTestingModule({
      imports: [
        CoreTestingModule.withConfig(),
        ThemeSharedTestingModule.withConfig(),
        NgxValidateCoreModule,
        HomeComponent,
      ],
      providers: [
        {
          provide: AuthService,
          useValue: mockAuthService,
        },
      ],
    }).compileComponents();
  });

  it("should be initiated", () => {
    fixture = TestBed.createComponent(HomeComponent);
    fixture.detectChanges();
    expect(fixture.componentInstance).toBeTruthy();
  });

  describe("when login state is false", () => {
    beforeEach(() => {
      mockAuthService.isAuthenticated = false;
      fixture = TestBed.createComponent(HomeComponent);
      fixture.detectChanges();
    });

    it("hasLoggedIn should be false", () => {
      expect(fixture.componentInstance.hasLoggedIn).toBe(false);
    });

    it("button should exist", () => {
      const button = fixture.nativeElement.querySelector('[role="button"]');
      expect(button).toBeDefined();
    });

    describe("when button clicked", () => {
      beforeEach(() => {
        const button = fixture.nativeElement.querySelector('[role="button"]');
        button.click();
      });

      it("navigateToLogin should have been called", () => {
        expect(mockAuthService.navigateToLogin).toHaveBeenCalled();
      });
    });
  });
});
```

For list pages with modals, confirmations, and service proxies, keep using the ABP testing modules, mock your generated proxy services with `vi.fn()`, and use `clearPage` / `wait` when body-level UI is involved.

## CI Configuration

Run unit tests once in CI with:

```sh
ng test --watch=false
```

If you need a dedicated CI configuration, add one under the `test` target in _angular.json_:

```json
// angular.json

"test": {
  "builder": "@angular/build:unit-test",
  "configurations": {
    "ci": {
      "watch": false
    }
  }
}
```

Then run:

```sh
ng test --configuration=ci
```

## See Also

- [ABP Community Video - Unit Testing with the Angular UI](https://abp.io/community/articles/unit-testing-with-the-angular-ui-p4l550q3)
