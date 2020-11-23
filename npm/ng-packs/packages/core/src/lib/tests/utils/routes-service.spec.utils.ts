import { RoutesService } from '../../services';
import { mockPermissionService } from './permission-service.spec.utils';
import { DummyInjector, mockActions } from './common.utils';

export const mockRoutesService = (injectorPayload = {} as { [key: string]: any }) => {
  const injector = new DummyInjector({
    PermissionService: mockPermissionService(),
    Actions: mockActions,
    ...injectorPayload,
  });
  return new RoutesService(injector);
};
