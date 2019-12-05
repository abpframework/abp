import { NgbDateParserFormatter, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { DatePipe } from '@angular/common';
export declare class DateParserFormatter extends NgbDateParserFormatter {
    private datePipe;
    constructor(datePipe: DatePipe);
    parse(value: string): NgbDateStruct;
    format(date: NgbDateStruct): string;
}
