export declare namespace Statistics {
    interface Response {
        data: Data;
    }
    interface Data {
        [key: string]: number;
    }
    interface Filter {
        startDate: string | Date;
        endDate: string | Date;
    }
}
