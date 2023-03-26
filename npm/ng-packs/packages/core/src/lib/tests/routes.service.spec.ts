import { Subject, lastValueFrom } from 'rxjs';
import { take } from 'rxjs/operators';
import { RoutesService } from '../services/routes.service';
import { DummyInjector } from './utils/common.utils';
import { mockPermissionService } from './utils/permission-service.spec.utils';

const updateStream$ = new Subject<void>();

export const mockRoutesService = (injectorPayload = {} as { [key: string]: any }) => {
  const injector = new DummyInjector({
    PermissionService: mockPermissionService(),
    ConfigStateService: { createOnUpdateStream: () => updateStream$ },
    OTHERS_GROUP: 'OthersGroup',
    ...injectorPayload,
  });
  return new RoutesService(injector);
};

describe('Routes Service', () => {
  let service: RoutesService;

  const fooGroup = 'FooGroup';
  const barGroup = 'BarGroup';
  const othersGroup = 'OthersGroup';

  const routes = [
    { path: '/foo', name: 'foo' },
    { path: '/foo/bar', name: 'bar', parentName: 'foo', invisible: true, order: 2 },
    { path: '/foo/bar/baz', name: 'baz', parentName: 'bar', order: 1 },
    { path: '/foo/bar/baz/qux', name: 'qux', parentName: 'baz', order: 1 },
    { path: '/foo/x', name: 'x', parentName: 'foo', order: 1 },
  ];

  const groupedRoutes = [
    { path: '/foo', name: 'foo', group: fooGroup },
    { path: '/foo/y', name: 'y', parentName: 'foo' },
    { path: '/foo/bar', name: 'bar', group: barGroup },
    { path: '/foo/bar/baz', name: 'baz', group: barGroup },
    { path: '/foo/z', name: 'z' },
  ];

  beforeEach(() => {
    service = mockRoutesService();
  });

  describe('#add', () => {
    it('should add given routes as flat$, tree$, and visible$', async () => {
      service.add(routes);

      const flat = await lastValueFrom(service.flat$.pipe(take(1)));
      const tree = await lastValueFrom(service.tree$.pipe(take(1)));
      const visible = await lastValueFrom(service.visible$.pipe(take(1)));

      expect(flat.length).toBe(5);
      expect(flat[0].name).toBe('baz');
      expect(flat[1].name).toBe('qux');
      expect(flat[2].name).toBe('x');
      expect(flat[3].name).toBe('bar');
      expect(flat[4].name).toBe('foo');

      expect(tree.length).toBe(1);
      expect(tree[0].name).toBe('foo');
      expect(tree[0].children.length).toBe(2);
      expect(tree[0].children[0].name).toBe('x');
      expect(tree[0].children[1].name).toBe('bar');
      expect(tree[0].children[1].children[0].name).toBe('baz');
      expect(tree[0].children[1].children[0].children[0].name).toBe('qux');

      expect(visible.length).toBe(1);
      expect(visible[0].name).toBe('foo');
      expect(visible[0].children.length).toBe(1);
      expect(visible[0].children[0].name).toBe('x');
    });
  });

  describe('#groupedVisible', () => {
    it('should return undefined when there are no visible routes', async () => {
      service.add(routes);
      const result = await lastValueFrom(service.groupedVisible$.pipe(take(1)));
      expect(result).toBeUndefined();
    });

    it(
      'should group visible routes under "' + othersGroup + '" when no group is specified',
      async () => {
        service.add([
          { path: '/foo', name: 'foo' },
          { path: '/foo/bar', name: 'bar', group: '' },
          { path: '/foo/bar/baz', name: 'baz', group: undefined },
          { path: '/x', name: 'y', group: 'z' },
        ]);

        const result = await lastValueFrom(service.groupedVisible$.pipe(take(1)));

        expect(result[0].group).toBe(othersGroup);
        expect(result[0].items[0].name).toBe('foo');
        expect(result[0].items[1].name).toBe('bar');
        expect(result[0].items[2].name).toBe('baz');
      },
    );

    it('should return grouped route list', async () => {
      service.add(groupedRoutes);

      const tree = await lastValueFrom(service.groupedVisible$.pipe(take(1)));

      expect(tree.length).toBe(3);

      expect(tree[0].group).toBe('FooGroup');
      expect(tree[0].items[0].name).toBe('foo');
      expect(tree[0].items[0].children[0].name).toBe('y');

      expect(tree[1].group).toBe('BarGroup');
      expect(tree[1].items[0].name).toBe('bar');
      expect(tree[1].items[1].name).toBe('baz');

      expect(tree[2].group).toBe(othersGroup);
      expect(tree[2].items[0].name).toBe('z');
    });
  });

  describe('#find', () => {
    it('should return node found based on query', () => {
      service.add(routes);
      const result = service.find(route => route.invisible);
      expect(result.name).toBe('bar');
      expect(result.children.length).toBe(1);
      expect(result.children[0].name).toBe('baz');
    });

    it('should return null when query is not found', () => {
      service.add(routes);
      const result = service.find(route => route.requiredPolicy === 'X');
      expect(result).toBe(null);
    });
  });

  describe('#hasChildren', () => {
    it('should return if node has invisible child', () => {
      service.add(routes);

      expect(service.hasChildren('foo')).toBe(true);
      expect(service.hasChildren('bar')).toBe(true);
      expect(service.hasChildren('baz')).toBe(true);
      expect(service.hasChildren('qux')).toBe(false);
    });
  });

  describe('#hasInvisibleChild', () => {
    it('should return if node has invisible child', () => {
      service.add(routes);

      expect(service.hasInvisibleChild('foo')).toBe(true);
      expect(service.hasInvisibleChild('bar')).toBe(false);
      expect(service.hasInvisibleChild('baz')).toBe(false);
    });
  });

  describe('#remove', () => {
    it('should remove routes based on given routeNames', () => {
      service.add(routes);
      service.remove(['bar']);

      const flat = service.flat;
      const tree = service.tree;
      const visible = service.visible;

      expect(flat.length).toBe(2);
      expect(flat[1].name).toBe('foo');
      expect(flat[0].name).toBe('x');

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
      service['isGranted'] = jest.fn(route => route.requiredPolicy !== 'X');
      service.add(routes);
      service.patch('x', { requiredPolicy: 'X' });

      const flat = service.flat;
      const tree = service.tree;
      const visible = service.visible;

      expect(flat.length).toBe(5);
      expect(flat[0].name).toBe('baz');
      expect(flat[1].name).toBe('qux');
      expect(flat[2].name).toBe('x');
      expect(flat[3].name).toBe('bar');
      expect(flat[4].name).toBe('foo');

      expect(tree.length).toBe(1);
      expect(tree[0].name).toBe('foo');
      expect(tree[0].children.length).toBe(2);
      expect(tree[0].children[0].name).toBe('x');
      expect(tree[0].children[1].name).toBe('bar');
      expect(tree[0].children[1].children[0].name).toBe('baz');
      expect(tree[0].children[1].children[0].children[0].name).toBe('qux');

      expect(visible.length).toBe(1);
      expect(visible[0].name).toBe('foo');
      expect(visible[0].children.length).toBe(0);
    });

    it('should return false when route name is not found', () => {
      service.add(routes);
      const result = service.patch('A man has no name.', { invisible: true });
      expect(result).toBe(false);
    });
  });

  describe('#refresh', () => {
    it('should call add once with empty array', () => {
      const add = jest.spyOn(service, 'add');
      service.refresh();
      expect(add).toHaveBeenCalledTimes(1);
      expect(add).toHaveBeenCalledWith([]);
    });

    it('should be called upon successful GetAppConfiguration action', () => {
      const refresh = jest.spyOn(service, 'refresh');
      updateStream$.next();
      expect(refresh).toHaveBeenCalledTimes(1);
    });
  });

  describe('#search', () => {
    it('should return node found based on query', () => {
      service.add(routes);
      const result = service.search({ invisible: true });
      expect(result.name).toBe('bar');
      expect(result.children.length).toBe(1);
      expect(result.children[0].name).toBe('baz');
    });

    it('should return null when query is not found', () => {
      service.add(routes);
      const result = service.search({ requiredPolicy: 'X' });
      expect(result).toBe(null);
    });
  });
});
