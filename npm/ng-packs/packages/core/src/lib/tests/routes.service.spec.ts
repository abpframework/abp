import { take } from 'rxjs/operators';
import { RoutesService } from '../services';

const routes = [
  { path: '/foo', name: 'foo' },
  { path: '/foo/bar', name: 'bar', parentName: 'foo', invisible: true, order: 2 },
  { path: '/foo/bar/baz', name: 'baz', parentName: 'bar', order: 1 },
  { path: '/foo/x', name: 'x', parentName: 'foo', order: 1 },
];

describe('Routes Service', () => {
  describe('#add', () => {
    it('should add given routes as flat$, tree$, and visible$', async () => {
      const service = new RoutesService();
      service.add(routes);

      const flat = await service.flat$.pipe(take(1)).toPromise();
      const tree = await service.tree$.pipe(take(1)).toPromise();
      const visible = await service.visible$.pipe(take(1)).toPromise();

      expect(flat.length).toBe(4);
      expect(flat[0].name).toBe('foo');
      expect(flat[1].name).toBe('baz');
      expect(flat[2].name).toBe('x');
      expect(flat[3].name).toBe('bar');

      expect(tree.length).toBe(1);
      expect(tree[0].name).toBe('foo');
      expect(tree[0].children.length).toBe(2);
      expect(tree[0].children[0].name).toBe('x');
      expect(tree[0].children[1].name).toBe('bar');
      expect(tree[0].children[1].children[0].name).toBe('baz');

      expect(visible.length).toBe(1);
      expect(visible[0].name).toBe('foo');
      expect(visible[0].children.length).toBe(1);
      expect(visible[0].children[0].name).toBe('x');
    });
  });

  describe('#remove', () => {
    it('should remove routes based on given routeNames', () => {
      const service = new RoutesService();
      service.add(routes);
      service.remove(['bar']);

      const flat = service.flat;
      const tree = service.tree;
      const visible = service.visible;

      expect(flat.length).toBe(2);
      expect(flat[0].name).toBe('foo');
      expect(flat[1].name).toBe('x');

      expect(tree.length).toBe(1);
      expect(tree[0].name).toBe('foo');
      expect(tree[0].children.length).toBe(1);
      expect(tree[0].children[0].name).toBe('x');

      expect(visible.length).toBe(1);
      expect(visible[0].name).toBe('foo');
      expect(visible[0].children.length).toBe(1);
      expect(visible[0].children[0].name).toBe('x');
    });
  });

  describe('#patch', () => {
    it('should patch propeties of routes based on given routeNames', () => {
      const service = new RoutesService();
      service.add(routes);
      service.patch('x', { invisible: true });

      const flat = service.flat;
      const tree = service.tree;
      const visible = service.visible;

      expect(flat.length).toBe(4);
      expect(flat[0].name).toBe('foo');
      expect(flat[1].name).toBe('baz');
      expect(flat[2].name).toBe('x');
      expect(flat[3].name).toBe('bar');

      expect(tree.length).toBe(1);
      expect(tree[0].name).toBe('foo');
      expect(tree[0].children.length).toBe(2);
      expect(tree[0].children[0].name).toBe('x');
      expect(tree[0].children[1].name).toBe('bar');
      expect(tree[0].children[1].children[0].name).toBe('baz');

      expect(visible.length).toBe(1);
      expect(visible[0].name).toBe('foo');
      expect(visible[0].children.length).toBe(0);
    });

    it('should return false when route name is not found', () => {
      const service = new RoutesService();
      service.add(routes);
      const result = service.patch('A man has no name.', { invisible: true });
      expect(result).toBe(false);
    });
  });

  describe('#search', () => {
    it('should return node found when route name is not found', () => {
      const service = new RoutesService();
      service.add(routes);
      const result = service.search({ invisible: true });
      expect(result.name).toBe('bar');
      expect(result.children.length).toBe(1);
      expect(result.children[0].name).toBe('baz');
    });
  });
});
