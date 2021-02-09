import { ContentProjectionService, PROJECTION_STRATEGY } from '@abp/ng.core';
import { ComponentRef, Injectable, TemplateRef, ViewContainerRef, OnDestroy } from '@angular/core';
import { ModalContainerComponent } from '../components/modal/modal-container.component';

/**
 * @deprecated Use ng-bootstrap modal. To be deleted in v5.0.
 */
@Injectable({
  providedIn: 'root',
})
export class ModalService implements OnDestroy {
  private containerComponentRef: ComponentRef<ModalContainerComponent>;

  constructor(private contentProjectionService: ContentProjectionService) {
    this.setContainer();
  }

  private setContainer() {
    this.containerComponentRef = this.contentProjectionService.projectContent(
      PROJECTION_STRATEGY.AppendComponentToBody(ModalContainerComponent),
    );

    this.containerComponentRef.changeDetectorRef.detectChanges();
  }

  clearModal() {
    this.getContainer().clear();
    this.detectChanges();
  }

  detectChanges() {
    this.containerComponentRef.changeDetectorRef.detectChanges();
  }

  getContainer(): ViewContainerRef {
    return this.containerComponentRef.instance.container;
  }

  renderTemplate<T extends any>(template: TemplateRef<T>, context?: T) {
    const containerRef = this.getContainer();

    const strategy = PROJECTION_STRATEGY.ProjectTemplateToContainer(
      template,
      containerRef,
      context,
    );

    this.contentProjectionService.projectContent(strategy);
  }

  ngOnDestroy() {
    this.containerComponentRef.destroy();
  }
}
