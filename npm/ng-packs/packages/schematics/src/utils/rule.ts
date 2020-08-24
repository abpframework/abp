import {
  apply,
  forEach,
  mergeWith,
  Rule,
  SchematicContext,
  Source,
  Tree,
} from '@angular-devkit/schematics';

export function applyWithOverwrite(source: Source, rules: Rule[]): Rule {
  return (tree: Tree, _context: SchematicContext) => {
    const rule = mergeWith(apply(source, [...rules, overwriteFileIfExists(tree)]));

    return rule(tree, _context);
  };
}

export function overwriteFileIfExists(tree: Tree): Rule {
  return forEach(fileEntry => {
    if (!tree.exists(fileEntry.path)) return fileEntry;

    tree.overwrite(fileEntry.path, fileEntry.content);
    return null;
  });
}
