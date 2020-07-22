import { of, Subscription, timer } from 'rxjs';
import { SubscriptionService } from '../services/subscription.service';

describe('SubscriptionService', () => {
  let service: SubscriptionService;

  beforeEach(() => {
    service = new SubscriptionService();
  });

  afterEach(() => {
    service['subscription'].unsubscribe();
  });

  describe('#addOne', () => {
    it('should subscribe to given observable with next and error functions and return the Subscription instance', () => {
      const next = jest.fn();
      const error = jest.fn();
      const subscription = service.addOne(of(null), next, error);
      expect(subscription).toBeInstanceOf(Subscription);
      expect(next).toHaveBeenCalledWith(null);
      expect(next).toHaveBeenCalledTimes(1);
      expect(error).not.toHaveBeenCalled();
    });

    it('should subscribe to given observable with observer and return the Subscription instance', () => {
      const observer = { next: jest.fn(), complete: jest.fn() };
      const subscription = service.addOne(of(null), observer);
      expect(subscription).toBeInstanceOf(Subscription);
      expect(observer.next).toHaveBeenCalledWith(null);
      expect(observer.next).toHaveBeenCalledTimes(1);
      expect(observer.complete).toHaveBeenCalledTimes(1);
    });
  });

  describe('#isClosed', () => {
    it('should return true if subscriptions are alive and false if not', () => {
      service.addOne(timer(1000), () => {});
      expect(service.isClosed).toBe(false);

      service['subscription'].unsubscribe();
      expect(service.isClosed).toBe(true);
    });
  });

  describe('#closeAll', () => {
    it('should close all subscriptions and the parent subscription', () => {
      const sub1 = service.addOne(timer(1000), () => {});
      const sub2 = service.addOne(timer(1000), () => {});

      expect(sub1.closed).toBe(false);
      expect(sub2.closed).toBe(false);
      expect(service.isClosed).toBe(false);

      service.closeAll();

      expect(sub1.closed).toBe(true);
      expect(sub2.closed).toBe(true);
      expect(service.isClosed).toBe(true);
    });
  });

  describe('#reset', () => {
    it('should close all subscriptions but not the parent subscription', () => {
      const sub1 = service.addOne(timer(1000), () => {});
      const sub2 = service.addOne(timer(1000), () => {});

      expect(sub1.closed).toBe(false);
      expect(sub2.closed).toBe(false);
      expect(service.isClosed).toBe(false);

      service.reset();

      expect(sub1.closed).toBe(true);
      expect(sub2.closed).toBe(true);
      expect(service.isClosed).toBe(false);
    });
  });

  describe('#closeOne', () => {
    it('should unsubscribe from given subscription only', () => {
      const sub1 = service.addOne(timer(1000), () => {});
      const sub2 = service.addOne(timer(1000), () => {});
      expect(service.isClosed).toBe(false);

      service.closeOne(sub1);
      expect(sub1.closed).toBe(true);
      expect(service.isClosed).toBe(false);

      service.closeOne(sub2);
      expect(sub2.closed).toBe(true);
      expect(service.isClosed).toBe(false);
    });
  });

  describe('#removeOne', () => {
    it('should remove given subscription from list of subscriptions', () => {
      const sub1 = service.addOne(timer(1000), () => {});
      const sub2 = service.addOne(timer(1000), () => {});
      expect(service.isClosed).toBe(false);

      service.removeOne(sub1);
      expect(sub1.closed).toBe(false);
      expect(service.isClosed).toBe(false);

      sub1.unsubscribe();
    });
  });
});
