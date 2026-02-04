import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, distinctUntilChanged, switchMap, of } from 'rxjs';
import { catchError, shareReplay, tap } from 'rxjs/operators';
import { ABP } from '../models/common';
import { LocalizationService } from './localization.service';
import { SessionStateService } from './session-state.service';
import { CORE_OPTIONS } from '../tokens/options.token';

export interface UILocalizationResource {
  [resourceName: string]: Record<string, string>;
}

/**
 * Service for managing UI localizations in ABP Angular applications.
 * Automatically loads localization files based on selected language
 * Merges with backend localizations (UI > Backend priority)
 */
@Injectable({ providedIn: 'root' })
export class UILocalizationService {
  private http = inject(HttpClient);
  private localizationService = inject(LocalizationService);
  private sessionState = inject(SessionStateService);
  private options = inject(CORE_OPTIONS);

  private loadedLocalizations$ = new BehaviorSubject<Record<string, UILocalizationResource>>({});

  private currentLanguage$ = this.sessionState.getLanguage$();

  constructor() {
    const uiLocalization = this.options.uiLocalization;
    if (uiLocalization?.enabled) {
      this.subscribeToLanguageChanges();
    }
  }

  private subscribeToLanguageChanges() {
    this.currentLanguage$
      .pipe(
        distinctUntilChanged(),
        switchMap(culture => this.loadLocalizationFile(culture))
      )
      .subscribe();
  }

  private loadLocalizationFile(culture: string) {
    const config = this.options.uiLocalization;
    if (!config?.enabled) return of(null);

    const basePath = config.basePath || '/assets/localization';
    const url = `${basePath}/${culture}.json`;

    return this.http.get<UILocalizationResource>(url).pipe(
      catchError(() => {
        // If file not found or error occurs, return null
        return of(null);
      }),
      tap(data => {
        if (data) {
          this.processLocalizationData(culture, data);
        }
      }),
    );
  }

  private processLocalizationData(culture: string, data: UILocalizationResource) {
    const abpFormat: ABP.Localization[] = [
      {
        culture,
        resources: Object.entries(data).map(([resourceName, texts]) => ({
          resourceName,
          texts,
        })),
      },
    ];
    this.localizationService.addLocalization(abpFormat);

    const current = this.loadedLocalizations$.value;
    current[culture] = data;
    this.loadedLocalizations$.next(current);
  }

  addAngularLocalizeLocalization(
    culture: string,
    resourceName: string,
    translations: Record<string, string>,
  ): void {
    const abpFormat: ABP.Localization[] = [
      {
        culture,
        resources: [
          {
            resourceName,
            texts: translations,
          },
        ],
      },
    ];
    this.localizationService.addLocalization(abpFormat);

    const current = this.loadedLocalizations$.value;
    if (!current[culture]) {
      current[culture] = {};
    }
    if (!current[culture][resourceName]) {
      current[culture][resourceName] = {};
    }
    current[culture][resourceName] = {
      ...current[culture][resourceName],
      ...translations,
    };
    this.loadedLocalizations$.next(current);
  }

  getLoadedLocalizations(culture?: string): UILocalizationResource {
    const lang = culture || this.sessionState.getLanguage();
    return this.loadedLocalizations$.value[lang] || {};
  }
}
