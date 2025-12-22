import { Injectable, inject } from '@angular/core';
import { RestService, Rest } from '@abp/ng.core';
import type { BookDto, CreateUpdateBookDto, GetBookListInput, BookListResponse } from './models';

@Injectable({
    providedIn: 'root',
})
export class BookService {
    protected readonly rest = inject(RestService);
    protected readonly apiName = 'Default';
    protected readonly baseUrl = '/api/app/book';

    getList(input: GetBookListInput, config?: Partial<Rest.Config>) {
        return this.rest.request<GetBookListInput, BookListResponse>(
            {
                method: 'GET',
                url: this.baseUrl,
                params: {
                    sorting: input.sorting,
                    skipCount: input.skipCount,
                    maxResultCount: input.maxResultCount,
                },
            },
            { apiName: this.apiName, ...config }
        );
    }

    get(id: string, config?: Partial<Rest.Config>) {
        return this.rest.request<void, BookDto>(
            {
                method: 'GET',
                url: `${this.baseUrl}/${id}`,
            },
            { apiName: this.apiName, ...config }
        );
    }

    create(input: CreateUpdateBookDto, config?: Partial<Rest.Config>) {
        return this.rest.request<CreateUpdateBookDto, BookDto>(
            {
                method: 'POST',
                url: this.baseUrl,
                body: input,
            },
            { apiName: this.apiName, ...config }
        );
    }

    update(id: string, input: CreateUpdateBookDto, config?: Partial<Rest.Config>) {
        return this.rest.request<CreateUpdateBookDto, BookDto>(
            {
                method: 'PUT',
                url: `${this.baseUrl}/${id}`,
                body: input,
            },
            { apiName: this.apiName, ...config }
        );
    }

    delete(id: string, config?: Partial<Rest.Config>) {
        return this.rest.request<void, void>(
            {
                method: 'DELETE',
                url: `${this.baseUrl}/${id}`,
            },
            { apiName: this.apiName, ...config }
        );
    }
}
