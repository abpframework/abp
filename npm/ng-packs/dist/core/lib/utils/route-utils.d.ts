import { ABP } from '../models';
export declare function organizeRoutes(routes: ABP.FullRoute[], wrappers?: ABP.FullRoute[], parentNameArr?: ABP.FullRoute[], parentName?: string): ABP.FullRoute[];
export declare function setChildRoute(routes: ABP.FullRoute[], parentNameArr: ABP.FullRoute[]): ABP.FullRoute[];
export declare function sortRoutes(routes?: ABP.FullRoute[]): ABP.FullRoute[];
export declare function addAbpRoutes(routes: ABP.FullRoute | ABP.FullRoute[]): void;
export declare function getAbpRoutes(): ABP.FullRoute[];
