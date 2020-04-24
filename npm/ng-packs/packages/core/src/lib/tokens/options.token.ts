import { InjectionToken } from '@angular/core';
import { ABP } from '../models/common';

export const CORE_OPTIONS = new InjectionToken<ABP.Root>('CORE_OPTIONS');
