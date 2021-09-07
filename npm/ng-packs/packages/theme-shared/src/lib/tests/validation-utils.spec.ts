import { ConfigStateService } from '@abp/ng.core';
import { CoreTestingModule } from '@abp/ng.core/testing';
import { HttpClient } from '@angular/common/http';
import { Component, Injector } from '@angular/core';
import { Validators } from '@angular/forms';
import { createComponentFactory, Spectator } from '@ngneat/spectator/jest';
import { NgxValidateCoreModule, validatePassword } from '@ngx-validate/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { getPasswordValidators } from '../utils';
@Component({ template: '', selector: 'abp-dummy' })
class DummyComponent {}

describe('ValidationUtils', () => {
  let spectator: Spectator<DummyComponent>;
  const createComponent = createComponentFactory({
    component: DummyComponent,
    imports: [CoreTestingModule.withConfig(), NgxValidateCoreModule.forRoot()],
    mocks: [HttpClient, OAuthService],
  });

  beforeEach(() => (spectator = createComponent()));

  describe('#getPasswordValidators', () => {
    it('should return password valdiators', () => {
      const configState = spectator.inject(ConfigStateService);
      configState.setState({
        setting: {
          values: {
            'Abp.Identity.Password.RequiredLength': '6',
            'Abp.Identity.Password.RequiredUniqueChars': '1',
            'Abp.Identity.Password.RequireNonAlphanumeric': 'True',
            'Abp.Identity.Password.RequireLowercase': 'True',
            'Abp.Identity.Password.RequireUppercase': 'True',
            'Abp.Identity.Password.RequireDigit': 'True',
          },
        },
      });

      const validators = getPasswordValidators(spectator.inject(Injector));
      const expectedValidators = [
        validatePassword(['number', 'small', 'capital', 'special']),
        Validators.minLength(6),
        Validators.maxLength(128),
      ];

      validators.forEach((validator, index) => {
        expect(validator.toString()).toBe(expectedValidators[index].toString());
      });
    });
  });
});
