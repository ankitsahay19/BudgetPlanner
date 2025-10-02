import { Injectable } from '@angular/core';
import { ApiEndpoints } from '../core/constants/api-endpoints';
import { IncomeSourceModel } from '../models/IncomeSourceModel';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class MonthlyIncomeService {
  constructor(private http: HttpClient) { }
  private apiUrl = ApiEndpoints.IncomeSource.getAll;

  getIncomeSources(): Observable<IncomeSourceModel[]> {
    return this.http.get<IncomeSourceModel[]>(this.apiUrl);
  }
  DeleteIncomeSource(id: number): Observable<IncomeSourceModel[]> {
    return this.http.delete<IncomeSourceModel[]>(`${this.apiUrl}/${id}`);
  }

  addIncomeSource(income: IncomeSourceModel): Observable<IncomeSourceModel> {
    return this.http.post<IncomeSourceModel>(ApiEndpoints.IncomeSource.createOrEdit, income);
  }

}
