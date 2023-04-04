import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { SafeHtmlPipe } from '../pipes';

describe('SafeHtmlPipe', () => {
  let pipe: SafeHtmlPipe;
  let spectator: SpectatorService<SafeHtmlPipe>;
  const createService = createServiceFactory(SafeHtmlPipe);

  beforeEach(() => {
    spectator = createService();
    pipe = spectator.service;
  });

  it('should create an instance', () => {
    expect(pipe).toBeTruthy();
  });

  test.each([42, false, {}, []])('should return empty string for "%p" input', input => {
    const result = pipe.transform(input as any);
    expect(result).toBe('');
  });

  it('should be equals with input after sanitized', () => {
    const input = '<div><h1>Hello world!</h1></div>';
    const result = pipe.transform(input);
    expect(result).toEqual(input);
  });

  it('should sanitize unsafe HTML content', () => {
    const input = `<script>alert("hello world");</script><p><a href='#' onclick="alert('This is an XSS attack!')">Click here!</a></p>`;
    const result = pipe.transform(input);
    expect(result).toBe(`<p><a href="#">Click here!</a></p>`);
  });
});
