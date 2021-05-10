# Page Component

ABP provides a component that wraps your content with some built-in components to reduce the amount of code you need to write.

If the template of a component looks as follows, you can utilize the `abp-page` component.

Let's look at the following example without `abp-page` component.

`dashboard.component.ts`

```html
<div class="row entry-row">
  <div class="col-auto">
    <h1 class="content-header-title">{%{{ '::Dashboard' | abpLocalization }}%}</h1>
  </div>
  <div id="breadcrumb" class="col-lg-auto pl-lg-0">
    <abp-breadcrumb></abp-breadcrumb>
  </div>
  <div class="col">
    <abp-page-toolbar [record]="data"></abp-page-toolbar>
  </div>
</div>

<div id="dashboard-id">
  <!-- dashboard content here -->
</div>
```

## Page Parts

PageComponent divides the template shown above into three parts, `title`, `breadcrumb`, `toolbar`. Each can be configured separately. There, also, is an enum exported from the package that describes each part.

```javascript
export enum PageParts {
  title = 'PageTitleContainerComponent',
  breadcrumb = 'PageBreadcrumbContainerComponent',
  toolbar = 'PageToolbarContainerComponent',
}

// You can import this enum from -> import { PageParts } from '@abp/ng.components/page';
```

## Usage

Firstly, you need to import `PageModule` from `@abp/ng.components/page` as follows:

`dashboard.module.ts`

```javascript
import { PageModule } from '@abp/ng.components/page';
import { DashboardComponent } from './dashboard.component';

@NgModule({
  declarations: [DashboardComponent],
  imports: [PageModule]
})
export class DashboardModule {}
```

And change the template of `dashboard.component.ts` to the following:

```html
<abp-page [title]="'::Dashboard' | abpLocalization" [toolbar]="data">
  <div id="dashboard-id">
    <!-- .... -->
  </div>
</abp-page>
```

## Inputs

* title: `string`: Will be be rendered within `h1.content-header-title`. If not provided, the parent `div` will not be rendered
* breadcrumb: `boolean`: Determines whether to render `abp-breadcrumb`. Default is `true`.
* toolbar: `any`: Will be passed into `abp-page-toolbar` component through `record` input. If your page does not contain `abp-page-toolbar`, you can simply omit this field.

## Overriding template

If you need to replace the template of any part, you can use the following sub-components. 

```html
<abp-page>
  <abp-page-title-container>
    <div class="col">
      <h2>Custom Title</h2>
    </div>
  </abp-page-title-container>

  <abp-page-breacrumb-container>
    <div class="col">
      <my-breadcrumb></my-breadcrumb>
    </div>
  </abp-page-breacrumb-container>

  <abp-page-toolbar-container>
    <div class="col">
      <!-- ... -->
    </div>
  </abp-page-toolbar-container>
</abp-page>
```

You do not have to provide them all. You can just use which one you need to replace. These components have priority over the inputs declared above. If you use these components, you can omit the inputs.

## PagePartDirective

`PageModule` provides a structural directive that is used internally within `PageComponent` and can also be used externally.

`PageComponent` employs this directive internally as follows: 

```html
<div class="col-lg-auto pl-lg-0" *abpPagePart="pageParts.breadcrumb">
  <abp-breadcrumb></abp-breadcrumb>
</div>
```

It also can take a context input as follows: 

```html
<div class="col" *abpPagePart="pageParts.toolbar; context: toolbarData">
  <abp-page-toolbar [record]="toolbarData"></abp-page-toolbar>
</div>
```

Its render strategy can be provided through Angular's Dependency Injection system. 

It expects a service through the `PAGE_RENDER_STRATEGY` injection token that implements the following interface.

```javascript
interface PageRenderStrategy {
  shouldRender(type?: string): boolean | Observable<boolean>;
  onInit?(type?: string, injector?: Injector, context?: any): void;
  onDestroy?(type?: string, injector?: Injector, context?: any): void;
  onContextUpdate?(change?: SimpleChange): void;
}
```

* `shouldRender` (required): It takes a string input named `type` and expects a `boolean` or `Observable<boolean>` in return. 
* `onInit` (optional): Will be called when the directive is initiated. Three inputs will be passed into this method. 
  * `type`: type of the page part
  * `injector`: injector of the directive which could be used to retrieve anything from directive's DI tree.
  * `context`: whatever context is available at the initialization phase. 
* `onDestroy` (optional): Will be called when the directive is destroyed. The parameters are the same with `onInit`
* `onContextUpdate` (optional): Will be called when the context is updated. 
  * `change`: changes of the `context` will be passed through this method. 

Let's see everything in action.

```javascript
import { 
  PageModule,
  PageRenderStrategy, 
  PageParts,
  PAGE_RENDER_STRATEGY
} from '@abp/ng.components/page';

@Injectable()
export class MyPageRenderStrategy implements PageRenderStrategy {
  shouldRender(type: string) {
    // meaning everything but breadcrumb and custom-part will be rendered
    return type !== PageParts.breadcrumb && type !== 'custom-part';
  }

  /**
   * shouldRender can also return an Observable<boolean> which means
   * an async service can be used within.

  constructor(private service: SomeAsyncService) {}

  shouldRender(type: string) {
    return this.service.checkTypeAsync(type).pipe(map(val => val.isTrue()));
  }
  */
   
  onInit(type: string, injector: Injector, context: any) {
    // this method will be called in ngOnInit of the directive
  }

  onDestroy(type: string, injector: Injector, context: any) {
    // this method will be called in ngOnDestroy of the directive
  }

  onContextUpdate?(change?: SimpleChange) {
    // this method will be called everytime context is updated within the directive
  }
}

@Component({
  selector: 'app-dashboard',
  template: `
    <abp-page [title]="'::Dashboard' | abpLocalization">
      <abp-page-toolbar-container>
        <button>New Dashboard</button>
      </abp-page-toolbar-container>

      <div class="dashboard-content">
        <h3 *abpPagePart="'custom-part'"> Inner Title </h3>
      </div>
    </abp-page>
  `
})
export class DashboardComponent {}

@NgModule({
  imports: [PageModule],
  declarations: [DashboardComponent],
  providers: [
    {
      provide: PAGE_RENDER_STRATEGY,
      useClass: MyPageRenderStrategy,
    }
  ]
})
export class DashboardModule {}
```

## See Also

- [Page Toolbar Extensions for Angular UI](./Page-Page-Toolbar-Extensions.md)
