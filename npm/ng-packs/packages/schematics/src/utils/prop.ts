export const isValidProp = (name: string) => {
  try {
    new Function(name, 'var ' + name);
  } catch (_) {
    return false;
  }

  return true;
};
