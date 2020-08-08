
export class FindTenantResultDto  {
  success: boolean;
  tenantId?: string;
  name: string;

  constructor(initialValues: Partial<FindTenantResultDto> = {}) {
    for (const key in initialValues) {
      if (initialValues.hasOwnProperty(key)) {
        this[key] = initialValues[key];
      }
    }
  }
}
