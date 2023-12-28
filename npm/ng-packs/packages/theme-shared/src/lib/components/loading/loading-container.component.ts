 import { ChangeDetectionStrategy, Component, Input, inject } from '@angular/core';
 import { DISABLE_LOADING_COMPONENT_TOKEN } from '../../tokens/loading-container.token';

@Component({
  selector: 'abp-loading-container',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div [ngClass]="{ 'position-relative': isBusy }">
      <ng-content> </ng-content>
      @if(isBusy){
      <abp-loading/>
      }
    </div>
  `,
})
export class LoadingContainerComponent {
  isLoadingDisabled = inject(DISABLE_LOADING_COMPONENT_TOKEN, { optional: true }) || false;
  @Input({ required: true }) loading: boolean;

  protected get isBusy() {
    return this.loading && !this.isLoadingDisabled;
  }
}
