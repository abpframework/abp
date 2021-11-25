// Omissible (given keys will become optional)
export type Omissible<T, K extends keyof T> = Partial<Pick<T, K>> & Omit<T, K>;

// ExcludeKeys (keys will be excluded based on their type)
type ExcludeKeys<Type, Excluded> = Exclude<
  {
    [Key in keyof Type]: Type[Key] extends Excluded ? never : Key;
  }[keyof Type],
  never
>;

// eslint-disable-next-line @typescript-eslint/ban-types
type ExcludeMethods<Type> = Pick<Type, ExcludeKeys<Type, Function>>;

// Options (methods will be omitted, given keys will become optional)
export type Options<T, K extends keyof ExcludeMethods<T>> = Omissible<ExcludeMethods<T>, K>;
