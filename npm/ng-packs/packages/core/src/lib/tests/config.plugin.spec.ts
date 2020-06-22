import { InitState } from '@ngxs/store';
import { ABP } from '../models';
import { ConfigPlugin } from '../plugins';

const options: ABP.Root = {
  environment: {
    production: false,
  },
};

const event = new InitState();

const state = {
  ConfigState: {
    foo: 'bar',
    ...options,
  },
};

describe('ConfigPlugin', () => {
  it('should ConfigState must be create with correct datas', () => {
    const next = jest.fn();
    const plugin = new ConfigPlugin(options);
    plugin.handle({ ConfigState: { foo: 'bar' } }, event, next);
    expect(next).toHaveBeenCalledWith(state, event);
    expect(next).toHaveBeenCalledTimes(1);
    next.mockClear();

    delete state.ConfigState.environment;
    plugin.handle(state, event, next);
    expect(next).toHaveBeenCalledWith(state, event);
    expect(next).toHaveBeenCalledTimes(1);
  });
});
