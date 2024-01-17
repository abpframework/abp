import { AbpLocalStorageService } from "@abp/ng.core";
import { Injectable, Injector } from "@angular/core";

const rememberMe = 'remember_me';

@Injectable({
    providedIn: 'root'
})
export class RememberMeService {
    constructor(private injector: Injector) { }
    localStorageService = this.injector.get(AbpLocalStorageService);

    set(remember: boolean) {
        this.localStorageService.setItem(rememberMe, JSON.stringify(remember));
    }

    remove() {
        this.localStorageService.removeItem(rememberMe);
    }

    get() {
        return Boolean(JSON.parse(this.localStorageService.getItem(rememberMe)));
    }
}