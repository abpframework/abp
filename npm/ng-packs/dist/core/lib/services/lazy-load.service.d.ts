import { Observable, ReplaySubject } from 'rxjs';
export declare class LazyLoadService {
    loadedLibraries: {
        [url: string]: ReplaySubject<void>;
    };
    load(urlOrUrls: string | string[], type: 'script' | 'style', content?: string, targetQuery?: string, position?: InsertPosition): Observable<void>;
}
