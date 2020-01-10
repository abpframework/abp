import {
  Directive,
  ElementRef,
  AfterViewInit,
  ViewContainerRef,
  ComponentFactoryResolver,
  Input,
  Injector,
  ComponentRef,
  ComponentFactory,
  HostBinding,
  EmbeddedViewRef,
  Renderer2,
  OnInit,
} from '@angular/core';
import { LoadingComponent } from '../components/loading/loading.component';

@Directive({ selector: '[abpLoading]' })
export class LoadingDirective implements OnInit {
  private _loading: boolean;

  @HostBinding('style.position')
  position = 'relative';

  @Input('abpLoading')
  get loading(): boolean {
    return this._loading;
  }

  set loading(newValue: boolean) {
    setTimeout(() => {
      if (!this.componentRef) {
        this.componentRef = this.cdRes
          .resolveComponentFactory(LoadingComponent)
          .create(this.injector);
      }

      if (newValue && !this.rootNode) {
        this.rootNode = (this.componentRef.hostView as EmbeddedViewRef<any>).rootNodes[0];
        this.targetElement.appendChild(this.rootNode);
      } else {
        this.renderer.removeChild(this.rootNode.parentElement, this.rootNode);
        this.rootNode = null;
      }

      this._loading = newValue;
    }, 0);
  }

  @Input('abpLoadingTargetElement')
  targetElement: HTMLElement;

  componentRef: ComponentRef<LoadingComponent>;
  rootNode: HTMLDivElement;

  constructor(
    private elRef: ElementRef<HTMLElement>,
    private vcRef: ViewContainerRef,
    private cdRes: ComponentFactoryResolver,
    private injector: Injector,
    private renderer: Renderer2,
  ) {}

  ngOnInit() {
    if (!this.targetElement) {
      const { offsetHeight, offsetWidth } = this.elRef.nativeElement;
      if (!offsetHeight && !offsetWidth && this.elRef.nativeElement.children.length) {
        this.targetElement = this.elRef.nativeElement.children[0] as HTMLElement;
      } else {
        this.targetElement = this.elRef.nativeElement;
      }
    }
  }
}
