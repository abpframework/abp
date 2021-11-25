export function isTokenValid(token) {
  if (!token || typeof token !== 'object' || !token.expire_time) return false;

  const now = new Date().valueOf();

  return now < token.expire_time;
}
