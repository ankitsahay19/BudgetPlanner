import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ExpensesState } from './expenses.reducer';

export const selectExpensesState = createFeatureSelector<ExpensesState>('expenses');

export const selectAllExpenses = createSelector(
    selectExpensesState,
    state => state.items
);

export const selectExpensesLoading = createSelector(
    selectExpensesState,
    state => state.loading
);

export const selectExpensesError = createSelector(
    selectExpensesState,
    state => state.error
);
