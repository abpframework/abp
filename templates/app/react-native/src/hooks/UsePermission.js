import { useEffect, useState } from 'react';
import { store } from '../store';
import { createGrantedPolicySelector } from '../store/selectors/AppSelectors';

export function usePermission(key) {
  const [permission, setPermission] = useState(false);

  const state = store.getState();
  const policy = createGrantedPolicySelector(key)(state);

  useEffect(() => {
    setPermission(policy);
  }, [policy]);

  return permission;
}
