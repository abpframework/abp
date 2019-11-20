import { APIDefination } from '../types/api-defination';
export interface Argument {
    key: string;
    type: string;
    isOptional?: boolean;
}
export declare function generateArgs(args: Argument[]): string;
export declare function parseParameters(parameters: APIDefination.Parameter[]): Argument[];
export declare function findType(typeAsString: string): string;
