import type { AuditedEntityDto, PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';

export interface BookDto extends AuditedEntityDto<string> {
    name: string;
    bookType: string;
    publishDate: string;
    price: number;
    author: string;
    resourcePermissions: Record<string, boolean>;
}

export interface CreateUpdateBookDto {
    name: string;
    bookType: string;
    publishDate: string;
    price: number;
    author: string;
}

export type GetBookListInput = PagedAndSortedResultRequestDto;

export type BookListResponse = PagedResultDto<BookDto>;
