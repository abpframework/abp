import { Directive, EventEmitter, HostListener, Output } from '@angular/core';

@Directive({
  standalone: true,
  selector: '[abpCapsLock]',
})
export class TrackCapsLockDirective {
  @Output('abpCapsLock') capsLock = new EventEmitter<boolean>();

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
