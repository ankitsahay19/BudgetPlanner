import { createAction, props } from '@ngrx/store';
import { ExpenseModel } from '../../models/ExpenseModel';

export const loadExpenses = createAction('[Expenses] Load Expenses');
export const loadExpensesSuccess = createAction('[Expenses] Load Expenses Success', props<{ data: ExpenseModel[] }>());
export const loadExpensesFailure = createAction('[Expenses] Load Expenses Failure', props<{ error: any }>());

export const addExpense = createAction('[Expenses] Add Expense', props<{ data: ExpenseModel }>());
export const addExpenseSuccess = createAction('[Expenses] Add Expense Success', props<{ data: ExpenseModel }>());
export const addExpenseFailure = createAction('[Expenses] Add Expense Failure', props<{ error: any }>());

export const updateExpense = createAction('[Expenses] Update Expense', props<{ data: ExpenseModel }>());
export const updateExpenseSuccess = createAction('[Expenses] Update Expense Success', props<{ data: ExpenseModel }>());
export const updateExpenseFailure = createAction('[Expenses] Update Expense Failure', props<{ error: any }>());

export const deleteExpense = createAction('[Expenses] Delete Expense', props<{ id: number }>());
export const deleteExpenseSuccess = createAction('[Expenses] Delete Expense Success', props<{ id: number }>());
export const deleteExpenseFailure = createAction('[Expenses] Delete Expense Failure', props<{ error: any }>());
