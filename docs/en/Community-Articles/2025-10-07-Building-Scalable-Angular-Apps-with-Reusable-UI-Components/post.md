# Building Scalable Angular Apps with Reusable UI Components

Frontend development keeps evolving at an incredible pace, and with every new update, our implementation standards improve as well. But even as tools and frameworks change, the core principles stay the same, and one of the most important is reusability.

Reusability means building components and utilities that can be used in multiple places instead of using the same logic repeatedly. This approach not only saves time but also keeps your code clean, consistent, and easier to maintain as your project grows.

Angular fully embraces this idea by offering modern features like **standalone components**, **signals**, **hybrid rendering**, and **component-level lazy loading**.

In this article, we will explore how these features make it easier to build reusable UI components. We will also look at how to style them and organize them into shared libraries for scalable, long-term development.

---

## 🧩 Breaking Down Components for True Reusability

The first approach to make an Angular component reusable is to use standalone components. As this feature has been supported for a long time, it is now the default behavior for the latest Angular versions. Keeping that in mind, we can ensure reusability by separating a big component into smaller ones to make the small pieces usable across the application.

Here is a quick example:

Imagine you start with a single `UserProfileComponent` that does everything including displaying user info, recent posts, a list of friends, and even handling profile editing.

```ts
// 📖 Compact user profile component
import { Component } from "@angular/core";

@Component({
  selector: "app-user-profile",
  template: `
    <section class="profile">
      <div class="header">
        <img [src]="user.avatar" alt="User avatar" />
        <h2>{{ user.name }}</h2>
        <button (click)="editProfile()">Edit</button>
      </div>

      <div class="posts">
        <h3>Recent Posts</h3>
        <ul>
          @for (post of user.posts; track post) {
          <li>{{ post }}</li>
          }
        </ul>
      </div>

      <div class="friends">
        <h3>Friends</h3>
        <ul>
          @for (friend of user.friends; track friend) {
          <li>{{ friend }}</li>
          }
        </ul>
      </div>
    </section>
  `,
})
export class UserProfileComponent {
  user = {
    name: "Jane Doe",
    avatar: "/assets/avatar.png",
    posts: ["Angular Tips", "Reusable Components FTW!"],
    friends: ["John", "Mary", "Steve"],
  };

  editProfile() {
    console.log("Editing profile...");
  }
}
```

Instead of this, you can create small components like these:

- `user-avatar.component.ts`
- `user-posts.component.ts`
- `user-friends.component.ts`

```ts
// 🧩 user-avatar.component.ts
import { Component, input } from "@angular/core";

@Component({
  selector: "app-user-avatar",
  template: `
    <div class="user-avatar">
      <img [src]="avatar()" alt="User avatar" />
      <h2>{{ name() }}</h2>
    </div>
  `,
})
export class UserAvatarComponent {
  name = input.required<string>();
  avatar = input.required<string>();
}
```

```ts
// 🧩 user-posts.component.ts
import { Component, input } from "@angular/core";

@Component({
  selector: "app-user-posts",
  template: `
    <div class="user-posts">
      <h3>Recent Posts</h3>
      <ul>
        @for (post of posts(); track post) {
        <li>{{ post }}</li>
        }
      </ul>
    </div>
  `,
})
export class UserPostsComponent {
  posts = input<string[]>([]);
}
```

```ts
// 🧩 user-friends.component.ts
import { Component, input, output } from "@angular/core";

@Component({
  selector: "app-user-friends",
  template: `
    <div class="user-friends">
      <h3>Friends</h3>
      <ul>
        @for (friend of friends(); track friend) {
        <li (click)="selectFriend(friend)">{{ friend }}</li>
        }
      </ul>
    </div>
  `,
})
export class UserFriendsComponent {
  friends = input<string[]>([]);
  friendSelected = output<string>();

  selectFriend(friend: string) {
    this.friendSelected.emit(friend);
  }
}
```

Then, you can use them in a container component like this

```ts
// 🧩 new user profile components that uses other user components
import { Component } from "@angular/core";
import { signal } from "@angular/core";
import { UserAvatarComponent } from "./user-avatar.component";
import { UserPostsComponent } from "./user-posts.component";
import { UserFriendsComponent } from "./user-friends.component";

@Component({
  selector: "app-user-profile",
  imports: [UserAvatarComponent, UserPostsComponent, UserFriendsComponent],
  template: `
    <section class="profile">
      <app-user-avatar [name]="user().name" [avatar]="user().avatar" />
      <app-user-posts [posts]="user().posts" />
      <app-user-friends
        [friends]="user().friends"
        (friendSelected)="onFriendSelected($event)"
      />
    </section>
  `,
})
export class UserProfileComponent {
  user = signal({
    name: "Jane Doe",
    avatar: "/assets/avatar.png",
    posts: ["Angular Tips", "Reusable Components FTW!"],
    friends: ["John", "Mary", "Steve"],
  });

  onFriendSelected(friend: string) {
    console.log(`Selected friend: ${friend}`);
  }
}
```

