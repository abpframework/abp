export function getPathName(url: string): string {
  const { pathname } = new URL(url, window.location.origin);
  return pathname;
}
