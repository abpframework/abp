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

  test.each(['offline',"online"])('should addEventListener for "%i",event',(v)=>{
    expect(events[v]).toBeTruthy()
  })

  test.each([['offline',false],["online",true]])('should called %i, then value must be %i',(eventName,value)=>{
    events[eventName]()
    expect(service.networkStatus()).toEqual(value);
  })

});

describe('Internet connection when connected', () => {
  const mockDocument = { defaultView: {navigator: {onLine: true}, addEventListener: jest.fn()} }

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers:[{provide:DOCUMENT, useValue: mockDocument}]
    })
    service = TestBed.inject(InternetConnectionService);
  });

  it('signal value should be true', () => {
    expect(service.networkStatus()).toEqual(true);
  });

  it('observable value should be true',
    (done: any) => {
    service.networkStatus$.pipe(first()).subscribe(value => {
      expect(value).toBe(true)
      done();
    });
  });
});

describe('when connection value changes', () => {
  const mockDocument = { defaultView: {navigator: {onLine: true}, addEventListener: (eventName,cb)=>{
    window.addEventListener(eventName,cb)
  }, dispatchEvent: function(){
    this.navigator.onLine = !this.navigator.onLine
  }}}

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers:[{provide:DOCUMENT, useValue: mockDocument}]
    })
    service = TestBed.inject(InternetConnectionService);
  });

  it('check is signal value changes', () => {
    expect(service.networkStatus()).toEqual(true);
    const event = new Event("offline");
    service.window.dispatchEvent(event)
    service.setStatus(service.window.navigator.onLine)
    expect(service.networkStatus()).toEqual(false);
  });

  it('check is observable value changes',(done: any) => {
    const event = new Event("online");
    service.window.dispatchEvent(event)
    service.networkStatus$.pipe(first()).subscribe(value => {
      expect(value).toEqual(true)
      done()
    });
    service.window.dispatchEvent(event)
    service.setStatus(service.window.navigator.onLine)
    service.networkStatus$.pipe().subscribe(value => {
      expect(value).toEqual(false)
      done()
    });
  });
});
