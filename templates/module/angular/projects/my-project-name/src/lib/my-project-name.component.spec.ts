import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { MyProjectNameComponent } from './components/my-project-name.component';
import { MyProjectNameService } from '@my-company-name/my-project-name';
import { of } from 'rxjs';

describe('MyProjectNameComponent', () => {
  let component: MyProjectNameComponent;
  let fixture: ComponentFixture<MyProjectNameComponent>;
  const mockMyProjectNameService = jasmine.createSpyObj('MyProjectNameService', {
    sample: of([]),
  });
  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [MyProjectNameComponent],
      providers: [
        {
          provide: MyProjectNameService,
          useValue: mockMyProjectNameService,
        },
      ],
    }).compileComponents();
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
