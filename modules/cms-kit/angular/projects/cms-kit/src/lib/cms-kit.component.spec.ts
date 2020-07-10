import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CmsKitComponent } from './cms-kit.component';

describe('CmsKitComponent', () => {
  let component: CmsKitComponent;
  let fixture: ComponentFixture<CmsKitComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CmsKitComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CmsKitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
