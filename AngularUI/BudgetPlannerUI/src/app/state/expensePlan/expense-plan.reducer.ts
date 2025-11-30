import { createReducer, on } from '@ngrx/store';
import * as ExpensePlanActions from './expense-plan.actions';
import { ExpensePlanModel } from '../../models/ExpensePlanModel';

export interface ExpensePlanState {
    items: ExpensePlanModel[];
    loading: boolean;
    error: any;
}

export const initialState: ExpensePlanState = {
    items: [],
    loading: false,
    error: null,
};

// Utility helpers
const startLoading = () => ({
    loading: true,
    error: null
});

const finishLoading = () => ({
    loading: false,
    error: null
});

const setError = (error: any) => ({
    loading: false,
    error
});

export const expensePlanReducer = createReducer(
    initialState,

    // LOAD ALL
    on(ExpensePlanActions.loadExpensePlans, state => ({
        ...state,
        ...startLoading()
    })),

    on(ExpensePlanActions.loadExpensePlansSuccess, (state, { data }) => ({
        ...state,
        items: data,
        ...finishLoading()
    })),

    on(ExpensePlanActions.loadExpensePlansFailure, (state, { error }) => ({
        ...state,
        ...setError(error)
    })),

    // ADD
    on(ExpensePlanActions.addExpensePlan, state => ({
        ...state,
        ...startLoading()
    })),

    on(ExpensePlanActions.addExpensePlanSuccess, (state, { data }) => ({
        ...state,
        items: [...state.items, data],
        ...finishLoading()
    })),

    on(ExpensePlanActions.addExpensePlanFailure, (state, { error }) => ({
        ...state,
        ...setError(error)
    })),

    // UPDATE
    on(ExpensePlanActions.updateExpensePlan, state => ({
        ...state,
        ...startLoading()
    })),

    on(ExpensePlanActions.updateExpensePlanSuccess, (state, { data }) => ({
        ...state,
        items: state.items.map(item =>
            item.uniqueId === data.uniqueId ? data : item
        ),
        ...finishLoading()
    })),

    on(ExpensePlanActions.updateExpensePlanFailure, (state, { error }) => ({
        ...state,
        ...setError(error)
    })),

    // DELETE
    on(ExpensePlanActions.deleteExpensePlan, state => ({
        ...state,
        ...startLoading()
    })),

    on(ExpensePlanActions.deleteExpensePlanSuccess, (state, { id }) => ({
        ...state,
        items: state.items.filter(item => item.uniqueId !== id),
        ...finishLoading()
    })),

    on(ExpensePlanActions.deleteExpensePlanFailure, (state, { error }) => ({
        ...state,
        ...setError(error)
    })),

    // GET BY ID
    on(ExpensePlanActions.getExpensePlanById, state => ({
        ...state,
        ...startLoading()
    })),

    on(ExpensePlanActions.getExpensePlanByIdSuccess, (state, { data }) => {
        const exists = state.items.some(i => i.uniqueId === data.uniqueId);

        return {
            ...state,
            items: exists
                ? state.items.map(item =>
                    item.uniqueId === data.uniqueId ? data : item
                )
                : [...state.items, data],
            ...finishLoading()
        };
    }),

    on(ExpensePlanActions.getExpensePlanByIdFailure, (state, { error }) => ({
        ...state,
        ...setError(error)
    }))
);
