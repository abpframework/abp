import { describe, expect, it } from 'vitest';
import { getParamValueName } from '../utils/methods';

describe('getParamValueName', () => {
  it('should navigate nested params with optional chaining', () => {
    expect(getParamValueName('NestedFilter.SomeField', 'input')).toBe(
      'input?.nestedFilter?.someField',
    );
  });

  it('should optional-chain every level of a deeper nested param', () => {
    expect(getParamValueName('Filter.Inner.Value', 'input')).toBe('input?.filter?.inner?.value');
  });

  it('should not add optional chaining for a non-nested param', () => {
    expect(getParamValueName('someField', 'input')).toBe('input.someField');
  });

  it('should keep bracket access for a non-nested param that needs quoting', () => {
    expect(getParamValueName('some-field', 'input')).toBe("input['some-field']");
  });

  it('should quote a nested segment that needs quoting and still optional-chain', () => {
    expect(getParamValueName('Filter.some-field', 'input')).toBe("input?.filter?.['some-field']");
  });
});
