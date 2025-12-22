import { Routes } from '@angular/router';
import { authGuard, permissionGuard } from '@abp/ng.core';

export const BOOK_ROUTES: Routes = [
    {
        path: '',
        loadComponent: () => import('./book.component').then(m => m.BookComponent),
        canActivate: [authGuard, permissionGuard],
        data: {
            requiredPolicy: 'MyProjectName.Books',
        },
    },
];
