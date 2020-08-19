export function deepMerge(target, source) {
  if (isObjectAndNotArray(target) && isObjectAndNotArray(source)) {
    return deepMergeRecursively(target, source);
  } else if (isNullOrUndefined(target) && isNullOrUndefined(source)) {
    return {};
  } else {
    return isNullOrUndefined(source) ? target : source;
  }
}

function deepMergeRecursively(target, source) {
  const shouldNotRecurse =
    isNullOrUndefined(target) ||
    isNullOrUndefined(source) || // at least one not defined
    isArray(target) ||
    isArray(source) || // at least one array
    !isObject(target) ||
    !isObject(source); // at least one not an object;

  /**
   * if we will not recurse any further,
   * we will prioritize source if it is a defined value.
   */
  if (shouldNotRecurse) {
    return !isNullOrUndefined(source) ? source : target;
  }

  const keysOfTarget = Object.keys(target);
  const keysOfSource = Object.keys(source);
  const uniqueKeys = new Set(keysOfTarget.concat(keysOfSource));
  return [...uniqueKeys].reduce((retVal, key) => {
    retVal[key] = deepMergeRecursively(target[key], source[key]);
    return retVal;
  }, {});
}

function isNullOrUndefined(obj) {
  return obj === null || obj === undefined;
}

function isObject(obj) {
  return obj instanceof Object;
}

function isArray(obj) {
  return Array.isArray(obj);
}

function isObjectAndNotArray(obj) {
  return isObject(obj) && !isArray(obj);
}
