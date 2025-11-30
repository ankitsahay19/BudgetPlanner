import { Routes } from '@angular/router';

import { HomePage } from './pages/home-page/home-page';
import { AboutPage } from './pages/about-page/about-page';
import { NotFoundPage } from './pages/not-found-page/not-found-page';
import { RegisterPage } from './pages/register-page/register-page';
import { LoginPage } from './pages/login-page/login-page';
import { IncomePage } from './pages/income-page/income-page';
import { ExpensesPage } from './pages/expenses-page/expenses-page';
import { ExpensePlanPage } from './pages/expense-plan-page/expense-plan-page';

export const routes: Routes = [
    { path: '', component: HomePage },
    { path: 'home', component: HomePage },
    { path: 'about', component: AboutPage },

    { path: 'login', component: LoginPage },
    { path: 'register', component: RegisterPage },

    { path: 'income', component: IncomePage },
    { path: 'expenses', component: ExpensesPage },
    { path: 'expensePlan', component: ExpensePlanPage },

    { path: '**', component: NotFoundPage },
];
