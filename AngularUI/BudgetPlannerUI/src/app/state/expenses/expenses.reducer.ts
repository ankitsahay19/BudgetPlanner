import { createReducer, on } from '@ngrx/store';
import * as ExpensesActions from './expenses.actions';
import { ExpenseModel } from '../../models/ExpenseModel';

export interface ExpensesState {
    items: ExpenseModel[];
    loading: boolean;
    error: any;
}

export const initialState: ExpensesState = {
    items: [],
    loading: false,
    error: null,
};

export const expensesReducer = createReducer(
    initialState,
    on(ExpensesActions.loadExpenses, state => ({ ...state, loading: true })),
    on(ExpensesActions.loadExpensesSuccess, (state, { data }) => ({ ...state, items: data, loading: false })),
    on(ExpensesActions.loadExpensesFailure, (state, { error }) => ({ ...state, error, loading: false })),

    on(ExpensesActions.addExpense, state => ({ ...state, loading: true })),
    on(ExpensesActions.addExpenseSuccess, (state, { data }) => ({ ...state, items: [...state.items, data], loading: false })),
    on(ExpensesActions.addExpenseFailure, (state, { error }) => ({ ...state, error, loading: false })),

    on(ExpensesActions.updateExpense, state => ({ ...state, loading: true })),
    on(ExpensesActions.updateExpenseSuccess, (state, { data }) => ({
        ...state,
        items: state.items.map(item => item.uniqueId === data.uniqueId ? data : item),
        loading: false
    })),
    on(ExpensesActions.updateExpenseFailure, (state, { error }) => ({ ...state, error, loading: false })),

    on(ExpensesActions.deleteExpense, state => ({ ...state, loading: true })),
    on(ExpensesActions.deleteExpenseSuccess, (state, { id }) => ({
        ...state,
        items: state.items.filter(item => item.uniqueId !== id),
        loading: false
    })),
    on(ExpensesActions.deleteExpenseFailure, (state, { error }) => ({ ...state, error, loading: false })),
);
