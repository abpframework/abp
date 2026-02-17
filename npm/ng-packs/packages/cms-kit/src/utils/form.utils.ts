import { FormGroup } from '@angular/forms';
import { DestroyRef } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { dasharize } from './text.utils';

/**
 * Sets up automatic slug generation from a source control (e.g., title, name) to a target control (slug).
 * The slug is automatically updated when the source control value changes.
 *
 * @param form - The form group containing the controls
 * @param sourceControlName - Name of the source control (e.g., 'title', 'name')
 * @param targetControlName - Name of the target control (e.g., 'slug')
 * @param destroyRef - DestroyRef for automatic subscription cleanup
 */
export function prepareSlugFromControl(
  form: FormGroup,
  sourceControlName: string,
  targetControlName: string,
  destroyRef: DestroyRef,
): void {
  const sourceControl = form.get(sourceControlName);
  const targetControl = form.get(targetControlName);

  if (!sourceControl || !targetControl) {
    return;
  }

  sourceControl.valueChanges.pipe(takeUntilDestroyed(destroyRef)).subscribe(value => {
    if (value && typeof value === 'string') {
      const dasharized = dasharize(value);
      const currentSlug = targetControl.value || '';
      if (dasharized !== currentSlug) {
        targetControl.setValue(dasharized, { emitEvent: false });
      }
    }
  });
}
