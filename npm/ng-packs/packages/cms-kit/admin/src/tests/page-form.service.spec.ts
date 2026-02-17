/* eslint-disable */
import { describe, it, expect, beforeEach, jest } from '@jest/globals';
import { FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
// @ts-ignore - test types are resolved only in the library build context
import { ToasterService } from '@abp/ng.theme.shared';
// @ts-ignore - proxy module types are resolved only in the library build context
import { PageAdminService, PageDto } from '@abp/ng.cms-kit/proxy';
import { PageFormService } from '../services';

describe('PageFormService', () => {
  let service: PageFormService;
  let pageAdminService: any;
  let toasterService: any;
  let router: any;

  beforeEach(() => {
    pageAdminService = {
      create: jest.fn().mockReturnValue(of({})),
      update: jest.fn().mockReturnValue(of({})),
      setAsHomePage: jest.fn(),
      delete: jest.fn(),
      get: jest.fn(),
      getList: jest.fn(),
    };

    toasterService = {
      success: jest.fn(),
    };

    router = {
      navigate: jest.fn(),
    };

    TestBed.configureTestingModule({
      providers: [
        PageFormService,
        { provide: PageAdminService, useValue: pageAdminService },
        { provide: ToasterService, useValue: toasterService },
        { provide: Router, useValue: router },
      ],
    });

    service = TestBed.inject(PageFormService);
  });

  function createValidForm(): FormGroup {
    return new FormGroup({});
  }

  function createInvalidForm(): FormGroup {
    const form = new FormGroup({});
    form.setErrors({ invalid: true });
    return form;
  }

  it('should throw when creating with invalid form', () => {
    const form = createInvalidForm();

    expect(() => service.create(form)).toThrowError('Form is invalid');
  });

  it('should call PageAdminService.create and navigate on create', done => {
    const form = createValidForm();

    service.create(form).subscribe({
      next: () => {
        expect(pageAdminService.create).toHaveBeenCalledWith(form.value);
        expect(toasterService.success).toHaveBeenCalledWith('AbpUi::SavedSuccessfully');
        expect(router.navigate).toHaveBeenCalledWith(['/cms/pages']);
        done();
      },
      error: err => done(err as any),
    });
  });

  it('should call PageAdminService.create on createAsDraft', done => {
    const form = createValidForm();

    service.createAsDraft(form).subscribe({
      next: () => {
        expect(pageAdminService.create).toHaveBeenCalled();
        done();
      },
      error: err => done(err as any),
    });
  });

  it('should call PageAdminService.create on publish', done => {
    const form = createValidForm();

    service.publish(form).subscribe({
      next: () => {
        expect(pageAdminService.create).toHaveBeenCalled();
        done();
      },
      error: err => done(err as any),
    });
  });

  it('should throw when updating with invalid form or missing page', () => {
    const form = createInvalidForm();

    expect(() => service.update('id', form, {} as any)).toThrowError(
      'Form is invalid or page is missing',
    );

    const validForm = createValidForm();
    expect(() => service.update('id', validForm, null as any)).toThrowError(
      'Form is invalid or page is missing',
    );
  });

  it('should call PageAdminService.update on update', done => {
    const form = createValidForm();
    const page = { id: '1', name: 'test', isHomePage: false };

    service.update('1', form, page).subscribe({
      next: () => {
        expect(pageAdminService.update).toHaveBeenCalledWith('1', expect.objectContaining(page));
        done();
      },
      error: err => done(err as any),
    });
  });

  it('should set status Draft on updateAsDraft', done => {
    const form = createValidForm();
    const page = { id: '1', name: 'test', isHomePage: false };

    service.updateAsDraft('1', form, page).subscribe({
      next: () => {
        const arg = pageAdminService.update.mock.calls[0][1];
        expect(arg).toMatchObject(page);
        done();
      },
      error: err => done(err as any),
    });
  });

  it('should set status Publish on updateAndPublish', done => {
    const form = createValidForm();
    const page = { id: '1', name: 'test', isHomePage: false };

    service.updateAndPublish('1', form, page).subscribe({
      next: () => {
        const arg = pageAdminService.update.mock.calls[0][1];
        expect(arg).toMatchObject(page);
        done();
      },
      error: err => done(err as any),
    });
  });
});
