import { TestBed } from '@angular/core/testing';
import { DOCUMENT } from '@angular/common';

import { InternetConnectionService } from '../services/internet-connection-service';
import { first, firstValueFrom, skip } from 'rxjs';

let service: InternetConnectionService;

describe('Internet connection when disconnected', () => {
  const events = {};
  const addEventListener = vi.fn((event, callback) => {
    events[event] = callback;
  });
  const mockDocument = { defaultView: { navigator: { onLine: false }, addEventListener } };
  beforeAll(() => {
    TestBed.configureTestingModule({
      providers: [{ provide: DOCUMENT, useValue: mockDocument }],
    });
    service = TestBed.inject(InternetConnectionService);
  });

  it('document should be created', () => {
    expect(service.document).toEqual(mockDocument);
  });

  it('signal value should be false', () => {
    expect(service.networkStatus()).toEqual(false);
  });

  it('observable value should be false', async () => {
    const value = await firstValueFrom(service.networkStatus$.pipe(first()));
    expect(value).toBe(false);
  });

  test.each(['offline', 'online'])('should addEventListener for %p, event', v => {
    expect(events[v]).toBeTruthy();
  });

  test.each([
    ['offline', false],
    ['online', true],
  ])('when %p called ,then signal value must be %p', (eventName, value) => {
    events[eventName]();
    expect(service.networkStatus()).toEqual(value);
  });

  test.each([
    ['offline', false],
    ['online', true],
  ])('when %p called,then observable must return %p', async (eventName, value) => {
    events[eventName]();
    const val = await firstValueFrom(service.networkStatus$);
    expect(val).toEqual(value);
  });
});

describe('when connection value changes for signals', () => {
  const events = {};
  const addEventListener = vi.fn((event, callback) => {
    events[event] = callback;
  });
  const mockDocument = { defaultView: { navigator: { onLine: false }, addEventListener } };
  beforeAll(() => {
    TestBed.configureTestingModule({
      providers: [{ provide: DOCUMENT, useValue: mockDocument }],
    });
    service = TestBed.inject(InternetConnectionService);
  });

  it('signal value must be false when offline event is called while internet is connected', () => {
    events['online']();
    expect(service.networkStatus()).toEqual(true);
    events['offline']();
    expect(service.networkStatus()).toEqual(false);
  });

  it('signal value must be true when online event is called while internet is disconnected', () => {
    events['offline']();
    expect(service.networkStatus()).toEqual(false);
    events['online']();
    expect(service.networkStatus()).toEqual(true);
  });

  it('observable value must be false when offline event is called while internet is connected', async () => {
    events['online']();
    // Get current value after online event
    const onlineVal = await firstValueFrom(service.networkStatus$);
    expect(onlineVal).toEqual(true);

    // Subscribe and skip the current value, then trigger offline event
    const offlinePromise = firstValueFrom(service.networkStatus$.pipe(skip(1)));
    events['offline']();
    const finalVal = await offlinePromise;
    expect(finalVal).toEqual(false);
  });

  it('observable value must be true when online event is called while internet is disconnected', async () => {
    events['offline']();
    // Get current value after offline event
    const offlineVal = await firstValueFrom(service.networkStatus$);
    expect(offlineVal).toEqual(false);

    // Subscribe and skip the current value, then trigger online event
    const onlinePromise = firstValueFrom(service.networkStatus$.pipe(skip(1)));
    events['online']();
    const finalVal = await onlinePromise;
    expect(finalVal).toEqual(true);
  });
});
