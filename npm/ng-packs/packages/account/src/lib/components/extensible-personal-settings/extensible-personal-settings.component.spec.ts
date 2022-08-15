import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExtensiblePersonalSettingsComponent } from './extensible-personal-settings.component';

describe('ExtensiblePersonalSettingsComponent', () => {
  let component: ExtensiblePersonalSettingsComponent;
  let fixture: ComponentFixture<ExtensiblePersonalSettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ExtensiblePersonalSettingsComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ExtensiblePersonalSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
