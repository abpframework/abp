export type PartialWithOptions<T, OptionalKeys extends keyof T> = Partial<Pick<T, OptionalKeys>>;
export type FilteredWithOptions<T, OptionalKeys extends keyof T> = Omit<T, OptionalKeys>;
