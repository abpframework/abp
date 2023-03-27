import { Injectable } from '@angular/core';
import { AbpStorageService } from './storage.service';
@Injectable({ providedIn: 'root' })
export class AbpCookieStorageService implements AbpStorageService {
    [name: string]: any;
    length: number;
    clear(): void {
        throw new Error("clear not implemented.");
    }
    getItem(key: string): string {
        throw new Error("getItem not implemented.");
    }
    key(index: number): string {
        throw new Error("key not implemented.");
    }
    removeItem(key: string): void {
        throw new Error("removeItem not implemented.");
    }
    setItem(key: string, value: string): void {
        throw new Error("setItem not implemented.");
    }
}