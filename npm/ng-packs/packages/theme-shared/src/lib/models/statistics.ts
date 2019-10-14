export namespace Statistics {
  export interface Response {
    data: Data;
  }

  export interface Data {
    [key: string]: number;
  }

  export interface Filter {
    startDate: string | Date;
    endDate: string | Date;
  }
}
