import { Injectable, effect, inject, signal } from "@angular/core";
import { Title } from "@angular/platform-browser";
import { TitleStrategy, RouterStateSnapshot } from "@angular/router";
import { ConfigStateService } from "./config-state.service";
import { LocalizationService } from "./localization.service";
import { tap } from "rxjs";
import { toSignal } from "@angular/core/rxjs-interop";

@Injectable({
    providedIn: 'root'
})
export class CustomTitleStrategy extends TitleStrategy {
    protected readonly title = inject(Title);
    protected readonly configState = inject(ConfigStateService);
    protected readonly localizationService = inject(LocalizationService);
    routerState: RouterStateSnapshot;
    projectName = toSignal(this.configState.getDeep$("localization.defaultResourceName"), { initialValue: "MyProjectName" });
    langugageChange = toSignal(this.localizationService.languageChange$);

    override updateTitle(routerState: RouterStateSnapshot) {
        let title = this.buildTitle(routerState);
        this.routerState = routerState;

        if (title === undefined) {
            this.title.setTitle(`${this.projectName()}`);
            return;
        }

        const localizedTitle = this.localizationService.instant({ key: title, defaultValue: title });
        this.title.setTitle(`${localizedTitle} | ${this.projectName()}`);
    }

    constructor() {
        super();
        effect(() => {
            if (this.langugageChange()) {
                this.updateTitle(this.routerState);
            }
        });
    }
}
// .pipe(tap(() => this.updateTitle(this.routerState))).subscribe();