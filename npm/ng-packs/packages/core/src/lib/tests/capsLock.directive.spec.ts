import { Component, DebugElement } from '@angular/core'
import { ComponentFixture, TestBed } from '@angular/core/testing'
import { TrackCapsLockDirective } from '../directives';
import { By } from '@angular/platform-browser';

@Component({
  standalone:true,
  template: `
    <input (abpCapsLock)="capsLock = $event" />
  `,
  imports:[TrackCapsLockDirective]
})
class TestComponent {
  capsLock = false
}

describe('TrackCapsLockDirective',()=>{
  let fixture: ComponentFixture<TestComponent>;;
  let des : DebugElement[];

  beforeEach(()=>{
    fixture = TestBed.configureTestingModule({
      imports: [ TestComponent ]
    }).createComponent(TestComponent);

    fixture.detectChanges();

    des = fixture.debugElement.queryAll(By.directive(TrackCapsLockDirective));
  });

    test.each(['keydown','keyup'])('is %p works when press capslock and is emit status', (eventName) => {
      const event = new KeyboardEvent(eventName, {
        key: 'CapsLock',
        modifierCapsLock: true
      });
      window.dispatchEvent(event);
      fixture.detectChanges();
      expect(fixture.componentInstance.capsLock).toBe(true)
    });

    test.each(['keydown','keyup'])('is %p detect the change capslock is emit status', (eventName) => {
      const trueEvent = new KeyboardEvent(eventName, {
        key: 'CapsLock',
        modifierCapsLock: true
      });
      window.dispatchEvent(trueEvent);
      fixture.detectChanges();
      expect(fixture.componentInstance.capsLock).toBe(true)
      const falseEvent = new KeyboardEvent(eventName, {
        key: 'CapsLock',
        modifierCapsLock: false
      });
      window.dispatchEvent(falseEvent);
      fixture.detectChanges();
      expect(fixture.componentInstance.capsLock).toBe(false)
    });
  });