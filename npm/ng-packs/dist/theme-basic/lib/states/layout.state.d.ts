import { StateContext } from '@ngxs/store';
import { LayoutAddNavigationElement, LayoutRemoveNavigationElementByName } from '../actions/layout.actions';
import { Layout } from '../models/layout';
export declare class LayoutState {
    static getNavigationElements({ navigationElements }: Layout.State): Layout.NavigationElement[];
    layoutAddAction({ getState, patchState }: StateContext<Layout.State>, { payload }: LayoutAddNavigationElement): Layout.State;
    layoutRemoveAction({ getState, patchState }: StateContext<Layout.State>, { name }: LayoutRemoveNavigationElementByName): Layout.State;
}
