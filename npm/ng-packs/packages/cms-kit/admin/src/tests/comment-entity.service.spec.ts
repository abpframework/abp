/* eslint-disable */
import { describe, it, expect, beforeEach, jest } from '@jest/globals';
import { Router } from '@angular/router';
import { TestBed } from '@angular/core/testing';
import { of, Subject } from 'rxjs';
// @ts-ignore - test types are resolved only in the library build context
import { ConfigStateService, ListService } from '@abp/ng.core';
// @ts-ignore - test types are resolved only in the library build context
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
// @ts-ignore - proxy module types are resolved only in the library build context
import { CommentAdminService, CommentGetListInput } from '@abp/ng.cms-kit/proxy';
import { CommentEntityService } from '../services';

describe('CommentEntityService', () => {
  let service: CommentEntityService;
  let commentAdminService: any;
  let toasterService: any;
  let confirmationService: any;
  let configStateService: any;
  let router: any;

  beforeEach(() => {
    commentAdminService = {
      updateApprovalStatus: jest.fn().mockReturnValue(of(void 0)),
      delete: jest.fn().mockReturnValue(of(void 0)),
    };

    toasterService = {
      success: jest.fn(),
    };

    confirmationService = {
      warn: jest.fn(),
    };

    configStateService = {
      getSetting: jest.fn(),
    };

    router = {
      url: '/cms/comments/123',
    };

    TestBed.configureTestingModule({
      providers: [
        CommentEntityService,
        { provide: CommentAdminService, useValue: commentAdminService },
        { provide: ToasterService, useValue: toasterService },
        { provide: ConfirmationService, useValue: confirmationService },
        { provide: ConfigStateService, useValue: configStateService },
        { provide: Router, useValue: router },
      ],
    });

    service = TestBed.inject(CommentEntityService);
  });

  it('should return requireApprovement based on setting', () => {
    configStateService.getSetting.mockReturnValue('true');
    expect(service.requireApprovement).toBe(true);

    configStateService.getSetting.mockReturnValue('false');
    expect(service.requireApprovement).toBe(false);
  });

  it('should detect comment reply from router url', () => {
    expect(service.isCommentReply('123')).toBe(true);
    expect(service.isCommentReply('456')).toBe(false);
    expect(service.isCommentReply(undefined)).toBe(false);
  });

  it('should update approval status and refresh list', () => {
    const list = {
      get: jest.fn(),
    } as unknown as ListService<any>;

    service.updateApprovalStatus('1', true, list);

    expect(commentAdminService.updateApprovalStatus).toHaveBeenCalledWith('1', {
      isApproved: true,
    });
    expect(list.get).toHaveBeenCalled();
    expect(toasterService.success).toHaveBeenCalledWith('CmsKit::ApprovedSuccessfully');
  });

  it('should show confirmation and delete comment when confirmed', () => {
    const subject = new Subject<Confirmation.Status>();
    (confirmationService.warn as jest.Mock).mockReturnValue(subject.asObservable());

    const list = {
      get: jest.fn(),
    } as unknown as ListService<CommentGetListInput>;

    service.delete('1', list);

    subject.next(Confirmation.Status.confirm);
    subject.complete();

    expect(commentAdminService.delete).toHaveBeenCalledWith('1');
    expect(list.get).toHaveBeenCalled();
  });
});
