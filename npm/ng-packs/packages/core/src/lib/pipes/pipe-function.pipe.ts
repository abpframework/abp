import {Pipe, PipeTransform} from "@angular/core";

@Pipe({
  name: 'pipeFunction'
})
export class PipeFunction implements PipeTransform {
  public transform(value: any, handler: (value: any) => any, context?: any): any {
    if (context) {
      return handler.call(context, value);
    }

    return handler(value);
  }
}
