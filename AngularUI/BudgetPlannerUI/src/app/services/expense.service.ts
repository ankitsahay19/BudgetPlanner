import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ExpenseModel } from '../models/ExpenseModel';
import { ApiEndpoints } from '../core/constants/api-endpoints';

@Injectable({ providedIn: 'root' })
export class ExpenseService {


  constructor(private http: HttpClient) {}

  createOrEditExpense(expense: ExpenseModel): Observable<ExpenseModel> {
    return this.http.post<ExpenseModel>(ApiEndpoints.Expenses.createOrEdit, expense);
  }

  getExpenses(): Observable<ExpenseModel[]> {
    return this.http.get<ExpenseModel[]>(ApiEndpoints.Expenses.getAll);
  }

  getExpense(id: number): Observable<ExpenseModel> {
    return this.http.get<ExpenseModel>(ApiEndpoints.Expenses.getById(id));
  }

  deleteExpense(id: number): Observable<void> {
    return this.http.delete<void>(ApiEndpoints.Expenses.delete(id));
  }
}
