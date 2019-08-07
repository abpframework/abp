import { Component } from '@angular/core';
import { slideFromBottom } from '@abp/ng.theme.shared';

@Component({
  selector: ' abp-layout',
  templateUrl: './layout.component.html',
  animations: [slideFromBottom],
})
export class LayoutComponent {
  isCollapsed: boolean = false;
}
