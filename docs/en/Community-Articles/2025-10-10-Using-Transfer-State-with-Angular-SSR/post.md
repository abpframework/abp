# From Server to Browser — the Elegant Way: Angular TransferState Explained

## Introduction

When building Angular applications with Server‑Side Rendering (SSR), a common performance pitfall is duplicated data fetching: the server loads data to render HTML, then the browser bootstraps Angular and fetches the same data again. That’s wasteful, increases Time‑to‑Interactive, and can hammer your APIs.

Angular’s built‑in **TransferState** lets you transfer the data fetched on the server to the browser during hydration so the client can reuse it instead of calling the API again. It’s simple, safe for serializable data, and makes SSR feel instant for users.

This article explains what TransferState is, and how to implement it in your Angular SSR app.

---

## What Is TransferState?

TransferState is a key–value store that exists for a single SSR render. On the server, you put serializable data into the store. Angular serializes it into the HTML as a small script tag. When the browser hydrates, Angular reads that payload back and makes it available to your app. You can then consume it and skip duplicate HTTP calls.

Key points:

- Works only across the SSR → browser hydration boundary (not a general cache).
- Data is cleaned up after bootstrapping (no stale data).
- Stores JSON‑serializable data only (if you need to use Date/Functions/Map; serialize it).
- Data is set on the server and read on the client.

---

## When Should You Use It?

- Data fetched during SSR that is also be needed on the client.
- Data that doesn’t change between server render and immediate client hydration.
- Expensive or slow API endpoints where a second request is visibly costly.

Avoid using it for:

- Highly dynamic data that changes frequently.
- Sensitive data (never put secrets/tokens in TransferState).
- Large payloads (keep the serialized state small to avoid bloating HTML).

---

## Prerequisites

- An Angular app with SSR enabled (Angular ≥16: `ng add @angular/ssr`).
- `HttpClient` configured. The examples below show both manual TransferState use and the build in solutions.

---

## Option A — Using TransferState Manually

This approach gives you full control over what to cache and when. It's straightforward and works in both module‑based and standalone‑based apps.

Service example that fetches books and uses TransferState:

```ts
// books.service.ts
import {
    Injectable,
    PLATFORM_ID,
    makeStateKey,
    TransferState,
    inject,
} from '@angular/core';
import { isPlatformServer } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { tap } from 'rxjs/operators';

export interface Book {
    id: number;
    name: string;
    price: number;
}

@Injectable({ providedIn: 'root' })
export class BooksService {
    BOOKS_KEY = makeStateKey<Book[]>('books:list');
    readonly httpClient = inject(HttpClient);
    readonly transferState = inject(TransferState);
    readonly platformId = inject(PLATFORM_ID);

    getBooks(): Observable<Book[]> {
        // If browser and we have the data that already fetched on the server, use it and remove from TransferState
        if (this.transferState.hasKey(this.BOOKS_KEY)) {
            const cached = this.transferState.get<Book[]>(this.BOOKS_KEY, []);
            this.transferState.remove(this.BOOKS_KEY); // remove to avoid stale reads
            return of(cached);
        }

        // Otherwise fetch data. If running on the server, write into TransferState
        return this.httpClient.get<Book[]>('/api/books').pipe(
            tap(list => {
                if (isPlatformServer(this.platformId)) {
                    this.transferState.set(this.BOOKS_KEY, list);
                }
            })
        );
    }
}

```

Use it in a component:

```ts
// books.component.ts
import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BooksService, Book } from './books.service';

@Component({
    selector: 'app-books',
    imports: [CommonModule],
    template: `
    <h1>Books</h1>
    <ul>
      @for (book of books; track book.id) {
        <li>{{ book.name }} — {{ book.price | currency }}</li>
      }
    </ul>
  `,
})
export class BooksComponent implements OnInit {
    private booksService = inject(BooksService);
    books: Book[] = [];

    ngOnInit() {
        this.booksService.getBooks().subscribe(data => (this.books = data));
    }
}

```

Route resolver variant (keeps templates simple and aligns with SSR prefetching):

