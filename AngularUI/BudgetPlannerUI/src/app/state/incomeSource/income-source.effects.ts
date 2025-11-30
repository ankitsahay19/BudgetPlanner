import { inject, Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { HttpClient } from '@angular/common/http';
import { of } from 'rxjs';
import { map, mergeMap, catchError, tap } from 'rxjs/operators';
import * as IncomeSourceActions from './income-source.actions';
import { IncomeSourceModel } from '../../models/IncomeSourceModel';
import { ApiEndpoints } from '../../core/constants/api-endpoints';

@Injectable()
export class IncomeSourceEffects {
    private apiUrl = ApiEndpoints.IncomeSource;
    private actions$ = inject(Actions);
    private http = inject(HttpClient);

    constructor() {
        console.log('IncomeSourceEffects initialized with API URL:', this.apiUrl);
    }
    load$ = createEffect(() =>
        this.actions$.pipe(
            ofType(IncomeSourceActions.loadIncomeSources),
            mergeMap(() =>
                this.http.get<IncomeSourceModel[]>(this.apiUrl.getAll).pipe(
                    map(data => IncomeSourceActions.loadIncomeSourcesSuccess({ data })),
                    catchError(() => of({ type: '[IncomeSource] Load Failed' }))
                )
            )
        )
    );

    add$ = createEffect(() =>
        this.actions$.pipe(
            ofType(IncomeSourceActions.addIncomeSource),
            mergeMap(action =>
                this.http.post<IncomeSourceModel>(this.apiUrl.create, action.data).pipe(
                    map(data => IncomeSourceActions.addIncomeSourceSuccess({ data })),
                    catchError(() => of({ type: '[IncomeSource] Add Failed' }))
                )
            )
        )
    );

    update$ = createEffect(() =>
        this.actions$.pipe(
            ofType(IncomeSourceActions.updateIncomeSource),
            mergeMap(action =>
                this.http.put<IncomeSourceModel>(`${this.apiUrl.edit}/${action.data.uniqueId}`, action.data).pipe(
                    map(data => IncomeSourceActions.updateIncomeSourceSuccess({ data })),
                    catchError(() => of({ type: '[IncomeSource] Update Failed' }))
                )
            )
        )
    );


    delete$ = createEffect(() =>
        this.actions$.pipe(
            ofType(IncomeSourceActions.deleteIncomeSource),
            tap(action => console.log('[Delete Request]', `${this.apiUrl.delete(action.id)}`, 'ID:', action.id)),
            mergeMap(action =>
                this.http.delete(`${this.apiUrl.delete(action.id)}`).pipe(
                    map(() => IncomeSourceActions.deleteIncomeSourceSuccess({ id: action.id })),
                    catchError(error => {
                        console.error('[Delete Failed]', error);
                        return of({ type: '[IncomeSource] Delete Failed' });
                    })
                )
            )
        )
    );
}
