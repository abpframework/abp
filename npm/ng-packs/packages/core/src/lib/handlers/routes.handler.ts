import { Injectable, Optional } from '@angular/core';
import { Router } from '@angular/router';
import { RoutesService } from '../services/routes.service';

@Injectable({
  providedIn: 'root',
})
export class RoutesHandler {
  constructor(private routes: RoutesService, @Optional() private router: Router) {
    this.addRoutes();
  }

  addRoutes() {
    this.router?.config.forEach(({ path, data }) => {
      if (!data) return;

      if (data.route) {
        this.routes.add([{ path: '/' + path, ...data.route }]);
        return;
      }

      if (data.routes) this.routes.add(data.routes);
    });
  }
}
