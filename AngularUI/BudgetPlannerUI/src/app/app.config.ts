import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection, isDevMode } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { UserAccountServiceInterceptor } from './Interceptor/user-account-interceptor';
import { provideState, provideStore } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { provideStoreDevtools } from '@ngrx/store-devtools';
import { incomeSourceReducer } from './state/incomeSource/income-source.reducer';
import { IncomeSourceEffects } from './state/incomeSource/income-source.effects';
import { expensePlanReducer } from './state/expensePlan/expense-plan.reducer';
import { ExpensePlanEffects } from './state/expensePlan/expense-plan.effects';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withInterceptorsFromDi()), { provide: HTTP_INTERCEPTORS, useClass: UserAccountServiceInterceptor, multi: true },
    provideStore(),
    provideState('incomeSource', incomeSourceReducer),
    provideState('expensePlan', expensePlanReducer),
    provideBrowserGlobalErrorListeners(), // Enable global error handling
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideEffects([IncomeSourceEffects, ExpensePlanEffects]),
    provideStoreDevtools({ maxAge: 25, logOnly: !isDevMode() }),
    provideRouter(routes)
  ]
};