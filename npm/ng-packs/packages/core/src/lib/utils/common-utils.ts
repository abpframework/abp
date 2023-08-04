export function noop() {
  const fn = function () {};
  return fn;
}

export function isUndefinedOrEmptyString(value: unknown): boolean {
  return value === undefined || value === '';
}

export function isNullOrUndefined<T>(obj: T) {
  return obj === null || obj === undefined;
}

export function isNullOrEmpty<T>(obj: T): boolean {
  return obj === null || obj === undefined || obj === '';
}

export function exists<T>(obj: T): obj is T {
  return !isNullOrUndefined(obj);
}

export function isObject<T>(obj: T): boolean {
  return obj instanceof Object;
}

export function isArray<T>(obj: T): boolean {
  return Array.isArray(obj);
}

export function isObjectAndNotArray<T>(obj: T): boolean {
  return isObject(obj) && !isArray(obj);
}

export function isNode<T>(obj: T): boolean {
  return obj instanceof Node;
}

export function isObjectAndNotArrayNotNode<T>(obj: T): boolean {
  return isObjectAndNotArray(obj) && !isNode(obj);
}

export function checkHasProp<T>(object: T, key: string | keyof T): key is keyof T {
  return Object.prototype.hasOwnProperty.call(object, key);
}
