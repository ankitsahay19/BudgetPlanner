import { inject, Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
// import { ExpensePlanService } from '../../services/expense-plan.service';
import * as ExpensePlanActions from './expense-plan.actions';
import { catchError, map, mergeMap, of } from 'rxjs';
import { ApiEndpoints } from '../../core/constants/api-endpoints';
import { HttpClient } from '@angular/common/http';
import { ExpensePlanModel } from '../../models/ExpensePlanModel';


@Injectable()
export class ExpensePlanEffects {
    private actions$ = inject(Actions);
    private http = inject(HttpClient);
    load$ = createEffect(() =>
        this.actions$.pipe(
            ofType(ExpensePlanActions.loadExpensePlans),
            mergeMap(() =>
                this.http.get<ExpensePlanModel[]>(ApiEndpoints.ExpensePlan.getAll).pipe(
                    map(data => ExpensePlanActions.loadExpensePlansSuccess({ data })),
                    catchError(() => of({ type: '[ExpensePlan] Load Failed' }))
                )
            )
        )
    );


    add$ = createEffect(() =>
        this.actions$.pipe(
            ofType(ExpensePlanActions.addExpensePlan),
            mergeMap(action =>
                this.http.post<ExpensePlanModel>(ApiEndpoints.ExpensePlan.create, action.data).pipe(
                    map(data => ExpensePlanActions.addExpensePlanSuccess({ data })),
                    catchError(error => of(ExpensePlanActions.addExpensePlanFailure({ error })))
                )
            )
        )
    );

    update$ = createEffect(() =>
        this.actions$.pipe(
            ofType(ExpensePlanActions.updateExpensePlan),
            mergeMap(action =>
                this.http.put<ExpensePlanModel>(ApiEndpoints.ExpensePlan.edit + '/' + action.data.uniqueId, action.data).pipe(
                    map(data => ExpensePlanActions.updateExpensePlanSuccess({ data })),
                    catchError(error => of(ExpensePlanActions.updateExpensePlanFailure({ error })))
                )
            )
        )
    );

    delete$ = createEffect(() =>
        this.actions$.pipe(
            ofType(ExpensePlanActions.deleteExpensePlan),
            mergeMap(action =>
                this.http.delete(ApiEndpoints.ExpensePlan.getById(action.id)).pipe(
                    map(() => ExpensePlanActions.deleteExpensePlanSuccess({ id: action.id })),
                    catchError(error => of(ExpensePlanActions.deleteExpensePlanFailure({ error })))
                )
            )
        )
    );

    getById$ = createEffect(() =>
        this.actions$.pipe(
            ofType(ExpensePlanActions.getExpensePlanById),
            mergeMap(action =>
                this.http.get<ExpensePlanModel>(ApiEndpoints.ExpensePlan.getById(action.id)).pipe(
                    map(data => ExpensePlanActions.getExpensePlanByIdSuccess({ data })),
                    catchError(error => of(ExpensePlanActions.getExpensePlanByIdFailure({ error })))
                )
            )
        )
    );
}
