import { RenderMode, ServerRoute } from '@angular/ssr';

export const appServerRoutes: ServerRoute[] = [
  {
    path: '**',
    renderMode: RenderMode.Server,
  }
];
