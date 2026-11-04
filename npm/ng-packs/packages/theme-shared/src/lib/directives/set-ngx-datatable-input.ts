import { ɵSIGNAL } from '@angular/core';

/**
 * Writes an ngx-datatable signal input without replacing the signal function.
 * Template bindings applied after construction still override these defaults.
 */
export function setNgxDatatableInput(inputSignal: object, value: unknown): void {
  const node = (inputSignal as { [ɵSIGNAL]: NgxDatatableInputNode })[ɵSIGNAL];
  const nextValue = node.transformFn ? node.transformFn(value) : value;
  node.applyValueToInputSignal(node, nextValue);
}

interface NgxDatatableInputNode {
  transformFn?: (value: unknown) => unknown;
  applyValueToInputSignal(node: NgxDatatableInputNode, value: unknown): void;
}
