import { ControlContainer, FormGroupDirective } from "@angular/forms";

export const EXTENSIBLE_FORM_VIEW_PROVIDER = { provide: ControlContainer, useExisting: FormGroupDirective }
