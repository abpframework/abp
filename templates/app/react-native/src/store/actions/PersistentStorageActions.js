import { createAction } from '@reduxjs/toolkit';

const setToken = createAction('persistentStorage/setToken');

const setLanguage = createAction('persistentStorage/setLanguage');

const setTenant = createAction('persistentStorage/setTenant');

export default {
  setToken,
  setLanguage,
  setTenant,
};
