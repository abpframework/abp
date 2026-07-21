import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MyProjectNameComponent } from './components/my-project-name.component';
import { MyProjectNameService } from '@my-company-name/my-project-name';
import { of } from 'rxjs';
import { vi } from 'vitest';

describe('MyProjectNameComponent', () => {
  let component: MyProjectNameComponent;
  let fixture: ComponentFixture<MyProjectNameComponent>;
  const mockMyProjectNameService = {
    sample: vi.fn().mockReturnValue(of([])),
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MyProjectNameComponent],
      providers: [
        {
          provide: MyProjectNameService,
          useValue: mockMyProjectNameService,
        },
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MyProjectNameComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
