import { DOM_STRATEGY, DomStrategy } from '../strategies/dom.strategy';

describe('DomStrategy', () => {
  describe('#insertElement', () => {
    it('should append element to head by default', () => {
      const strategy = new DomStrategy(() => document.head);
      const element = document.createElement('script');
      strategy.insertElement(element);

      expect(document.head.lastChild).toBe(element);
    });

    it('should append element to body when body is given as target', () => {
      const strategy = new DomStrategy(() => document.body);
      const element = document.createElement('script');
      strategy.insertElement(element);

      expect(document.body.lastChild).toBe(element);
    });

    it('should prepend to head when position is given as "afterbegin"', () => {
      const strategy = new DomStrategy(() => document.head, 'afterbegin');
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
    name               | target           | position        | hasArg
    ${'AfterElement'}  | ${div}           | ${'afterend'}   | ${true}
    ${'AppendToBody'}  | ${document.body} | ${'beforeend'}  | ${false}
    ${'AppendToHead'}  | ${document.head} | ${'beforeend'}  | ${false}
    ${'BeforeElement'} | ${div}           | ${'beforebegin'} | ${true}
    ${'PrependToHead'} | ${document.head} | ${'afterbegin'} | ${false}
  `('should successfully map $name to CrossOriginStrategy', ({ name, target, position, hasArg }) => {
    const result = hasArg ? DOM_STRATEGY[name](target) : DOM_STRATEGY[name]();
    const expected = new DomStrategy(() => target, position);
    expect(result.position).toBe(expected.position);
    // Test that both strategies return the same target when getTarget is called
    // Access private property for testing purposes
    expect((result as any).getTarget()).toBe((expected as any).getTarget());
  });
});
