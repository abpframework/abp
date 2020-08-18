import { Import } from './import';
import { Method } from './method';
import { Omissible } from './util';

export class Service {
  apiName: string;
  imports: Import[] = [];
  methods: Method[] = [];
  name: string;
  namespace: string;

  constructor(options: ServiceOptions) {
    Object.assign(this, options);
  }
}

export type ServiceOptions = Omissible<Service, 'imports' | 'methods'>;
