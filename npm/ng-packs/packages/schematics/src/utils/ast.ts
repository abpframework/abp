import * as ts from 'typescript';
import { findNodes } from './angular/ast-utils';

export function findEnvironmentExpression(source: ts.SourceFile) {
  const expressions = findNodes(source, ts.isObjectLiteralExpression);
  return expressions.find(expr => expr.getText().includes('production'));
}

export function getAssignedPropertyFromObjectliteral(
  expression: ts.ObjectLiteralExpression,
  variableSelector: string[],
) {
  const expressions = findNodes(expression, isBooleanStringOrNumberLiteral);

  const literal = expressions.find(node =>
    Boolean(
      variableSelector.reduceRight(
        (acc: ts.PropertyAssignment, key) =>
          acc?.name?.getText() === key ? acc.parent.parent : undefined,
        node.parent,
      ),
    ),
  );

  return literal ? literal.getText() : undefined;
}

export function isBooleanStringOrNumberLiteral(
  node: ts.Node,
): node is ts.StringLiteral | ts.NumericLiteral | ts.BooleanLiteral {
  return (
    ts.isStringLiteral(node) ||
    ts.isNumericLiteral(node) ||
    node.kind === ts.SyntaxKind.TrueKeyword ||
    node.kind === ts.SyntaxKind.FalseKeyword
  );
}
