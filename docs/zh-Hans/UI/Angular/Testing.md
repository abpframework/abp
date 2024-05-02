# Angular UI 单元测试

ABP Angular UI的测试与其他Angular应用程序一样. 所以, [这里的指南](https://angular.io/guide/testing)也适用于ABP. 也就是说, 我们想指出一些**特定于ABP Angular应用程序的单元测试内容**.

## 设置

在Angular中, 单元测试默认使用[Karma](https://karma-runner.github.io/)和[Jasmine](https://jasmine.github.io). 虽然我们更喜欢Jest, 但我们选择不偏离这些默认设置, 因此**你下载的应用程序模板将预先配置Karma和Jasmine**. 你可以在根目录中的 _karma.conf.js_ 文件中找到Karma配置. 你什么都不用做. 添加一个spec文件并运行`npm test`即可.

## 基础

简化版的spec文件如下所示:

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

如果你看一下导入内容, 你会注意到我们已经准备了一些测试模块来取代内置的ABP模块. 这对于模拟某些特性是必要的, 否则这些特性会破坏你的测试. 请记住**使用测试模块**并**调用其`withConfig`静态方法**.

## 提示

### Angular测试库

虽然你可以使用Angular TestBed测试代码, 但你可以找到一个好的替代品[Angular测试库](https://testing-library.com/docs/angular-testing-library/intro).

上面的简单示例可以用Angular测试库编写, 如下所示:

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

正如你所见, 二者非常相似. 当我们使用查询和触发事件时, 真正的区别就显现出来了.

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

**Angular测试库中的查询遵循可维护测试**, 用户事件库提供了与DOM的**类人交互**, 并且该库通常有**清晰的API**简化组件测试. 下面提供一些有用的链接:

- [查询](https://testing-library.com/docs/dom-testing-library/api-queries)
- [用户事件](https://testing-library.com/docs/ecosystem-user-event)
- [范例](https://github.com/testing-library/angular-testing-library/tree/main/apps/example-app/src/app/examples)

### 在每个Spec之后清除DOM

需要记住的一点是, Karma在真实的浏览器实例中运行测试. 这意味着, 你将能够看到测试代码的结果, 但也会遇到与文档正文连接的组件的问题, 这些组件可能无法在每次测试后都清除, 即使你配置了Karma也一样无法清除.

我们准备了一个简单的函数, 可以在每次测试后清除所有剩余的DOM元素.

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

请确保你使用它, 否则Karma将无法删除对话框, 并且你将有多个模态对话框、确认框等的副本.

### 等待

一些组件, 特别是在检测周期之外工作的模态对话框. 换句话说, 你无法在打开这些组件后立即访问这些组件插入的DOM元素. 同样, 插入的元素在关闭时也不会立即销毁.

为此, 我们准备了一个`wait`函数.

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

`wait`函数接受第二个参数, 即超时(默认值为`0`). 但是尽量不要使用它. 使用大于`0`的超时通常表明某些不正确事情发生了.

## 测试示例

下面是一个测试示例. 它并没有涵盖所有内容, 但却能够对测试有一个更好的了解.

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

## CI配置

你的CI环境需要不同的配置. 要为单元测试设置新的配置, 请在测试项目中找到 _angular.json_ 文件, 或者如下所示添加一个:

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

现在你可以复制 _karma.conf.js_ 作为 _karma.conf.prod.js_ 并在其中使用你喜欢的任何配置. 请查看[Karma配置文档](http://karma-runner.github.io/5.2/config/configuration-file.html)配置选项.

最后, 不要忘记使用以下命令运行CI测试:

```sh
npm test -- --prod
```

## 另请参阅

- [ABP Community Video - Unit Testing with the Angular UI](https://community.abp.io/articles/unit-testing-with-the-angular-ui-p4l550q3)
