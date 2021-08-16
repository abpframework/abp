import { Import } from '../models';

export function sortImports(imports: Import[]) {
  imports.sort((a, b) =>
    removeRelative(a) > removeRelative(b) ? 1 : a.keyword > b.keyword ? 1 : -1,
  );
}

export function removeRelative(importDef: Import) {
  return importDef.path.replace(/\.\.\//g, '');
}
