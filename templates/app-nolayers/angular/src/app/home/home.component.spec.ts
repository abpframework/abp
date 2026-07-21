import { CoreTestingModule } from '@abp/ng.core/testing';
import { ThemeSharedTestingModule } from '@abp/ng.theme.shared/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { HomeComponent } from './home.component';
import { AuthService } from '@abp/ng.core';
import { vi } from 'vitest';

describe('HomeComponent', () => {
  let fixture: ComponentFixture<HomeComponent>;
  let mockAuthService: { isAuthenticated: boolean; navigateToLogin: ReturnType<typeof vi.fn> };

  beforeEach(async () => {
    mockAuthService = {
      isAuthenticated: false,
      navigateToLogin: vi.fn(),
    };

    await TestBed.configureTestingModule({
      imports: [
        CoreTestingModule.withConfig(),
        ThemeSharedTestingModule.withConfig(),
        NgxValidateCoreModule,
        HomeComponent,
      ],
      providers: [
        {
          provide: AuthService,
          useValue: mockAuthService,
        },
      ],
    }).compileComponents();
  });

  it('should be initiated', () => {
    fixture = TestBed.createComponent(HomeComponent);
    fixture.detectChanges();
    expect(fixture.componentInstance).toBeTruthy();
  });

  describe('when login state is true', () => {
    beforeEach(() => {
      mockAuthService.isAuthenticated = true;
      fixture = TestBed.createComponent(HomeComponent);
      fixture.detectChanges();
    });

    it('hasLoggedIn should be true', () => {
      expect(fixture.componentInstance.hasLoggedIn).toBe(true);
    });

    it('button should not be exists', () => {
      const element = fixture.nativeElement;
      const cardTitle = element.querySelector('.card-title');
      expect(cardTitle).toBeTruthy();
    });
  });
});
