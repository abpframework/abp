import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { LocalizationPipe, UILocalizationService, SessionStateService } from '@abp/ng.core';
import { CommonModule } from '@angular/common';
import { CardComponent, CardBodyComponent } from '@abp/ng.theme.shared';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-localization-test',
  imports: [CommonModule, LocalizationPipe, CardComponent, CardBodyComponent],
  template: `
    <div class="container mt-5">
      <h2>Hybrid Localization Test</h2>

      <abp-card cardClass="mt-4">
        <abp-card-body>
          <h5>Backend Localization (if available)</h5>
          <p><strong>MyProjectName::Welcome:</strong> {{ 'MyProjectName::Welcome' | abpLocalization }}</p>
          <p><strong>AbpAccount::Login:</strong> {{ 'AbpAccount::Login' | abpLocalization }}</p>
        </abp-card-body>
      </abp-card>

      <abp-card cardClass="mt-4">
        <abp-card-body>
          <h5>UI Localization (from /assets/localization/{{ currentLanguage() }}.json)</h5>
          <p><strong>MyProjectName::CustomKey:</strong> {{ 'MyProjectName::CustomKey' | abpLocalization }}</p>
          <p><strong>MyProjectName::TestMessage:</strong> {{ 'MyProjectName::TestMessage' | abpLocalization }}</p>
        </abp-card-body>
      </abp-card>

      <abp-card cardClass="mt-4">
        <abp-card-body>
          <h5>UI Override (UI > Backend Priority)</h5>
          <p><strong>AbpAccount::Login:</strong> {{ 'AbpAccount::Login' | abpLocalization }}</p>
          <p class="text-muted">If backend has "Login", UI version should override it</p>
        </abp-card-body>
      </abp-card>

      <abp-card cardClass="mt-4">
        <abp-card-body>
          <h5>Loaded UI Localizations</h5>
          <pre>{{ loadedLocalizations() | json }}</pre>
        </abp-card-body>
      </abp-card>
    </div>
  `,
})
export class LocalizationTestComponent {
  private uiLocalizationService = inject(UILocalizationService);
  private sessionState = inject(SessionStateService);

  readonly loadedLocalizations = signal(this.uiLocalizationService.getLoadedLocalizations());

  readonly currentLanguage = toSignal(this.sessionState.getLanguage$(), {
    initialValue: this.sessionState.getLanguage(),
  });
}
