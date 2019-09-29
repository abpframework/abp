import { NgxsConfig } from '@ngxs/store/src/symbols';
export declare function removeDollarAtTheEnd(name: string): string;
export declare function getPropsArray(selectorOrFeature: string, paths: string[]): string[];
export declare function propGetter(paths: string[], config: NgxsConfig): (x: any) => any;
export declare const META_KEY = "NGXS_META";
