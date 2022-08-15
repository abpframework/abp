import {ePropType, FormProp} from "@abp/ng.theme.shared/extensions";
import {UpdateProfileDto} from "@abp/ng.account.core/proxy";
import {Validators} from "@angular/forms";
import {HelloComponent} from "../components/hello/hello.component";

export const DEFAULT_PERSONAL_SETTINGS_UPDATE_FORM_PROPS = FormProp.createMany<UpdateProfileDto>([
  {
    type: ePropType.String,
    name: 'userName',
    displayName: 'Account::UserName',
    id: 'user-name',
    validators: () => [Validators.required, Validators.maxLength(256)],
    template: HelloComponent
  },
])
