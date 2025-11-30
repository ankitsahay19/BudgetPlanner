export const getExpensePlanById = createAction('[ExpensePlan] Get ExpensePlan By Id', props<{ id: number }>());
export const getExpensePlanByIdSuccess = createAction('[ExpensePlan] Get ExpensePlan By Id Success', props<{ data: ExpensePlanModel }>());
export const getExpensePlanByIdFailure = createAction('[ExpensePlan] Get ExpensePlan By Id Failure', props<{ error: any }>());
import { createAction, props } from '@ngrx/store';
import { ExpensePlanModel } from '../../models/ExpensePlanModel';

export const loadExpensePlans = createAction('[ExpensePlan] Load ExpensePlans');
export const loadExpensePlansSuccess = createAction('[ExpensePlan] Load ExpensePlans Success', props<{ data: ExpensePlanModel[] }>());
export const loadExpensePlansFailure = createAction('[ExpensePlan] Load ExpensePlans Failure', props<{ error: any }>());

export const addExpensePlan = createAction('[ExpensePlan] Add ExpensePlan', props<{ data: ExpensePlanModel }>());
export const addExpensePlanSuccess = createAction('[ExpensePlan] Add ExpensePlan Success', props<{ data: ExpensePlanModel }>());
export const addExpensePlanFailure = createAction('[ExpensePlan] Add ExpensePlan Failure', props<{ error: any }>());

export const updateExpensePlan = createAction('[ExpensePlan] Update ExpensePlan', props<{ data: ExpensePlanModel }>());
export const updateExpensePlanSuccess = createAction('[ExpensePlan] Update ExpensePlan Success', props<{ data: ExpensePlanModel }>());
export const updateExpensePlanFailure = createAction('[ExpensePlan] Update ExpensePlan Failure', props<{ error: any }>());

export const deleteExpensePlan = createAction('[ExpensePlan] Delete ExpensePlan', props<{ id: number }>());
export const deleteExpensePlanSuccess = createAction('[ExpensePlan] Delete ExpensePlan Success', props<{ id: number }>());
export const deleteExpensePlanFailure = createAction('[ExpensePlan] Delete ExpensePlan Failure', props<{ error: any }>());
