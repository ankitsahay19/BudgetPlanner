import { Injectable, signal } from '@angular/core';
import { ApiEndpoints } from '../core/constants/api-endpoints';
import { IncomeSourceModel } from '../models/IncomeSourceModel';
import { Observable, tap } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { error } from 'console';

@Injectable({ providedIn: 'root' })
export class MonthlyIncomeService {

  constructor(private http: HttpClient) { }

  // State
  myIncomeSources = signal<IncomeSourceModel[]>([]);
  loadingIncomes = signal(false);
  errorMsg = signal('');
  successMsg = signal('');
  // New: selected income for edit
  selectedIncomeIdForEdit = signal<number | null>(null);

  // Load initial list
  getIncomeSources() {
    this.loadingIncomes.set(true);
    this.errorMsg.set('');
    this.successMsg.set('');

    this.http.get<IncomeSourceModel[]>(ApiEndpoints.IncomeSource.getAll).subscribe({
      next: (data) => {
        this.myIncomeSources.set(data);
        this.successMsg.set('Loaded successfully');
        this.loadingIncomes.set(false);
      },
      error: (err) => {
        this.errorMsg.set('Error loading income sources');
        this.loadingIncomes.set(false);
      }
    });
  }

  deleteIncomeSource(id: number): Observable<void> {
    const deleteUrl = ApiEndpoints.IncomeSource.delete(id);
    return this.http.delete<void>(deleteUrl).pipe(
      tap(() => {
        const updated = this.myIncomeSources().filter(x => x.uniqueId !== id);
        this.myIncomeSources.set(updated);
        this.successMsg.set('Deleted successfully');
      })
    );
  }

  // ✅ Add (append only when API responds successfully)
  addIncomeSource(income: IncomeSourceModel): Observable<IncomeSourceModel> {
    return this.http.post<IncomeSourceModel>(ApiEndpoints.IncomeSource.createOrEdit, income).pipe(
      tap((savedIncome) => {
        this.myIncomeSources.set([...this.myIncomeSources(), savedIncome]);
      })
    );
  }

  // ✅ Edit (replace existing record on success)
  editIncomeSource(income: IncomeSourceModel): Observable<IncomeSourceModel> {
    return this.http.put<IncomeSourceModel>(ApiEndpoints.IncomeSource.createOrEdit, income).pipe(
      tap((updatedIncome) => {
        const updatedList = this.myIncomeSources().map(item =>
          item.uniqueId === updatedIncome.uniqueId ? updatedIncome : item
        );
        this.myIncomeSources.set(updatedList);
      })
    );
  }


  setSelectedIncomeIdForEdit(incomeId: number) {
    this.selectedIncomeIdForEdit.set(incomeId);
    console.log(' from income service setSelectedIncomeIdForEdit with ID:', incomeId);
    //    return this.myIncomeSources().find(x => x.uniqueId === incomeId) || null;
  }
  getIncomeById(incomeId: number) {
    this.selectedIncomeIdForEdit.set(incomeId);
    const income = this.myIncomeSources().find(x => x.uniqueId === incomeId) || null;
    console.log(' from income service getIncomeById with ID:', incomeId, ' found income:', income);
    return income;
  }


}
