import { Component, DebugElement } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { ShowPasswordDirective } from '../directives';
import { setInputSignal } from './utils';
@Component({
  template: ` <input abpShowPassword />
    <input abpShowPassword />
    <input />
    <input abpShowPassword />`,
  imports: [ShowPasswordDirective],
})
class TestComponent {}

describe('ShowPasswordDirective', () => {
  let fixture: ComponentFixture<TestComponent>;
  let des: DebugElement[];
  let desAll: DebugElement[];

  const detectChanges = () => {
    fixture.detectChanges();
    TestBed.flushEffects();
  };

  const setShowPassword = (index: number, value: boolean) => {
    setInputSignal(des[index].injector.get(ShowPasswordDirective).abpShowPassword, value);
    detectChanges();
  };

  beforeEach(() => {
    fixture = TestBed.configureTestingModule({
      imports: [TestComponent],
    }).createComponent(TestComponent);

    detectChanges();

    des = fixture.debugElement.queryAll(By.directive(ShowPasswordDirective));

    desAll = fixture.debugElement.queryAll(By.all());

    setShowPassword(0, true);
    setShowPassword(1, false);
    setShowPassword(2, false);
  });

  it('should have three input has ShowPasswordDirective elements', () => {
    expect(des.length).toBe(3);
  });

  test.each([
    [0, 'text'],
    [1, 'password'],
    [2, 'text'],
    [3, 'password'],
  ])('%p. input type must be %p)', (index, inpType) => {
    const inputType = desAll[index].nativeElement.type;
    expect(inputType).toBe(inpType);
  });

  it('should toggle input type when showPassword changes', () => {
    const input = des[2].nativeElement;
    expect(input.type).toBe('password');

    setShowPassword(2, true);

    expect(input.type).toBe('text');
  });
});
