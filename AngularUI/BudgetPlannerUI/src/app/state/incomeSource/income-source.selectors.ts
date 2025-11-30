import { createFeatureSelector, createSelector } from '@ngrx/store';
import { IncomeSourceState } from './income-source.reducer';

export const selectIncomeSourceState = createFeatureSelector<IncomeSourceState>('incomeSource');
export const selectAllIncomeSources = createSelector(selectIncomeSourceState, state => state.data);
export const selectLoading = createSelector(selectIncomeSourceState, state => state.loading);
