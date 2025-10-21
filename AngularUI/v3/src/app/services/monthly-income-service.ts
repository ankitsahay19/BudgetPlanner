import { Injectable, signal } from '@angular/core';
import { ApiEndpoints } from '../core/constants/api-endpoints';
import { IncomeSourceModel } from '../models/IncomeSourceModel';
import { Observable, tap } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class MonthlyIncomeService {
  // Dependency injection for HTTP client
  constructor(private http: HttpClient) { }

  // --- Signals for state management ---
  /** List of all income sources for the user */
  myIncomeSources = signal<IncomeSourceModel[]>([]);
  /** Loading state for API requests */
  loadingIncomes = signal(false);
  /** Error message for UI display */
  errorMsg = signal('');
  /** Success message for UI display */
  successMsg = signal('');
  /** Currently selected income ID for editing */
  selectedIncomeIdForEdit = signal<number | null>(null);

  /**
   * Loads all income sources from the API and updates state signals.
   */
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

  /**
   * Deletes an income source by ID and updates the state.
   * @param id Unique ID of the income source to delete
   */
  deleteIncomeSource(id: number): Observable<void> {
    const deleteUrl = ApiEndpoints.IncomeSource.delete(id);
    return this.http.delete<void>(deleteUrl).pipe(
      tap(() => {
        // Remove deleted item from local state
        const updated = this.myIncomeSources().filter(x => x.uniqueId !== id);
        this.myIncomeSources.set(updated);
        this.successMsg.set('Deleted successfully');
      })
    );
  }

  /**
   * Sets the selected income ID for editing.
   * @param incomeId Unique ID of the income to edit
   */
  setSelectedIncomeIdForEdit(incomeId: number) {
    this.selectedIncomeIdForEdit.set(incomeId);
    console.log('Set selected income ID for edit:', incomeId);
  }

  /**
   * Gets an income source by ID from the local state.
   * @param incomeId Unique ID of the income to retrieve
   * @returns The found IncomeSourceModel or null
   */
  getIncomeById(incomeId: number): IncomeSourceModel | null {
    this.selectedIncomeIdForEdit.set(incomeId);
    const income = this.myIncomeSources().find(x => x.uniqueId === incomeId) || null;
    console.log('Get income by ID:', incomeId, 'Found:', income);
    return income;
  }

  // Create new income
  addIncomeSource(income: IncomeSourceModel): Observable<IncomeSourceModel> {
    return this.http.post<IncomeSourceModel>(ApiEndpoints.IncomeSource.create, income)
      .pipe(
        tap(savedIncome => {
          this.myIncomeSources.set([...this.myIncomeSources(), savedIncome]);
        })
      );
  }

  // Update existing income
  editIncomeSource(income: IncomeSourceModel): Observable<IncomeSourceModel> {
    return this.http.put<IncomeSourceModel>(`${ApiEndpoints.IncomeSource.edit}/${income.uniqueId}`, income)
      .pipe(
        tap(updatedIncome => {
          const updatedList = this.myIncomeSources().map(item =>
            item.uniqueId === updatedIncome.uniqueId ? updatedIncome : item
          );
          this.myIncomeSources.set(updatedList);
        })
      );
  }



  // // ✅ Add (append only when API responds successfully)
  // addIncomeSource(income: IncomeSourceModel): Observable<IncomeSourceModel> {
  //   return this.http.post<IncomeSourceModel>(ApiEndpoints.IncomeSource.createOrEdit, income).pipe(
  //     tap((savedIncome) => {
  //       const list = this.myIncomeSources();

  //       if (income.uniqueId && income.uniqueId !== 0) {
  //         // 🟢 Edit case — replace existing
  //         const updatedList = list.map(item =>
  //           item.uniqueId === income.uniqueId ? savedIncome : item
  //         );
  //         this.myIncomeSources.set(updatedList);
  //       } else {
  //         // 🟢 Add case — append new
  //         this.myIncomeSources.set([...list, savedIncome]);
  //       }

  //     })
  //   );
  // } 


}
