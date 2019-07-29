import { Observable } from 'rxjs';
export declare const takeUntilDestroy: (componentInstance: any, destroyMethodName?: string) => <T>(source: Observable<T>) => Observable<T>;
