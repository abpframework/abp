import { ConfigStateService } from '../services';
import { getShortDateFormat, getShortTimeFormat, getShortDateShortTimeFormat } from '../utils';

const dateTimeFormat = {
  calendarAlgorithmType: 'SolarCalendar',
  dateSeparator: '/',
  dateTimeFormatLong: 'dddd, MMMM d, yyyy',
  fullDateTimePattern: 'dddd, MMMM d, yyyy h:mm:ss tt',
  longTimePattern: 'h:mm:ss tt',
  shortDatePattern: 'M/d/yyyy',
  shortTimePattern: 'h:mm tt',
};

describe('Date Utils', () => {
  let config: ConfigStateService;

  beforeEach(() => {
    config = new ConfigStateService();
  });

  describe('#getShortDateFormat', () => {
    test('should get the short date format from ConfigState and return it', () => {
      const getDeepSpy = jest.spyOn(config, 'getDeep');
      getDeepSpy.mockReturnValueOnce(dateTimeFormat);

      expect(getShortDateFormat(config)).toBe('M/d/yyyy');
      expect(getDeepSpy).toHaveBeenCalledWith('localization.currentCulture.dateTimeFormat');
    });
  });

  describe('#getShortTimeFormat', () => {
    test('should get the short time format from ConfigState and return it', () => {
      const getDeepSpy = jest.spyOn(config, 'getDeep');
      getDeepSpy.mockReturnValueOnce(dateTimeFormat);

      expect(getShortTimeFormat(config)).toBe('h:mm a');
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
