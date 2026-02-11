import { Component, inject, OnInit } from '@angular/core';
import { LocalizationPipe, UILocalizationService, SessionStateService } from '@abp/ng.core';
import { CommonModule } from '@angular/common';
import { CardComponent, CardBodyComponent } from '@abp/ng.theme.shared';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-localization-test',
  standalone: true,
  imports: [CommonModule, LocalizationPipe, CardComponent, CardBodyComponent, AsyncPipe],
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
          <h5>UI Localization (from /assets/localization/{{ currentLanguage$ | async }}.json)</h5>
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
          <pre>{{ loadedLocalizations | json }}</pre>
        </abp-card-body>
      </abp-card>
    </div>
  `,
})
export class LocalizationTestComponent implements OnInit {
  private uiLocalizationService = inject(UILocalizationService);
  private sessionState = inject(SessionStateService);

  loadedLocalizations: any = {};
  currentLanguage$ = this.sessionState.getLanguage$();

  ngOnInit() {
    this.loadedLocalizations = this.uiLocalizationService.getLoadedLocalizations();
  }
}
