# Unit Testing Angular UI

ABP Angular UI is tested like any other Angular application. So, [the guide here](https://angular.io/guide/testing) applies to ABP too. That said, we would like to point out some **unit testing topics specific to ABP Angular applications**.

## Setup

In Angular, unit tests use [Karma](https://karma-runner.github.io/) and [Jasmine](https://jasmine.github.io) by default. Although we like Jest more, we chose not to deviate from these defaults, so **the application template you download will have Karma and Jasmine preconfigured**. You can find the Karma configuration inside the _karma.conf.js_ file in the root folder. You don't have to do anything. Adding a spec file and running `npm test` will work.

## Basics

An over-simplified spec file looks like this:

```js
import { CoreTestingModule } from "@abp/ng.core/testing";
import { ThemeBasicTestingModule } from "@abp/ng.theme.basic/testing";
import { ThemeSharedTestingModule } from "@abp/ng.theme.shared/testing";
import { ComponentFixture, TestBed, waitForAsync } from "@angular/core/testing";
import { NgxValidateCoreModule } from "@ngx-validate/core";
import { MyComponent } from "./my.component";

describe("MyComponent", () => {
  let fixture: ComponentFixture<MyComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [MyComponent],
        imports: [
          CoreTestingModule.withConfig(),
          ThemeSharedTestingModule.withConfig(),
          ThemeBasicTestingModule.withConfig(),
          NgxValidateCoreModule,
        ],
        providers: [
          /* mock providers here */
        ],
      }).compileComponents();
    })
  );

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

## Tips

### Angular Testing Library

Although you can test your code with Angular TestBed, you may find [Angular Testing Library](https://testing-library.com/docs/angular-testing-library/intro) a good alternative.

The simple example above can be written with Angular Testing Library as follows:

```js
import { CoreTestingModule } from "@abp/ng.core/testing";
import { ThemeBasicTestingModule } from "@abp/ng.theme.basic/testing";
import { ThemeSharedTestingModule } from "@abp/ng.theme.shared/testing";
import { ComponentFixture } from "@angular/core/testing";
import { NgxValidateCoreModule } from "@ngx-validate/core";
import { render } from "@testing-library/angular";
import { MyComponent } from "./my.component";

describe("MyComponent", () => {
  let fixture: ComponentFixture<MyComponent>;

  beforeEach(async () => {
    const result = await render(MyComponent, {
      imports: [
        CoreTestingModule.withConfig(),
        ThemeSharedTestingModule.withConfig(),
        ThemeBasicTestingModule.withConfig(),
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

Very similar, as you can see. The real difference kicks in when we use queries and fire events.

```js
// other imports
import { getByLabelText, screen } from "@testing-library/angular";
import userEvent from "@testing-library/user-event";

describe("MyComponent", () => {
  beforeEach(/* removed for sake of brevity */);

  it("should display advanced filters", () => {
    const filters = screen.getByTestId("author-filters");
    const nameInput = getByLabelText(filters, /name/i) as HTMLInputElement;
    expect(nameInput.offsetWidth).toBe(0);

    const advancedFiltersBtn = screen.getByRole("link", { name: /advanced/i });
    userEvent.click(advancedFiltersBtn);

    expect(nameInput.offsetWidth).toBeGreaterThan(0);

    userEvent.type(nameInput, "fooo{backspace}");
    expect(nameInput.value).toBe("foo");
  });
});
```

The **queries in Angular Testing Library follow practices for maintainable tests**, the user event package provides a **human-like interaction** with the DOM, and the library in general has **a clear API** that simplifies component testing. Please find some useful links below:

- [Queries](https://testing-library.com/docs/dom-testing-library/api-queries)
- [User Event](https://testing-library.com/docs/ecosystem-user-event)
- [Examples](https://github.com/testing-library/angular-testing-library/tree/main/apps/example-app/src/app/examples)

### Clearing DOM After Each Spec

One thing to remember is that Karma runs tests in real browser instances. That means, you will be able to see the result of your test code, but also have problems with components attached to the document body which may not get cleared after each test, even when you configure Karma to do so.

We have prepared a simple function with which you can clear any leftover DOM elements after each test.

```js
// other imports
import { clearPage } from "@abp/ng.core/testing";

describe("MyComponent", () => {
  let fixture: ComponentFixture<MyComponent>;

  afterEach(() => clearPage(fixture));

  beforeEach(async () => {
    const result = await render(MyComponent, {
      /* removed for sake of brevity */
    });
    fixture = result.fixture;
  });

  // specs here
});
```

Please make sure you use it because Karma will fail to remove dialogs otherwise and you will have multiple copies of modals, confirmation boxes, and alike.

### Waiting

Some components, modals, in particular, work off-detection-cycle. In other words, you cannot reach DOM elements inserted by these components immediately after opening them. Similarly, inserted elements are not immediately destroyed upon closing them.

For this purpose, we have prepared a `wait` function.

```js
// other imports
import { wait } from "@abp/ng.core/testing";

describe("MyComponent", () => {
  beforeEach(/* removed for sake of brevity */);

  it("should open a modal", async () => {
    const openModalBtn = screen.getByRole("button", { name: "Open Modal" });
    userEvent.click(openModalBtn);

    await wait(fixture);

    const modal = screen.getByRole("dialog");

    expect(modal).toBeTruthy();

    /* wait again after closing the modal */
  });
});
```

The `wait` function takes a second parameter, i.e. timeout (default: `0`). Try not to use it though. Using a timeout bigger than `0` is usually a signal that something is not quite right.

## Testing Example

Here is an example test suite. It doesn't cover all, but gives quite a good idea about what the testing experience will be like.

```js
import { clearPage, CoreTestingModule, wait } from "@abp/ng.core/testing";
import { ThemeBasicTestingModule } from "@abp/ng.theme.basic/testing";
import { ThemeSharedTestingModule } from "@abp/ng.theme.shared/testing";
import { ComponentFixture } from "@angular/core/testing";
import {
  NgbCollapseModule,
  NgbDatepickerModule,
  NgbDropdownModule,
} from "@ng-bootstrap/ng-bootstrap";
import { NgxValidateCoreModule } from "@ngx-validate/core";
import { CountryService } from "@proxy/countries";
import {
  findByText,
  getByLabelText,
  getByRole,
  getByText,
  queryByRole,
  render,
  screen,
} from "@testing-library/angular";
import userEvent from "@testing-library/user-event";
import { BehaviorSubject, of } from "rxjs";
import { CountryComponent } from "./country.component";

const list$ = new BehaviorSubject({
  items: [{ id: "ID_US", name: "United States of America" }],
  totalCount: 1,
});

describe("Country", () => {
  let fixture: ComponentFixture<CountryComponent>;

  afterEach(() => clearPage(fixture));

  beforeEach(async () => {
    const result = await render(CountryComponent, {
      imports: [
        CoreTestingModule.withConfig(),
        ThemeSharedTestingModule.withConfig(),
        ThemeBasicTestingModule.withConfig(),
        NgxValidateCoreModule,
        NgbCollapseModule,
        NgbDatepickerModule,
        NgbDropdownModule,
      ],
      providers: [
        {
          provide: CountryService,
          useValue: {
            getList: () => list$,
          },
        },
      ],
    });

    fixture = result.fixture;
  });

  it("should display advanced filters", () => {
    const filters = screen.getByTestId("country-filters");
    const nameInput = getByLabelText(filters, /name/i) as HTMLInputElement;
    expect(nameInput.offsetWidth).toBe(0);

    const advancedFiltersBtn = screen.getByRole("link", { name: /advanced/i });
    userEvent.click(advancedFiltersBtn);

    expect(nameInput.offsetWidth).toBeGreaterThan(0);

    userEvent.type(nameInput, "fooo{backspace}");
    expect(nameInput.value).toBe("foo");

    userEvent.click(advancedFiltersBtn);
    expect(nameInput.offsetWidth).toBe(0);
  });

  it("should have a heading", () => {
    const heading = screen.getByRole("heading", { name: "Countries" });
    expect(heading).toBeTruthy();
  });

  it("should render list in table", async () => {
    const table = await screen.findByTestId("country-table");

    const name = getByText(table, "United States of America");
    expect(name).toBeTruthy();
  });

  it("should display edit modal", async () => {
    const actionsBtn = screen.queryByRole("button", { name: /actions/i });
    userEvent.click(actionsBtn);

    const editBtn = screen.getByRole("button", { name: /edit/i });
    userEvent.click(editBtn);

    await wait(fixture);

    const modal = screen.getByRole("dialog");
    const modalHeading = queryByRole(modal, "heading", { name: /edit/i });
    expect(modalHeading).toBeTruthy();

    const closeBtn = getByText(modal, "×");
    userEvent.click(closeBtn);

    await wait(fixture);

    expect(screen.queryByRole("dialog")).toBeFalsy();
  });

  it("should display create modal", async () => {
    const newBtn = screen.getByRole("button", { name: /new/i });
    userEvent.click(newBtn);

    await wait(fixture);

    const modal = screen.getByRole("dialog");
    const modalHeading = queryByRole(modal, "heading", { name: /new/i });

    expect(modalHeading).toBeTruthy();
  });

  it("should validate required name field", async () => {
    const newBtn = screen.getByRole("button", { name: /new/i });
    userEvent.click(newBtn);

    await wait(fixture);

    const modal = screen.getByRole("dialog");
    const nameInput = getByRole(modal, "textbox", {
      name: /^name/i,
    }) as HTMLInputElement;

    userEvent.type(nameInput, "x");
    userEvent.type(nameInput, "{backspace}");

    const nameError = await findByText(modal, /required/i);
    expect(nameError).toBeTruthy();
  });

  it("should delete a country", () => {
    const getSpy = spyOn(fixture.componentInstance.list, "get");
    const deleteSpy = jasmine.createSpy().and.returnValue(of(null));
    fixture.componentInstance.service.delete = deleteSpy;

    const actionsBtn = screen.queryByRole("button", { name: /actions/i });
    userEvent.click(actionsBtn);

    const deleteBtn = screen.getByRole("button", { name: /delete/i });
    userEvent.click(deleteBtn);

    const confirmText = screen.getByText("AreYouSure");
    expect(confirmText).toBeTruthy();

    const confirmBtn = screen.getByRole("button", { name: "Yes" });
    userEvent.click(confirmBtn);

    expect(deleteSpy).toHaveBeenCalledWith(list$.value.items[0].id);
    expect(getSpy).toHaveBeenCalledTimes(1);
  });
});
```

## CI Configuration

You would need a different configuration for your CI environment. To set up a new configuration for your unit tests, find the test project in _angular.json_ file and add one as seen below:

```json
// angular.json

"test": {
  "builder": "@angular-devkit/build-angular:karma",
  "options": { /* several options here */ },
  "configurations": {
    "production": {
      "karmaConfig": "karma.conf.prod.js"
    }
  }
}
```

Now you can copy the _karma.conf.js_ as _karma.conf.prod.js_ and use any configuration you like in it. Please check [Karma configuration file document](http://karma-runner.github.io/5.2/config/configuration-file.html) for config options.

Finally, don't forget to run your CI tests with the following command:

```sh
npm test -- --prod
```

## See Also

- [ABP Community Video - Unit Testing with the Angular UI](https://community.abp.io/articles/unit-testing-with-the-angular-ui-p4l550q3)
