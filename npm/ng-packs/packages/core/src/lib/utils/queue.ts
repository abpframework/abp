export interface QueueManager {
  add(fn: () => void): void;
  init(interval: number, stackSize: number): void;
}

export class DefaultQueueManager implements QueueManager {
  private queue: Array<() => void> = [];
  private isRunning = false;
  private stack = 0;
  private interval = 0;
  private stackSize = 100;

  public init(interval: number, stackSize: number) {
    this.interval = interval;
    this.stackSize = stackSize;
  }

  public add(fn: () => void) {
    this.queue.push(fn);
    this.run();
  }

  private run() {
    if (this.isRunning) return;
    this.stack++;

    this.isRunning = true;

    const fn = this.queue.shift();

    if (!fn) {
      this.isRunning = false;
      return;
    }

    fn();
    if (this.stack > this.stackSize) {
      setTimeout(() => {
        this.isRunning = false;
        this.run();
        this.stack = 0;
      }, this.interval);
    } else {
      this.isRunning = false;
      this.run();
    }
  }
}
