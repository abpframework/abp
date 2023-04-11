import { CoreTestingModule } from '@abp/ng.core/testing';
import { ThemeSharedTestingModule } from '@abp/ng.theme.shared/testing';
import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { HomeComponent } from './home.component';
import { OAuthService } from 'angular-oauth2-oidc';
import { AuthService } from '@abp/ng.core';

describe('HomeComponent', () => {
  let fixture: ComponentFixture<HomeComponent>;
  const mockOAuthService = jasmine.createSpyObj('OAuthService', ['hasValidAccessToken']);
  const mockAuthService = jasmine.createSpyObj('AuthService', ['navigateToLogin']);
  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [HomeComponent],
      imports: [
        CoreTestingModule.withConfig(),
        ThemeSharedTestingModule.withConfig(),
        NgxValidateCoreModule,
      ],
      providers: [
        /* mock providers here */
        {
          provide: OAuthService,
          useValue: mockOAuthService,
        },
        {
          provide: AuthService,
          useValue: mockAuthService,
        },
      ],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeComponent);
    fixture.detectChanges();
  });

  it('should be initiated', () => {
    expect(fixture.componentInstance).toBeTruthy();
  });

  describe('when login state is true', () => {
    beforeAll(() => {
      mockOAuthService.hasValidAccessToken.and.returnValue(true);
    });

    it('hasLoggedIn should be true', () => {
      expect(fixture.componentInstance.hasLoggedIn).toBeTrue();
      expect(mockOAuthService.hasValidAccessToken).toHaveBeenCalled();
    });

    it('button should not be exists', () => {
      const element = fixture.nativeElement;
      const cardTitle = element.querySelector('.card-title');
      expect(cardTitle).toBeTruthy();
    });
  });
});
