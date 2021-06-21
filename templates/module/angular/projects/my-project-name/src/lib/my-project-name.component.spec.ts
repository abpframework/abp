import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { MyProjectNameComponent } from './my-project-name.component';

describe('MyProjectNameComponent', () => {
  let component: MyProjectNameComponent;
  let fixture: ComponentFixture<MyProjectNameComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ MyProjectNameComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MyProjectNameComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
