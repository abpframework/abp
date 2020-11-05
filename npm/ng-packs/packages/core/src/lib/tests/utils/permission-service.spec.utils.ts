import { PermissionService } from '../../services';
import { Subject } from 'rxjs';

export const mockPermissionService = (args = {} as Partial<PermissionService>) => {
  const permissionService = { getGrantedPolicy$: new Subject(), getGrantedPolicy: arg => true };
  return Object.assign({}, permissionService, args);
};
