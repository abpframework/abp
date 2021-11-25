export function pushValueTo<T extends any>(array: T[]) {
  return (element: T) => {
    array.push(element);
    return array;
  };
}
