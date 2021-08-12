import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { EntityActionsFactory } from '../lib/models/entity-actions';
import { EntityPropsFactory } from '../lib/models/entity-props';
import { CreateFormPropsFactory, EditFormPropsFactory } from '../lib/models/form-props';
import { ToolbarActionsFactory } from '../lib/models/toolbar-actions';
import { ExtensionsService } from '../lib/services/extensions.service';

describe('ExtensionsService', () => {
  let service: ExtensionsService;
  let spectator: SpectatorService<ExtensionsService>;

  const createService = createServiceFactory({
    service: ExtensionsService,
  });

  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
  });

  describe('#entityActions', () => {
    it('should be an instance of EntityActionsFactory class', () => {
      expect(service.entityActions).toBeInstanceOf(EntityActionsFactory);
    });
  });

  describe('#toolbarActions', () => {
    it('should be an instance of ToolbarActionsFactory class', () => {
      expect(service.toolbarActions).toBeInstanceOf(ToolbarActionsFactory);
    });
  });

  describe('#entityProps', () => {
    it('should be an instance of EntityPropsFactory class', () => {
      expect(service.entityProps).toBeInstanceOf(EntityPropsFactory);
    });
  });

  describe('#createFormProps', () => {
    it('should be an instance of CreateFormPropsFactory class', () => {
      expect(service.createFormProps).toBeInstanceOf(CreateFormPropsFactory);
    });
  });

  describe('#editFormProps', () => {
    it('should be an instance of EditFormPropsFactory class', () => {
      expect(service.editFormProps).toBeInstanceOf(EditFormPropsFactory);
    });
  });
});
