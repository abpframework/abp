import { ABP } from '@abp/ng.core';

export namespace Books {
  export interface State {
    data: Response;
  }

  export type Response = ABP.PagedResponse<Item>;

  export interface Item {
    name: string;
    type: Type;
    publishDate: string;
    price: number;
    lastModificationTime: string;
    lastModifierId: string;
    creationTime: string;
    creatorId: string;
    id: string;
  }

  export enum Type {
    Undefined,
    Adventure,
    Biography,
    Dystopia,
    Fantastic,
    Horror,
    Science,
    ScienceFiction,
    Poetry,
  }

  export interface SaveRequest {
    name: string;
    type: Type;
    publishDate: string;
    price: number;
  }
}
