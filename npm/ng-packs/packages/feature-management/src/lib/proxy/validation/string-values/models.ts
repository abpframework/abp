
export interface IStringValueType {
  name: string;
  item: object;
  properties: Record<string, object>;
  validator: IValueValidator;
}

export interface IValueValidator {
  name: string;
  item: object;
  properties: Record<string, object>;
}
