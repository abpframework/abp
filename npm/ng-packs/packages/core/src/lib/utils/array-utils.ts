export function pushValueTo<T>(array: T[]) {
  return (element: T) => {
    array.push(element);
    return array;
  };
}
