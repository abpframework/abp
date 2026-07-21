import { Pipe, PipeTransform, inject } from '@angular/core';
import { RouteBasedCultureUrlService } from '../services/route-based-culture-url.service';

/**
 * Prefixes menu/navigation paths with the current culture when route-based culture is enabled.
 * Impure so links update after language changes.
 */
@Pipe({
  name: 'abpRouteCultureUrl',
  pure: false,
})
export class AbpRouteCultureUrlPipe implements PipeTransform {
  private readonly url = inject(RouteBasedCultureUrlService);

  transform(path: string | undefined | null): string | undefined | null {
    return this.url.prefixPathWithCulture(path);
  }
}
