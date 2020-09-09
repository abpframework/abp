import { eImportKeyword } from '../enums';
import { Omissible } from './util';

export class Import {
  alias?: string;
  keyword = eImportKeyword.Default;
  path: string;
  refs: string[] = [];
  specifiers: string[] = [];

  constructor(options: ImportOptions) {
    Object.assign(this, options);
  }
}

export type ImportOptions = Omissible<Import, 'keyword' | 'refs' | 'specifiers'>;
