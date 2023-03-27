export abstract class AbpStorageService implements Storage {
  abstract get length(): number;
  abstract clear(): void;
  abstract getItem(key: string): string | null;
  abstract key(index: number): string | null;
  abstract removeItem(key: string): void;
  abstract setItem(key: string, value: string): void;
}