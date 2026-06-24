import { createServiceFactory, SpectatorService } from '@ngneat/spectator/vitest';
import { UtcToLocalPipe } from '../pipes/utc-to-local.pipe';
import { ConfigStateService, LocalizationService, TimeService, TimezoneService } from '../services';

describe('UtcToLocalPipe', () => {
  let spectator: SpectatorService<UtcToLocalPipe>;
  let pipe: UtcToLocalPipe;

  const createService = createServiceFactory({
    service: UtcToLocalPipe,
    providers: [
      TimeService,
      {
        provide: TimezoneService,
        useValue: { isUtcClockEnabled: true, timezone: 'Europe/Lisbon' },
      },
      {
        provide: ConfigStateService,
        useValue: {
          getDeep: () => ({ shortDatePattern: 'yyyy-MM-dd', shortTimePattern: 'HH:mm' }),
        },
      },
      { provide: LocalizationService, useValue: {} },
    ],
  });

  beforeEach(() => {
    spectator = createService();
    pipe = spectator.service;
  });

  it('should apply daylight saving time when UTC clock is enabled', () => {
    // Europe/Lisbon is UTC+1 in July (DST)
    expect(pipe.transform('2025-07-25T12:00:00Z', 'time')).toBe('13:00');
  });

  it('should use standard offset outside daylight saving time', () => {
    // Europe/Lisbon is UTC+0 in January (no DST)
    expect(pipe.transform('2025-01-15T12:00:00Z', 'time')).toBe('12:00');
  });

  it('should roll the date over when the DST offset crosses midnight', () => {
    // 23:30 UTC in July becomes 00:30 the next day in Europe/Lisbon (UTC+1)
    expect(pipe.transform('2025-07-25T23:30:00Z', 'date')).toBe('2025-07-26');
    expect(pipe.transform('2025-07-25T23:30:00Z', 'datetime')).toBe('2025-07-26 00:30');
  });
});
