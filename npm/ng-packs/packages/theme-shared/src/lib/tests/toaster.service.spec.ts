import { ContentProjectionService } from '@abp/ng.core';
import { ComponentRef } from '@angular/core';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator/vitest';
import { beforeEach, describe, expect, test, vi } from 'vitest';
import { ToastContainerComponent } from '../components/toast-container/toast-container.component';
import { ToasterService } from '../services/toaster.service';

describe('ToasterService', () => {
  let spectator: SpectatorService<ToasterService>;
  let service: ToasterService;
  const mockComponentRef = {
    changeDetectorRef: { detectChanges: vi.fn() },
    instance: {} as ToastContainerComponent,
  } as unknown as ComponentRef<ToastContainerComponent>;

  const contentProjectionService = {
    projectContent: vi.fn().mockReturnValue(mockComponentRef),
  } satisfies Partial<ContentProjectionService>;
  
  const createService = createServiceFactory({
    service: ToasterService,
    providers: [{ provide: ContentProjectionService, useValue: contentProjectionService }],
  });

  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
  });

  test('should create service', () => {
    expect(service).toBeTruthy();
  });

  test('should have show method', () => {
    expect(typeof service.show).toBe('function');
  });

  test('should have info method', () => {
    expect(typeof service.info).toBe('function');
  });

  test('should have success method', () => {
    expect(typeof service.success).toBe('function');
  });

  test('should have warn method', () => {
    expect(typeof service.warn).toBe('function');
  });

  test('should have error method', () => {
    expect(typeof service.error).toBe('function');
  });

  test('should have remove method', () => {
    expect(typeof service.remove).toBe('function');
  });

  test('should have clear method', () => {
    expect(typeof service.clear).toBe('function');
  });

  test('should call show method without error', () => {
    expect(() => service.show('MESSAGE', 'TITLE')).not.toThrow();
  });

  test('should call info method without error', () => {
    expect(() => service.info('MESSAGE', 'TITLE')).not.toThrow();
  });

  test('should call success method without error', () => {
    expect(() => service.success('MESSAGE', 'TITLE')).not.toThrow();
  });

  test('should call warn method without error', () => {
    expect(() => service.warn('MESSAGE', 'TITLE')).not.toThrow();
  });

  test('should call error method without error', () => {
    expect(() => service.error('MESSAGE', 'TITLE')).not.toThrow();
  });

  test('should call remove method without error', () => {
    expect(() => service.remove(0)).not.toThrow();
  });

  test('should call clear method without error', () => {
    expect(() => service.clear()).not.toThrow();
  });
});
