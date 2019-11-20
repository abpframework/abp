import { Argument } from '../../utils/generators';
export declare namespace ServiceTemplates {
    function classTemplate(name: string, content: string): string;
    function getMethodTemplate(name: string, url: string, args?: string, params?: Argument[], queryParams?: object): string;
    function requestTemplate(method: string, url: string, params?: Argument[], body?: boolean, queryParams?: boolean): string;
}
