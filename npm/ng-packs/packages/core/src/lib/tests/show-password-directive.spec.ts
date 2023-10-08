import { Component, DebugElement } from '@angular/core'
import { ComponentFixture, TestBed } from '@angular/core/testing'
import { ShowPasswordDirective } from '../directives';
import { By } from '@angular/platform-browser';

@Component({
  standalone:true,
  template: `
  <input [abpShowPassword]="true">
  <input [abpShowPassword]="false">
  <input />
  <input [abpShowPassword]="showPassword" />`,
  imports:[ShowPasswordDirective]
})
class TestComponent {
  showPassword = false
}

describe('ShowPasswordDirective',()=>{
  let fixture: ComponentFixture<TestComponent>;;
  let des : DebugElement[];
  let desAll : DebugElement[];
  let bareInput;

  beforeEach(()=>{
    fixture = TestBed.configureTestingModule({
      imports: [ TestComponent ]
    }).createComponent(TestComponent)

    fixture.detectChanges();

    des = fixture.debugElement.queryAll(By.directive(ShowPasswordDirective));

    desAll = fixture.debugElement.queryAll(By.all());
    
    bareInput = fixture.debugElement.query(By.css('input:not([abpShowPassword])'));
    })

    it('should have three input has ShowPasswordDirective elements', () => {
      expect(des.length).toBe(3);
    });

    test.each([[0,'text'],[1,'password'],[2,'text'],[3,'password']])('%p. input type must be %p)', (index,inpType) => {
      const inputType = desAll[index].nativeElement.type;
      expect(inputType).toBe(inpType);
    });

    it('should have three input has ShowPasswordDirective elements', () => {
      const input = des[2].nativeElement
      expect(input.type).toBe('password')
      fixture.componentInstance.showPassword = true
      fixture.detectChanges()
      expect(input.type).toBe('text')
    });
  });