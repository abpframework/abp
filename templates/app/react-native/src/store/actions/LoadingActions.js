import { createAction } from '@reduxjs/toolkit';

const start = createAction('loading/start');

const stop = createAction('loading/stop');

const clear = createAction('loading/clear');

export default {
  start,
  stop,
  clear,
};
