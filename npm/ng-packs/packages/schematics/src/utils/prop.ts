export const shouldQuoteProp = (key: string) => {
  try {
    new Function('return {}.' + key);
  } catch (_) {
    return true;
  }

  return false;
};
