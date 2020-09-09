import { DomStrategy, DOM_STRATEGY } from '../strategies';

describe('DomStrategy', () => {
  describe('#insertElement', () => {
    it('should append element to head by default', () => {
      const strategy = new DomStrategy();
      const element = document.createElement('script');
      strategy.insertElement(element);

      expect(document.head.lastChild).toBe(element);
    });

    it('should append element to body when body is given as target', () => {
      const strategy = new DomStrategy(document.body);
      const element = document.createElement('script');
      strategy.insertElement(element);

      expect(document.body.lastChild).toBe(element);
    });

    it('should prepend to head when position is given as "afterbegin"', () => {
      const strategy = new DomStrategy(undefined, 'afterbegin');
      const element = document.createElement('script');
      strategy.insertElement(element);

      expect(document.head.firstChild).toBe(element);
    });
  });
});

describe('DOM_STRATEGY', () => {
  const div = document.createElement('DIV');

  beforeEach(() => {
    document.body.innerHTML = '';
    document.body.appendChild(div);
  });

  test.each`
    name               | target           | position
    ${'AfterElement'}  | ${div}           | ${'afterend'}
    ${'AppendToBody'}  | ${document.body} | ${'beforeend'}
    ${'AppendToHead'}  | ${document.head} | ${'beforeend'}
    ${'BeforeElement'} | ${div}           | ${'beforebegin'}
    ${'PrependToHead'} | ${document.head} | ${'afterbegin'}
  `('should successfully map $name to CrossOriginStrategy', ({ name, target, position }) => {
    expect(DOM_STRATEGY[name](target)).toEqual(new DomStrategy(target, position));
  });
});
