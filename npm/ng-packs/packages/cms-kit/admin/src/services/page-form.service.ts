import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { ToasterService } from '@abp/ng.theme.shared';
import {
  PageAdminService,
  CreatePageInputDto,
  UpdatePageInputDto,
  PageDto,
  PageStatus,
} from '@abp/ng.cms-kit/proxy';

@Injectable({
  providedIn: 'root',
})
export class PageFormService {
  private pageService = inject(PageAdminService);
  private toasterService = inject(ToasterService);
  private router = inject(Router);

  create(form: FormGroup): Observable<PageDto> {
    if (!form.valid) {
      throw new Error('Form is invalid');
    }

    return this.pageService.create(form.value as CreatePageInputDto).pipe(
      tap(() => {
        this.toasterService.success('AbpUi::SavedSuccessfully');
        this.router.navigate(['/cms/pages']);
      }),
    );
  }

  createAsDraft(form: FormGroup): Observable<PageDto> {
    if (!form.valid) {
      throw new Error('Form is invalid');
    }

    const formValue = { ...form.value, status: PageStatus.Draft } as CreatePageInputDto;
    return this.pageService.create(formValue).pipe(
      tap(() => {
        this.toasterService.success('AbpUi::SavedSuccessfully');
        this.router.navigate(['/cms/pages']);
      }),
    );
  }

  publish(form: FormGroup): Observable<PageDto> {
    if (!form.valid) {
      throw new Error('Form is invalid');
    }

    const formValue = { ...form.value, status: PageStatus.Publish } as CreatePageInputDto;
    return this.pageService.create(formValue).pipe(
      tap(() => {
        this.toasterService.success('AbpUi::SavedSuccessfully');
        this.router.navigate(['/cms/pages']);
      }),
    );
  }

  update(pageId: string, form: FormGroup, page: PageDto): Observable<PageDto> {
    if (!form.valid || !page) {
      throw new Error('Form is invalid or page is missing');
    }

    const formValue = {
      ...page,
      ...form.value,
    } as UpdatePageInputDto;

    return this.pageService.update(pageId, formValue).pipe(
      tap(() => {
        this.toasterService.success('AbpUi::SavedSuccessfully');
        this.router.navigate(['/cms/pages']);
      }),
    );
  }

  updateAsDraft(pageId: string, form: FormGroup, page: PageDto): Observable<PageDto> {
    if (!form.valid || !page) {
      throw new Error('Form is invalid or page is missing');
    }

    const formValue = {
      ...page,
      ...form.value,
      status: PageStatus.Draft,
    } as UpdatePageInputDto;

    return this.pageService.update(pageId, formValue).pipe(
      tap(() => {
        this.toasterService.success('AbpUi::SavedSuccessfully');
        this.router.navigate(['/cms/pages']);
      }),
    );
  }

  updateAndPublish(pageId: string, form: FormGroup, page: PageDto): Observable<PageDto> {
    if (!form.valid || !page) {
      throw new Error('Form is invalid or page is missing');
    }

    const formValue = {
      ...page,
      ...form.value,
      status: PageStatus.Publish,
    } as UpdatePageInputDto;

    return this.pageService.update(pageId, formValue).pipe(
      tap(() => {
        this.toasterService.success('AbpUi::SavedSuccessfully');
        this.router.navigate(['/cms/pages']);
      }),
    );
  }
}
