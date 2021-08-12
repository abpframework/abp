export function noop() {
  // tslint:disable-next-line: only-arrow-functions
  const fn = function() {};
  return fn;
}

export function isUndefinedOrEmptyString(value: unknown): boolean {
  return value === undefined || value === '';
}

export function isNullOrUndefined(obj) {
  return obj === null || obj === undefined;
}

export function exists(obj) {
  return !isNullOrUndefined(obj);
}

export function isObject(obj) {
  return obj instanceof Object;
}

export function isArray(obj) {
  return Array.isArray(obj);
}

export function isObjectAndNotArray(obj) {
  return isObject(obj) && !isArray(obj);
}

export function isNode(obj) {
  return obj instanceof Node;
}

export function isObjectAndNotArrayNotNode(obj) {
  return isObjectAndNotArray(obj) && !isNode(obj);
}