The most common problem of creating such components is over-creating new elements when you actually do not need them. So, it is a design decision that needs to be carefully taken while building the application. If misused, it can lead to:

- a management nightmare
- unnecessary lifecycle hook complexity
- extra indirect data flow (makes debugging harder)

Nevertheless, this makes the app more scalable and maintainable if correctly used. Such structure will provide:

- a clear separation of concerns as each component will maintain decided tasks
- faster feature development
- shared libraries or elements across the application

---

## 🚀 Why Standalone Components Matter

As Angular has announced standalone components starting from version 17, they have been gradually developing features that support reusability. This important feature brings a great migration for components, directives, and pipes.

Since it allows these elements to be used directly inside an `imports` array rather than through a module structure, it reinforces reusability patterns and simplifies management.

Back in the module-based structure, we used to create these components and declare them in modules. This still offers some reusability, as we can import the modules where needed. However, standalone components can be consumed both by other standalone components and modules. For this reason, migrating from the module-based structure to a fully standalone architecture brings many benefits for this concern.

---

## 🧠 Designing Components That Scale and Reuse Well

The first point you need to consider here is to encapsulate and isolate logic.

For example:

1. This counter component isolates the concept of incrementing/decrementing so the parent component will not take care of this logic except showing the result.

   ```ts
   import { Component, signal } from "@angular/core";

   @Component({
     selector: "app-counter",
     template: `
       <button (click)="decrement()">-</button>
       <span>{{ count() }}</span>
       <button (click)="increment()">+</button>
     `,
   })
   export class CounterComponent {
     private count = signal(0); // internal state

     increment() {
       this.count.update((v) => v + 1);
     }
     decrement() {
       this.count.update((v) => v - 1);
     }
   }
   ```

2. This component isolates the styles and makes the badge reusable. Styles in this component will not leak out to others, and global styles will not affect it.

   ```ts
   import { Component, ViewEncapsulation } from "@angular/core";

   @Component({
     selector: "app-badge",
     template: `<span class="badge">{{ label }}</span>`,
     styles: [
       `
         .badge {
           background: #007bff;
           color: white;
           padding: 4px 8px;
           border-radius: 4px;
         }
       `,
     ],
     encapsulation: ViewEncapsulation.Emulated, // default; isolates CSS
   })
   export class BadgeComponent {
     label = "New";
   }
   ```

3. The search component below is a very common example since it handles a business logic exposing simple inputs/outputs

   ```ts
   import { Component, input, output } from "@angular/core";

   @Component({
     selector: "app-search-box",
     template: `
       <input
         type="text"
         [value]="query()"
         (input)="onChange($event)"
         placeholder="Search..."
       />
     `,
   })
   export class SearchBoxComponent {
     query = input<string>("");
     changed = output<string>();

     onChange(event: Event) {
       const value = (event.target as HTMLInputElement).value;
       this.changed.emit(value);
     }
   }
   ```

Encapsulation ensures that each component manages its own logic without leaking details to the outside. By keeping behavior self-contained, components become easier to understand, test, and reuse. This isolation prevents unexpected side effects, keeps your UI predictable, and allows each component to evolve independently as your application grows.

At this point, we can also briefly mention smart and dumb components. Smart components handle business logic, while dumb components take care of displaying data and emitting user actions.

This separation keeps your UI structure scalable. Smart components can change how data is loaded or handled without affecting presentation components, and dumb components can be reused anywhere since they just rely on inputs and outputs.

```ts
// smart component (container)
@Component({
  selector: "app-user-profile",
  imports: [UserCardComponent],
  template: `<app-user-card [user]="user()" (select)="onSelect($event)" />`,
})
export class UserProfileComponent {
  user = signal({ name: "Jane", role: "Admin" });

  onSelect(user: any) {
    console.log("Selected user:", user);
  }
}

// dumb component (presentation)
@Component({
  selector: "app-user-card",
  standalone: true,
  template: `
    <div (click)="select.emit(user())" class="card">
      <h3>{{ user().name }}</h3>
      <p>{{ user().role }}</p>
    </div>
  `,
})
export class UserCardComponent {
  user = input.required<{ name: string; role: string }>();
  select = output<{ name: string; role: string }>();
}
```

---

## 🔁 Reusing Components Across the Application

As there are many ways of reusing a component in the project, we will go over a real-life example.

Here are two very common ABP components that can be reused anywhere in the app:

