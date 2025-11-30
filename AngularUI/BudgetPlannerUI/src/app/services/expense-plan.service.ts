// import { Injectable } from '@angular/core';
// import { HttpClient } from '@angular/common/http';
// import { Observable } from 'rxjs';
// import { ExpensePlanModel } from '../models/ExpensePlanModel';
// import { ApiEndpoints } from '../core/constants/api-endpoints';


// @Injectable({ providedIn: 'root' })
// export class ExpensePlanService {
//   private apiUrl = ApiEndpoints.Categories.getAllCategories;

//   constructor(private http: HttpClient) { }

//   getExpensePlans(): Observable<ExpensePlanModel[]> {
//     return this.http.get<ExpensePlanModel[]>(ApiEndpoints.Categories.getAllCategories);
//   }

//   getExpensePlan(id: number): Observable<ExpensePlanModel> {
//     return this.http.get<ExpensePlanModel>(ApiEndpoints.Categories.getCategoriesById(id));
//   }

//   createExpensePlan(expensePlan: ExpensePlanModel): Observable<ExpensePlanModel> {
//     return this.http.post<ExpensePlanModel>(ApiEndpoints.Categories.SaveCategories, expensePlan);
//   }

//   updateExpensePlan(id: number, expensePlan: ExpensePlanModel): Observable<ExpensePlanModel> {
//     return this.http.put<ExpensePlanModel>(`${this.apiUrl}/${id}`, expensePlan);
//   }

//   deleteExpensePlan(id: number): Observable<void> {
//     return this.http.delete<void>(`${this.apiUrl}/${id}`);
//   }
// }
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ExpensePlanModel } from '../models/ExpensePlanModel';
import { ApiEndpoints } from '../core/constants/api-endpoints';


@Injectable({ providedIn: 'root' })
export class ExpensePlanService {
  private apiUrl = ApiEndpoints.Categories.getAllCategories;

  constructor(private http: HttpClient) { }

  getExpensePlans(): Observable<ExpensePlanModel[]> {
    return this.http.get<ExpensePlanModel[]>(ApiEndpoints.Categories.getAllCategories);
  }

  getExpensePlan(id: number): Observable<ExpensePlanModel> {
    return this.http.get<ExpensePlanModel>(ApiEndpoints.Categories.getCategoriesById(id));
  }

  createExpensePlan(expensePlan: ExpensePlanModel): Observable<ExpensePlanModel> {
    return this.http.post<ExpensePlanModel>(ApiEndpoints.Categories.SaveCategories, expensePlan);
  }

  updateExpensePlan(id: number, expensePlan: ExpensePlanModel): Observable<ExpensePlanModel> {
    return this.http.put<ExpensePlanModel>(`${this.apiUrl}/${id}`, expensePlan);
  }

  deleteExpensePlan(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
