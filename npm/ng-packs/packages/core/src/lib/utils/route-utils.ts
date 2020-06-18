import { PRIMARY_OUTLET, Router, UrlSegmentGroup } from '@angular/router';

export function getRoutePath(router: Router) {
  const emptyGroup = { segments: [] } as UrlSegmentGroup;
  const primaryGroup = router.parseUrl(router.url).root.children[PRIMARY_OUTLET];

  return '/' + (primaryGroup || emptyGroup).segments.map(({ path }) => path).join('/');
}