```ts
//...
import { ABP } from "@abp/ng.core";

@Component({
  selector: "abp-button",
  template: `
    <button
      #button
      [id]="buttonId"
      [attr.type]="buttonType"
      [attr.form]="formName"
      [ngClass]="buttonClass"
      [disabled]="loading || disabled"
      (click.stop)="click.next($event); abpClick.next($event)"
      (focus)="focus.next($event); abpFocus.next($event)"
      (blur)="blur.next($event); abpBlur.next($event)"
    >
      <i [ngClass]="icon" class="me-1" aria-hidden="true"></i
      ><ng-content></ng-content>
    </button>
  `,
  imports: [NgClass],
})
export class ButtonComponent implements OnInit {
  private renderer = inject(Renderer2);

  @Input()
  buttonId = "";

  @Input()
  buttonClass = "btn btn-primary";

  @Input()
  buttonType = "button";

  @Input()
  formName?: string = undefined;

  @Input()
  iconClass?: string;

  @Input()
  loading = false;

  @Input()
  disabled: boolean | undefined = false;

  @Input()
  attributes?: ABP.Dictionary<string>;

  @Output() readonly click = new EventEmitter<MouseEvent>();

  @Output() readonly focus = new EventEmitter<FocusEvent>();

  @Output() readonly blur = new EventEmitter<FocusEvent>();

  @Output() readonly abpClick = new EventEmitter<MouseEvent>();

  @Output() readonly abpFocus = new EventEmitter<FocusEvent>();

  @Output() readonly abpBlur = new EventEmitter<FocusEvent>();

  @ViewChild("button", { static: true })
  buttonRef!: ElementRef<HTMLButtonElement>;

  get icon(): string {
    return `${
      this.loading ? "fa fa-spinner fa-spin" : this.iconClass || "d-none"
    }`;
  }

  ngOnInit() {
    if (this.attributes) {
      Object.keys(this.attributes).forEach((key) => {
        if (this.attributes?.[key]) {
          this.renderer.setAttribute(
            this.buttonRef.nativeElement,
            key,
            this.attributes[key]
          );
        }
      });
    }
  }
}
```

This button component can be used by simply importing the `ButtonComponent` and using the `<abp-button />` tag.

