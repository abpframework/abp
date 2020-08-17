import { Spectator, createComponentFactory, SpyObject } from '@ngneat/spectator/jest';
import { Component } from '@angular/core';
import { ConfigStateService, getShortDateFormat, getShortDateShortTimeFormat } from '@abp/ng.core';

const dateTimeFormat = {
  calendarAlgorithmType: 'SolarCalendar',
  dateSeparator: '/',
  dateTimeFormatLong: 'dddd, MMMM d, yyyy',
  fullDateTimePattern: 'dddd, MMMM d, yyyy h:mm:ss tt',
  longTimePattern: 'h:mm:ss tt',
  shortDatePattern: 'M/d/yyyy',
  shortTimePattern: 'h:mm tt',
};

@Component({
  selector: 'abp-dummy',
  template: 'dummy',
})
class DummyComponent {}

describe('Date Utils', () => {
  let spectator: Spectator<DummyComponent>;
  let config: SpyObject<ConfigStateService>;

  const createComponent = createComponentFactory({
    component: DummyComponent,
    mocks: [ConfigStateService],
  });

  beforeEach(() => {
    spectator = createComponent();
    config = spectator.inject(ConfigStateService);
  });

  describe('#getShortDateFormat', () => {
    test('should get the short date format from ConfigState and return it', () => {
      const getDeepSpy = jest.spyOn(config, 'getDeep');
      getDeepSpy.mockReturnValueOnce(dateTimeFormat);

      expect(getShortDateFormat(config)).toBe('M/d/yyyy');
      expect(getDeepSpy).toHaveBeenCalledWith('localization.currentCulture.dateTimeFormat');
    });
  });

  describe('#getShortDateShortTimeFormat', () => {
    test('should get the short date time format from ConfigState and return it', () => {
      const getDeepSpy = jest.spyOn(config, 'getDeep');
      getDeepSpy.mockReturnValueOnce(dateTimeFormat);

      expect(getShortDateShortTimeFormat(config)).toBe('M/d/yyyy h:mm a');
      expect(getDeepSpy).toHaveBeenCalledWith('localization.currentCulture.dateTimeFormat');
    });
  });
});
