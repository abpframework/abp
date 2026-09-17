import { By } from '@angular/platform-browser';
import { Component, DebugElement } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TrackCapsLockDirective } from '../directives';

@Component({
  template: ` <input abpCapsLock /> `,
  imports: [TrackCapsLockDirective],
})
class TestComponent {
  capsLock = false;
}

describe('TrackCapsLockDirective', () => {
  let fixture: ComponentFixture<TestComponent>;
  let des: DebugElement[];
  let directive: TrackCapsLockDirective;
  let emitSpy: ReturnType<typeof vi.spyOn>;

  const createCapsLockEvent = (eventName: string, capsLock: boolean) => {
    const event = new KeyboardEvent(eventName, {
      key: 'CapsLock',
    });

    Object.defineProperty(event, 'getModifierState', {
      value: (key: string) => key === 'CapsLock' && capsLock,
    });

    return event;
  };

  beforeEach(() => {
    fixture = TestBed.configureTestingModule({
      imports: [TestComponent],
    }).createComponent(TestComponent);

    fixture.detectChanges();

    des = fixture.debugElement.queryAll(By.directive(TrackCapsLockDirective));
    directive = des[0].injector.get(TrackCapsLockDirective);
    emitSpy = vi.spyOn(directive.capsLock, 'emit');
  });

  afterEach(() => {
    vi.restoreAllMocks();
  });

  test.each(['keydown', 'keyup'])(
    'is %p works when press capslock and is emit status',
    eventName => {
      const event = createCapsLockEvent(eventName, true);
      eventName === 'keydown' ? directive.onKeyDown(event) : directive.onKeyUp(event);
      expect(emitSpy).toHaveBeenCalledWith(true);
    },
  );

  test.each(['keydown', 'keyup'])('is %p detect the change capslock is emit status', eventName => {
    const trueEvent = createCapsLockEvent(eventName, true);
    eventName === 'keydown' ? directive.onKeyDown(trueEvent) : directive.onKeyUp(trueEvent);
    expect(emitSpy).toHaveBeenCalledWith(true);
    const falseEvent = createCapsLockEvent(eventName, false);
    eventName === 'keydown' ? directive.onKeyDown(falseEvent) : directive.onKeyUp(falseEvent);
    expect(emitSpy).toHaveBeenLastCalledWith(false);
  });
});
