import { describe, expect, it } from 'vitest';
import { createActionToBodyMapper } from '../utils/service';
import { eBindingSourceId } from '../enums';
import { Action } from '../models/api-definition';

// Exercises the real proxy-generation pipeline (registerActionParameter -> getParamValueName ->
// Body.params) for a flattened nullable nested query param, the case reported in #25176.
describe('proxy generation - nested nullable query param', () => {
  const action = {
    uniqueName: 'GetListAsync',
    name: 'GetListAsync',
    httpMethod: 'GET',
    url: 'api/app/my',
    supportedVersions: [],
    parametersOnMethod: [],
    parameters: [
      {
        nameOnMethod: 'input',
        name: 'NestedFilter.SomeField',
        jsonName: null,
        type: 'System.String',
        typeSimple: 'string',
        isOptional: true,
        defaultValue: null,
        constraintTypes: null,
        bindingSourceId: eBindingSourceId.Query,
        descriptorName: 'input',
      },
    ],
    returnValue: { type: 'System.Void', typeSimple: 'void' },
  } as unknown as Action;

  it('emits optional chaining in the generated params object', () => {
    const body = createActionToBodyMapper()(action);

    expect(body.params).toContain(
      '["NestedFilter.SomeField"]: input?.nestedFilter?.someField',
    );
  });
});
