import { StateContext } from '@ngxs/store';
import { AddNavigationElement, RemoveNavigationElementByName } from '../actions/layout.actions';
import { Layout } from '../models/layout';
export declare class LayoutState {
    static getNavigationElements({ navigationElements }: Layout.State): Layout.NavigationElement[];
    layoutAddAction({ getState, patchState }: StateContext<Layout.State>, { payload }: AddNavigationElement): Layout.State;
    layoutRemoveAction({ getState, patchState }: StateContext<Layout.State>, { name }: RemoveNavigationElementByName): Layout.State;
}
