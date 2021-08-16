import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TenantBoxComponent } from './tenant-box.component';

describe('TenantBoxComponent', () => {
  let component: TenantBoxComponent;
  let fixture: ComponentFixture<TenantBoxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TenantBoxComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TenantBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
