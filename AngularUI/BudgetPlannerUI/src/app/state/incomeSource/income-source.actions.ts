// import { createAction, props } from '@ngrx/store';
// import { IncomeSource } from '../../../model/income-source.model';
import { createAction, props } from '@ngrx/store';
import { IncomeSourceModel } from '../../models/IncomeSourceModel';

// Load
export const loadIncomeSources = createAction('[IncomeSource] Load');
export const loadIncomeSourcesSuccess = createAction('[IncomeSource] Load Success', props<{ data: IncomeSourceModel[] }>());

// Add
export const addIncomeSource = createAction('[IncomeSource] Add', props<{ data: IncomeSourceModel }>());
export const addIncomeSourceSuccess = createAction('[IncomeSource] Add Success', props<{ data: IncomeSourceModel }>());

// Update
export const updateIncomeSource = createAction('[IncomeSource] Update', props<{ data: IncomeSourceModel }>());
export const updateIncomeSourceSuccess = createAction('[IncomeSource] Update Success', props<{ data: IncomeSourceModel }>());

// Delete
export const deleteIncomeSource = createAction('[IncomeSource] Delete', props<{ id: number }>());
export const deleteIncomeSourceSuccess = createAction('[IncomeSource] Delete Success', props<{ id: number }>());
