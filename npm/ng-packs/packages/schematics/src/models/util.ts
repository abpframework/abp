// Omissible (given keys will become optional)
export type Omissible<T, K extends keyof T> = Partial<Pick<T, K>> & Omit<T, K>;
