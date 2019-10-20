import { Store } from '@ngxs/store';
export declare class ConfigService {
  private store;
  constructor(store: Store);
  getAll(): import('../models').Config.State;
  getOne(key: string): any;
  getDeep(keys: string[] | string): any;
  getSetting(key: string): string;
}
