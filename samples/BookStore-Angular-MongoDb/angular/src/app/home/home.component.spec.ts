import { ComponentFixture, TestBed } from '@angular/core/testing';
import { OAuthService } from 'angular-oauth2-oidc';
import { HomeComponent } from './home.component';
import { HomeModule } from './home.module';
import { RouterTestingModule } from '@angular/router/testing';
import { NgxsModule } from '@ngxs/store';
import { By } from '@angular/platform-browser';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;
  let mockOAuthService: { hasValidAccessToken: () => boolean };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ NgxsModule.forRoot(), HomeModule, RouterTestingModule],
      providers: [{ provide: OAuthService, useValue: { hasValidAccessToken: () => false } }],
    });
    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
    mockOAuthService = TestBed.get(OAuthService);
    fixture.detectChanges();
  });

  describe('#hasLoggedIn', () => {
    it('should return the hasValidAccessToken method of oAuthService', () => {
      const spy = spyOn(mockOAuthService, 'hasValidAccessToken');
      spy.and.returnValue(false);

      expect(component.hasLoggedIn).toBe(false);
      expect(spy).toHaveBeenCalled();
    });
  });

  describe('login button', () => {
    it('should display', () => {
      const button = fixture.debugElement.query(By.css('[routerLink="/account/login"]'));
      expect(button).toBeTruthy();
      expect(button.nativeElement.textContent).toContain('AbpAccount::Login');
    });

    it('should not display when user logged in', () => {
      const spy = spyOn(mockOAuthService, 'hasValidAccessToken');
      spy.and.returnValue(true);
      fixture.detectChanges();

      const button = fixture.debugElement.query(By.css('#login-button'));
      expect(button).toBeFalsy();
    });
  });
});