```ts
// src/app/routes.ts

export const routes: Routes = [
  {
    path: 'books',
    component: BooksComponent,
    resolve: {
      books: () => inject(BooksService).getBooks(),
    },
  },
];
```

Then read `books` from the `ActivatedRoute` data in your component.

---

## Option B — Using HttpInterceptor to Automate TransferState

Like Option A, but less boilerplate. This approach uses an **HttpInterceptor** to automatically cache HTTP GET (also POST/PUT request but not recommended) responses in TransferState. You can determine which requests to cache based on URL patterns.

Example interceptor that caches GET requests:

```ts
import { inject, makeStateKey, PLATFORM_ID, TransferState } from '@angular/core';
import {
    HttpEvent,
    HttpHandlerFn,
    HttpInterceptorFn,
    HttpRequest,
    HttpResponse,
} from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { isPlatformBrowser, isPlatformServer } from '@angular/common';
import { tap } from 'rxjs/operators';

export const transferStateInterceptor: HttpInterceptorFn = (
    req: HttpRequest<any>,
    next: HttpHandlerFn,
): Observable<HttpEvent<any>> => {
    const transferState = inject(TransferState);
    const platformId = inject(PLATFORM_ID);

    // Only cache GET requests. You can customize this to match specific URLs if needed.
    if (req.method !== 'GET') {
        return next(req);
    }

    // Create a unique key for this request
    const stateKey = makeStateKey<HttpResponse<any>>(req.urlWithParams);

    // If browser, check if we have the response in TransferState
    if (isPlatformBrowser(platformId)) {
        const storedResponse = transferState.get<HttpResponse<any>>(stateKey, null);
        if (storedResponse) {
            transferState.remove(stateKey); // remove to avoid stale reads
            return of(new HttpResponse<any>({ body: storedResponse, status: 200 }));
        }
    }

    return next(req).pipe(
        tap(event => {
            // If server, store the response in TransferState
            if (isPlatformServer(platformId) && event instanceof HttpResponse) {
                transferState.set(stateKey, event.body);
            }
        }),
    );
};

```

Add the interceptor to your app module or bootstrap function:

````ts
        provideHttpClient(withFetch(), withInterceptors([transferStateInterceptor]))
````


---

## Option C — Using Angular's Built-in HTTP Transfer Cache

This is the simplest option if you want to HTTP requests that without custom logic.

Angular docs: https://angular.dev/api/platform-browser/withHttpTransferCacheOptions


Usage examples:

```ts
   // Only cache GET requests that have no headers
   provideClientHydration(withHttpTransferCacheOptions({}))

    // Also cache POST requests (not recommended for most cases)
    provideClientHydration(withHttpTransferCacheOptions({
        includePostRequests: true
    }))

    // Cache requests that have auth headers (e.g., JWT tokens)
    provideClientHydration(withHttpTransferCacheOptions({
        includeRequestsWithAuthHeaders: true
    }))
```

To see all options, check the Angular docs: https://angular.dev/api/common/http/HttpTransferCacheOptions

## Best Practices and Pitfalls

- Keep payloads small: only put what’s needed for initial paint.
- Serialize explicitly if needed: for Dates or complex types, convert to strings and reconstruct on the client.
- Don’t transfer secrets: never place tokens or sensitive user data in TransferState.
- Per‑request isolation: state is scoped to a single SSR request; it is not a global cache.

---

## Debugging Tips

- Log on server vs browser: use `isPlatformServer` and `isPlatformBrowser` checks to confirm where code runs.
- DevTools inspection: view the page source after SSR; you’ll see a small script tag that embeds the transfer state.
- Count requests: put a console log in your service to verify the second HTTP call is gone on the client.

---

## Measurable Impact

On content‑heavy pages, TransferState typically removes 1–3 duplicate API calls during hydration, shaving 100–500 ms from the critical path on average networks. It’s a low‑effort, high‑impact win for SSR apps.

---

## Conclusion

If you already have SSR, enabling TransferState is one of the easiest ways to make hydration feel instant. You can use it built‑in HTTP caching or manually control what to cache. Either way, it eliminates redundant data fetching, speeds up Time‑to‑Interactive, and improves user experience with minimal effort.
