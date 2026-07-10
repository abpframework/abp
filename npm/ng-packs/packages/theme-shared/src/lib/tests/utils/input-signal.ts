import { ɵSIGNAL as SIGNAL } from '@angular/core';

export const setInputSignal = <T>(inputSignal: () => T, value: T) => {
  const node = inputSignal[SIGNAL];
  node.applyValueToInputSignal(node, value);
};