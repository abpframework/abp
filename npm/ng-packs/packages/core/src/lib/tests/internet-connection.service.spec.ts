import { TestBed} from '@angular/core/testing';
import { DOCUMENT } from '@angular/common';

import { InternetConnectionService } from '../services/internet-connection-service';
import { first } from 'rxjs';

let service: InternetConnectionService;

describe('Internet connection when disconnected', () => {
  const events = {};
  const addEventListener =  jest.fn((event, callback) => {
    events[event] = callback;
  });
  const mockDocument = { defaultView: {navigator: {onLine: false}, addEventListener } }
  beforeAll(() => {
    TestBed.configureTestingModule({
      providers:[{provide:DOCUMENT, useValue: mockDocument}]
    })
    service = TestBed.inject(InternetConnectionService);
  });

  it('document should be created', () => {
    expect(service.document).toEqual(mockDocument);
  });

  it('signal value should be false', () => {
    expect(service.networkStatus()).toEqual(false);
  });

  it('observable value should be false',
    (done: any) => {
    service.networkStatus$.pipe(first()).subscribe(value => {
      expect(value).toBe(false)
      done();
    });
  });

  test.each(['offline','online'])('should addEventListener for %p, event',(v)=>{
    expect(events[v]).toBeTruthy()
  })

  test.each([['offline',false],["online",true]])('when %p called ,then signal value must be %p',(eventName,value)=>{
    events[eventName]()
    expect(service.networkStatus()).toEqual(value);
  })

  test.each([['offline',false],["online",true]])('when %p called,then observable must return %p',(eventName,value)=>{
    events[eventName]()
    service.networkStatus$.subscribe(val=>{
      expect(val).toEqual(value)
    })
  })
});

describe('when connection value changes for signals', () => {
  const events = {};
  const addEventListener =  jest.fn((event, callback) => {
    events[event] = callback;
  });
  const mockDocument = { defaultView: {navigator: {onLine: false}, addEventListener } }
  beforeAll(() => {
    TestBed.configureTestingModule({
      providers:[{provide:DOCUMENT, useValue: mockDocument}]
    })
    service = TestBed.inject(InternetConnectionService);
  });

  it('signal value must be false when offline event is called while internet is connected', () => {
    events['online']()
    expect(service.networkStatus()).toEqual(true);
    events['offline']()
    expect(service.networkStatus()).toEqual(false);
  });

  it('signal value must be true when online event is called while internet is disconnected', () => {
    events['offline']()
    expect(service.networkStatus()).toEqual(false);
    events['online']()
    expect(service.networkStatus()).toEqual(true);
  });

  it('observable value must be false when offline event is called while internet is connected', (done:any) => {
    events['online']()
    events['offline']()
    service.networkStatus$.subscribe(val=>{
      expect(val).toEqual(false)
      done()
    })
  });

  it('observable value must be true when online event is called while internet is disconnected', (done:any) => {
    events['offline']()
    events['online']()
    service.networkStatus$.subscribe(val=>{
      console.log(val);
      expect(val).toEqual(true)
      done()
    })
  });
});