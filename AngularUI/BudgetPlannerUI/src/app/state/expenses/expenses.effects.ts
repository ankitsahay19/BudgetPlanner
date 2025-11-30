import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import * as ExpensesActions from './expenses.actions';
import { catchError, map, mergeMap, of } from 'rxjs';

@Injectable()
export class ExpensesEffects {
    load$ = createEffect(() =>
        this.actions$.pipe(
            ofType(ExpensesActions.loadExpenses),
            mergeMap(() =>
                of([]).pipe( // Replace with actual API call if needed
                    map(data => ExpensesActions.loadExpensesSuccess({ data })),
                    catchError(error => of(ExpensesActions.loadExpensesFailure({ error })))
                )
            )
        )
    );

    add$ = createEffect(() =>
        this.actions$.pipe(
            ofType(ExpensesActions.addExpense),
            mergeMap(action =>
                of(action.data).pipe( // Replace with actual API call if needed
                    map(data => ExpensesActions.addExpenseSuccess({ data })),
                    catchError(error => of(ExpensesActions.addExpenseFailure({ error })))
                )
            )
        )
    );

    update$ = createEffect(() =>
        this.actions$.pipe(
            ofType(ExpensesActions.updateExpense),
            mergeMap(action =>
                of(action.data).pipe( // Replace with actual API call if needed
                    map(data => ExpensesActions.updateExpenseSuccess({ data })),
                    catchError(error => of(ExpensesActions.updateExpenseFailure({ error })))
                )
            )
        )
    );

    delete$ = createEffect(() =>
        this.actions$.pipe(
            ofType(ExpensesActions.deleteExpense),
            mergeMap(action =>
                of(action.id).pipe( // Replace with actual API call if needed
                    map(() => ExpensesActions.deleteExpenseSuccess({ id: action.id })),
                    catchError(error => of(ExpensesActions.deleteExpenseFailure({ error })))
                )
            )
        )
    );

    constructor(private actions$: Actions) { }
}
