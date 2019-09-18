import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { LazyLoadService } from '@abp/ng.core';
import styles from '../constants/styles';

@Injectable({ providedIn: 'root' })
export class InitialService {
  constructor(private lazyLoadService: LazyLoadService) {
    this.appendStyle().subscribe();
  }

  appendStyle() {
    return this.lazyLoadService.load(null, 'style', styles, 'head', 'afterbegin');
  }
}
