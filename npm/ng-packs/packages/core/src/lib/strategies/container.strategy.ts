import { ViewContainerRef } from '@angular/core';

export abstract class ContainerStrategy {
  constructor(public containerRef: ViewContainerRef) {}

  abstract getIndex(): number;

  prepare(): void {}
}

export class ClearContainerStrategy extends ContainerStrategy {
  getIndex(): number {
    return 0;
  }

  prepare() {
    this.containerRef.clear();
  }
}

export class InsertIntoContainerStrategy extends ContainerStrategy {
  constructor(containerRef: ViewContainerRef, private index: number) {
    super(containerRef);
  }

  getIndex() {
    return Math.min(Math.max(0, this.index), this.containerRef.length);
  }
}

export const CONTAINER_STRATEGY = {
  Clear(containerRef: ViewContainerRef) {
    return new ClearContainerStrategy(containerRef);
  },
  Append(containerRef: ViewContainerRef) {
    return new InsertIntoContainerStrategy(containerRef, containerRef.length);
  },
  Prepend(containerRef: ViewContainerRef) {
    return new InsertIntoContainerStrategy(containerRef, 0);
  },
  Insert(containerRef: ViewContainerRef, index: number) {
    return new InsertIntoContainerStrategy(containerRef, index);
  },
};
