import { StateContext } from '@ngxs/store';
import { LayoutAddNavigationElement } from '../actions/layout.actions';
import { Layout } from '../models/layout';
export declare class LayoutState {
    static getNavigationElements({ navigationElements }: Layout.State): Layout.NavigationElement[];
    layoutAction({ getState, patchState }: StateContext<Layout.State>, { payload }: LayoutAddNavigationElement): Layout.State;
}
