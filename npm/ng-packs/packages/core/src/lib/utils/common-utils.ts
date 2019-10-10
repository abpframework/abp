export function noop() {
  // tslint:disable-next-line: only-arrow-functions
  const fn = function() {};
  return fn;
}
