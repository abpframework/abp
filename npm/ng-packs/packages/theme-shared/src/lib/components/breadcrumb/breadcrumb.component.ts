import { ABP, getRoutePath, RoutesService, takeUntilDestroy, TreeNode } from '@abp/ng.core';
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { filter, map, startWith } from 'rxjs/operators';
import { eThemeSharedRouteNames } from '../../enums';

@Component({
  selector: 'abp-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BreadcrumbComponent implements OnDestroy, OnInit {
  segments: Partial<ABP.Route>[] = [];

  constructor(
    public readonly cdRef: ChangeDetectorRef,
    private router: Router,
    private routes: RoutesService,
  ) {}

  ngOnDestroy() {}

  ngOnInit(): void {
    this.router.events
      .pipe(
        takeUntilDestroy(this),
        filter<NavigationEnd>(event => event instanceof NavigationEnd),
        // tslint:disable-next-line:deprecation
        startWith(null),
        map(() => this.routes.search({ path: getRoutePath(this.router) })),
      )
      .subscribe(route => {
        this.segments = [];
        if (route) {
          let node = { parent: route } as TreeNode<ABP.Route>;

          while (node.parent) {
            node = node.parent;
            const { parent, children, isLeaf, ...segment } = node;
            if (!isAdministration(segment)) this.segments.unshift(segment);
          }

          this.cdRef.detectChanges();
        }
      });
  }
}

function isAdministration(route: ABP.Route) {
  return route.name === eThemeSharedRouteNames.Administration;
}
