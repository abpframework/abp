import { Layout } from '../models/layout';
export declare class AddNavigationElement {
    payload: Layout.NavigationElement | Layout.NavigationElement[];
    static readonly type = "[Layout] Add Navigation Element";
    constructor(payload: Layout.NavigationElement | Layout.NavigationElement[]);
}
export declare class RemoveNavigationElementByName {
    name: string;
    static readonly type = "[Layout] Remove Navigation ElementByName";
    constructor(name: string);
}
