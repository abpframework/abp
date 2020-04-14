export function toLocalISOString(date) {
  const timezoneOffset = date.getTimezoneOffset();

  return new Date(date.getTime() - timezoneOffset * 60000).toISOString();
}
