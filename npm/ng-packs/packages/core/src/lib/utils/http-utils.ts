export function getPathName(url: string): string {
  const { pathname } = new URL(url);
  return pathname;
}
