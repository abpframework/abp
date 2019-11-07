import { Action, Selector, State, StateContext } from '@ngxs/store';
import { SetSelectedSettingTab } from '../actions/setting-management.actions';
import { SettingManagement } from '../models/setting-management';

@State<SettingManagement.State>({
  name: 'SettingManagementState',
  defaults: { selectedTab: {} } as SettingManagement.State,
})
export class SettingManagementState {
  @Selector()
  static getSelectedTab({ selectedTab }: SettingManagement.State) {
    return selectedTab;
  }

  @Action(SetSelectedSettingTab)
  settingManagementAction({ patchState }: StateContext<SettingManagement.State>, { payload }: SetSelectedSettingTab) {
    patchState({
      selectedTab: payload,
    });
  }
}