You can reach the source code [here](https://github.com/abpframework/abp/blob/dev/npm/ng-packs/packages/theme-shared/src/lib/components/button/button.component.ts).

This modal component is also commonly used. The source code is [here](https://github.com/abpframework/abp/blob/dev/npm/ng-packs/packages/theme-shared/src/lib/components/modal/modal.component.ts).

```ts
//...
export type ModalSize = "sm" | "md" | "lg" | "xl";

@Component({
  selector: "abp-modal",
  templateUrl: "./modal.component.html",
  styleUrls: ["./modal.component.scss"],
  providers: [SubscriptionService],
  imports: [NgTemplateOutlet],
})
export class ModalComponent implements OnInit, OnDestroy, DismissableModal {
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly modal = inject(NgbModal);
  protected readonly modalRefService = inject(ModalRefService);
  protected readonly suppressUnsavedChangesWarningToken = inject(
    SUPPRESS_UNSAVED_CHANGES_WARNING,
    {
      optional: true,
    }
  );
  protected readonly destroyRef = inject(DestroyRef);
  private document = inject(DOCUMENT);

  visible = model<boolean>(false);

  busy = input(false, {
    transform: (value: boolean) => {
      if (this.abpSubmit() && this.abpSubmit() instanceof ButtonComponent) {
        this.abpSubmit().loading = value;
      }
      return value;
    },
  });

  options = input<NgbModalOptions>({ keyboard: true });

  suppressUnsavedChangesWarning = input(
    this.suppressUnsavedChangesWarningToken
  );

  modalContent = viewChild<TemplateRef<any>>("modalContent");

  abpHeader = contentChild<TemplateRef<any>>("abpHeader");

  abpBody = contentChild<TemplateRef<any>>("abpBody");

  abpFooter = contentChild<TemplateRef<any>>("abpFooter");

  abpSubmit = contentChild(ButtonComponent, { read: ButtonComponent });

  readonly init = output();

  readonly appear = output();

  readonly disappear = output();

  modalRef!: NgbModalRef;

  isConfirmationOpen = false;

  modalIdentifier = `modal-${uuid()}`;

  get modalWindowRef() {
    return this.document.querySelector(
      `ngb-modal-window.${this.modalIdentifier}`
    );
  }

  get isFormDirty(): boolean {
    return Boolean(this.modalWindowRef?.querySelector(".ng-dirty"));
  }

  constructor() {
    effect(() => {
      this.toggle(this.visible());
    });
  }

  ngOnInit(): void {
    this.modalRefService.register(this);
  }

  dismiss(mode: ModalDismissMode) {
    switch (mode) {
      case "hard":
        this.visible.set(false);
        break;
      case "soft":
        this.close();
        break;
      default:
        break;
    }
  }

  protected toggle(value: boolean) {
    this.visible.set(value);

    if (!value) {
      this.modalRef?.dismiss();
      this.disappear.emit();
      return;
    }

    setTimeout(() => this.listen(), 0);
    this.modalRef = this.modal.open(this.modalContent(), {
      size: "md",
      centered: false,
      keyboard: false,
      scrollable: true,
      beforeDismiss: () => {
        if (!this.visible()) return true;

        this.close();
        return !this.visible();
      },
      ...this.options(),
      windowClass: `${this.options().windowClass || ""} ${
        this.modalIdentifier
      }`,
    });

    this.appear.emit();
  }

  ngOnDestroy(): void {
    this.modalRefService.unregister(this);
    this.toggle(false);
  }

  close() {
    if (this.busy()) return;

    if (this.isFormDirty && !this.suppressUnsavedChangesWarning()) {
      if (this.isConfirmationOpen) return;

      this.isConfirmationOpen = true;
      this.confirmationService
        .warn(
          "AbpUi::AreYouSureYouWantToCancelEditingWarningMessage",
          "AbpUi::AreYouSure",
          {
            dismissible: false,
          }
        )
        .subscribe((status: Confirmation.Status) => {
          this.isConfirmationOpen = false;
          if (status === Confirmation.Status.confirm) {
            this.visible.set(false);
          }
        });
    } else {
      this.visible.set(false);
    }
  }

  listen() {
    if (this.modalWindowRef) {
      fromEvent<KeyboardEvent>(this.modalWindowRef, "keyup")
        .pipe(
          takeUntilDestroyed(this.destroyRef),
          debounceTime(150),
          filter(
            (key: KeyboardEvent) =>
              key && key.key === "Escape" && this.options().keyboard
          )
        )
        .subscribe(() => this.close());
    }

    fromEvent(window, "beforeunload")
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((event) => {
        if (this.isFormDirty && !this.suppressUnsavedChangesWarning()) {
          event.preventDefault();
        }
      });

    this.init.emit();
  }
}
```

This concept differs slightly from the others mentioned above since these components are introduced within a library called `theme-shared`, which you can explore [here](https://github.com/abpframework/abp/tree/dev/npm/ng-packs/packages/theme-shared).

Using **shared libraries** for such common components is one of the most effective ways to make your app modular and maintainable. By grouping frequently used elements into a dedicated library, you create a single source of truth for your UI and logic.

However, over-creating or prematurely abstracting small pieces of logic into separate libraries can lead to unnecessary complexity and dependency management overhead. When every feature has its own “mini-library,” updates and debugging become scattered and difficult to coordinate.

The key is to extract shared functionality only when it is proven to be reused across multiple contexts. Start small, let patterns emerge naturally, and then move them into a shared library when the benefits of reusability outweigh the maintenance cost.

---

## ⚙️ Best Practices and Common Pitfalls

### ✅ Best Practices

1. **Start with real reuse:** Extract components only after the pattern appears in multiple places.
2. **Keep them focused:** One clear responsibility per component—avoid “do-it-all” designs.
3. **Use standalone components:** Simplify imports and improve independence.
4. **Promote through libraries:** Move proven, stable components into shared libraries for wider use.

### ⚠️ Common Mistakes

1. **Premature abstraction:** Don't create components before actual reuse.
2. **Too many input/output bindings:** Overly generic components are hard to configure and maintain.
3. **Neglecting performance:** Too many micro-components can hurt performance.
4. **Ignoring accessibility and semantics:** Reusable does not mean usable—always consider ARIA roles and HTML structure.

---

## 📚 Further Reading and References

As this article has mentioned some concepts and best practices, you can explore these resources for more details:

- [Angular Components Guide](https://angular.dev/guide/components)
- [Standalone Migration Guides](https://angular.dev/reference/migrations/standalone), [ABP Angular Standalone Applications](https://abp.io/community/articles/abp-now-supports-angular-standalone-applications-zzi2rr2z#gsc.tab=0)
- [Smart vs. Dumb Components](https://blog.angular-university.io/angular-2-smart-components-vs-presentation-components-whats-the-difference-when-to-use-each-and-why/)
- [Angular Libraries Overview](https://angular.dev/tools/libraries)

You can also check these open-source libraries for a better understanding of reusability and modularity:

- [Angular Components on GitHub](https://github.com/angular/components)
- [ABP NPM Libraries](https://github.com/abpframework/abp/tree/dev/npm/ng-packs/packages)

---

## 🏁 Conclusion

Reusability is one of the strongest architectural foundations for scalable Angular applications. By combining **standalone components**, **signals**, **encapsulated logic**, and **shared libraries**, you can create a modular system that grows gracefully over time.

The goal is not just to make components reusable. It is to make them meaningful, maintainable, and consistent across your app. Build only what truly adds value, reuse intentionally, and let Angular's evolving ecosystem handle the rest.
