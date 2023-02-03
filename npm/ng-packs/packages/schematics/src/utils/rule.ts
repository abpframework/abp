import {
  apply,
  callRule,
  forEach,
  MergeStrategy,
  mergeWith,
  Rule,
  SchematicContext,
  Source,
  Tree,
} from '@angular-devkit/schematics';
import { RuleReturn } from '../models/rule';

export function applyWithOverwrite(source: Source, rules: Rule[]): Rule {
  return (tree: Tree, _context: SchematicContext) => {
    const mapped = rules.map<Rule>(r => {
      return ((_tree: Tree, _ctx: SchematicContext) =>
        new Promise<RuleReturn>(res => res(r(_tree, _ctx)))) as Rule;
    });
    const rule = mergeWith(apply(source, [...mapped, overwriteFileIfExists(tree)]));
    return rule(tree, _context);
  };
}

export function mergeAndAllowDelete(host: Tree, rule: Rule) {
  return async (tree: Tree, context: SchematicContext) => {
    const nextTree = await callRule(rule, tree, context).toPromise();
    host.merge(nextTree, MergeStrategy.AllowDeleteConflict);
  };
}

export function overwriteFileIfExists(tree: Tree): Rule {
  return forEach(fileEntry => {
    if (!tree.exists(fileEntry.path)) return fileEntry;

    tree.overwrite(fileEntry.path, fileEntry.content);
    return null;
  });
}
