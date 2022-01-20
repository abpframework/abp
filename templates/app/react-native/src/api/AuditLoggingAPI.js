import api from './API';

export function getErrorRateStatistics(params = {}) {
  return api
    .get('/api/audit-logging/audit-logs/statistics/error-rate', { params })
    .then(({ data }) => data);
}
