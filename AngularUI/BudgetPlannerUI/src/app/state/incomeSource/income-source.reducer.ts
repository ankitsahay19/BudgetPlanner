import { createReducer, on } from '@ngrx/store';
import * as IncomeSourceActions from './income-source.actions';
import { IncomeSourceModel } from '../../models/IncomeSourceModel';
export interface IncomeSourceState { data: IncomeSourceModel[]; loading: boolean; }
export const initialState: IncomeSourceState = { data: [], loading: false };

export const incomeSourceReducer = createReducer(initialState,
    on(IncomeSourceActions.loadIncomeSources, state => ({ ...state, loading: true })),
    on(IncomeSourceActions.loadIncomeSourcesSuccess, (state, { data }) => ({ ...state, loading: false, data })),

    on(IncomeSourceActions.addIncomeSourceSuccess, (state, { data }) => ({ ...state, data: [...state.data, data] })),

    on(IncomeSourceActions.updateIncomeSourceSuccess, (state, { data }) => ({ ...state, data: state.data.map(x => x.uniqueId === data.uniqueId ? data : x) })),

    on(IncomeSourceActions.deleteIncomeSourceSuccess, (state, { id }) => ({ ...state, data: state.data.filter(x => x.uniqueId !== id) }))
);
