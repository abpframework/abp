export const isValidProp = (name: string) => {
  try {
    new Function('return {}.' + name);
  } catch (_) {
    return false;
  }

  return true;
};
