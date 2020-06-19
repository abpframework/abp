import { ABP, ConfigState, RoutesService, takeUntilDestroy, TreeNode } from '@abp/ng.core';
import { Component, Input, OnDestroy, TrackByFunction } from '@angular/core';
import { Store } from '@ngxs/store';
import compare from 'just-compare';
import { distinctUntilChanged, skip } from 'rxjs/operators';

@Component({
  selector: 'abp-routes',
  templateUrl: 'routes.component.html',
})
export class RoutesComponent implements OnDestroy {
  @Input() smallScreen: boolean;

  isShow = true;

  trackByFn: TrackByFunction<TreeNode<ABP.Route>> = (_, item) => item.name;

  constructor(public readonly routes: RoutesService, private store: Store) {
    this.store
      .select(ConfigState.getDeep('auth.grantedPolicies'))
      .pipe(distinctUntilChanged(compare), skip(1), takeUntilDestroy(this))
      .subscribe(() => {
        this.isShow = false;
        setTimeout(() => (this.isShow = true), 0);
      });
  }

  ngOnDestroy() {}
}
