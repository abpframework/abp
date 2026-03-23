import { Directive, HostListener, output } from '@angular/core';

@Directive({
  selector: '[abpCapsLock]',
})
export class TrackCapsLockDirective {
  readonly capsLock = output<boolean>({ alias: 'abpCapsLock' });

  @HostListener('window:keydown', ['$event'])
  onKeyDown(event: KeyboardEvent): void {
    this.capsLock.emit(this.isCapsLockOpen(event));
  }
  @HostListener('window:keyup', ['$event'])
  onKeyUp(event: KeyboardEvent): void {
    this.capsLock.emit(this.isCapsLockOpen(event));
  }

  isCapsLockOpen(e): boolean {
    const s = String.fromCharCode(e.which);
    if (
      (s.toUpperCase() === s && s.toLowerCase() !== s && e.shiftKey) ||
      (s.toUpperCase() !== s && s.toLowerCase() === s && e.shiftKey) ||
      (e.getModifierState && e.getModifierState('CapsLock'))
    ) {
      return true;
    }
    return false;
  }
}
