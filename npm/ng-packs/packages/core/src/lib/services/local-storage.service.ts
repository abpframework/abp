import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AbpLocalStorageService implements Storage {
  constructor() {}
  [name: string]: any;
  get length(): number {
    return localStorage.length;
  }

  clear(): void {
    localStorage.clear();
  }
  getItem(key: string): string {
    return localStorage.getItem(key);
  }
  key(index: number): string {
    return localStorage.key(index);
  }
  removeItem(key: string): void {
    localStorage.removeItem(key);
  }
  setItem(key: string, value: string): void {
    localStorage.setItem(key, value);
  }
}
