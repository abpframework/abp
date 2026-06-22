/* eslint-disable */
import { describe, it, expect } from '@jest/globals';
import { Routes } from '@angular/router';
import { createRoutes } from '../cms-kit-admin.routes';
import { CmsKitAdminConfigOptions } from '../models';

describe('cms-kit-admin routes', () => {
  function findRoute(routes: Routes, path: string): any {
    for (const route of routes) {
      if (route.path === path) {
        return route;
      }
      if (route.children) {
        const found = findRoute(route.children, path);
        if (found) {
          return found;
        }
      }
    }
    return null;
  }

  it('should create base route with children', () => {
    const routes = createRoutes();

    expect(Array.isArray(routes)).toBe(true);
    const root = routes[0];
    expect(root.path).toBe('');
    expect(root.children?.length).toBeGreaterThan(0);
  });

  it('should contain expected admin routes with required policies', () => {
    const routes = createRoutes();

    const comments = findRoute(routes, 'comments');
    const pages = findRoute(routes, 'pages');
    const blogs = findRoute(routes, 'blogs');
    const blogPosts = findRoute(routes, 'blog-posts');
    const menus = findRoute(routes, 'menus');
    const globalResources = findRoute(routes, 'global-resources');

    expect(comments?.data?.requiredPolicy).toBe('CmsKit.Comments');
    expect(pages?.data?.requiredPolicy).toBe('CmsKit.Pages');
    expect(blogs?.data?.requiredPolicy).toBe('CmsKit.Blogs');
    expect(blogPosts?.data?.requiredPolicy).toBe('CmsKit.BlogPosts');
    expect(menus?.data?.requiredPolicy).toBe('CmsKit.Menus');
    expect(globalResources?.data?.requiredPolicy).toBe('CmsKit.GlobalResources');
  });

  it('should propagate contributors from config options', () => {
    const options: CmsKitAdminConfigOptions = {
      entityActionContributors: {},
      entityPropContributors: {},
      toolbarActionContributors: {},
      createFormPropContributors: {},
      editFormPropContributors: {},
    };

    const routes = createRoutes(options);
    const root = routes[0];

    expect(root.providers).toBeDefined();
  });
});
