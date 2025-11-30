// Add missing selectors for compatibility
export const selectExpensePlanState = createFeatureSelector<ExpensePlanState>('expensePlan');
export const selectAllExpensePlans = createSelector(selectExpensePlanState, state => state.items);
export const selectExpensePlanLoading = createSelector(selectExpensePlanState, state => state.loading);
import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ExpensePlanState } from './expense-plan.reducer';

// Returns a nested structure: parents with subExpensePlans[]
export const selectNestedExpensePlans = createSelector(
    selectAllExpensePlans,
    (items) => {
        const parents = items.filter(item => !item.parentId || item.parentId === 0);
        return parents.map(parent => ({
            ...parent,
            subExpensePlans: items.filter(child => child.parentId === parent.uniqueId)
        }));
    }
);
