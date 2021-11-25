export {};

declare global {
  interface Date {
    toLocalISOString?: () => string;
  }
}

Date.prototype.toLocalISOString = function (this: Date): string {
  const timezoneOffset = this.getTimezoneOffset();

  return new Date(this.getTime() - timezoneOffset * 60000).toISOString();
};
