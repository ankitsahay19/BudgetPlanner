import { Component, effect, inject, signal } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { MonthlyIncomeService } from '../../../../services/monthly-income-service';
import { IncomeSourceModel } from '../../../../models/IncomeSourceModel';

@Component({
  selector: 'app-add-edit-income-component',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './add-edit-income-component.html',
  styleUrl: './add-edit-income-component.scss',
})
export class AddEditIncomeComponent {
  /**
   * Controls fade-out animation for error message
   */
  /**
   * Controls fade-out animation for error message
   */
  errorFading = false;

  /**
   * Reactive form for add/edit income
   */
  incomeForm = inject(FormBuilder).group({
    sourceName: ['', Validators.required],
    incomeAmount: [0, [Validators.required, Validators.min(1)]],
    uniqueId: [0],
    userId: [0],
  });

  /**
   * Signals for UI state
   */
  savingIncome = signal(false);
  successMsg = signal('');
  errorMsg = signal('');

  /**
   * MonthlyIncomeService for API and state
   */
  public incomeService = inject(MonthlyIncomeService);

  /**
   * Populate form when selected income changes
   */
  constructor() {
    effect(() => {
      const incomeId = this.incomeService.selectedIncomeIdForEdit();
      const income = incomeId ? this.incomeService.getIncomeById(incomeId) : null;
      if (income) {
        this.incomeForm.patchValue({
          sourceName: income.sourceName,
          incomeAmount: income.incomeAmount,
          uniqueId: income.uniqueId,
          userId: income.userId,
        });
      } else {
        this.incomeForm.reset({ sourceName: '', incomeAmount: 0, uniqueId: 0, userId: 0 });
      }
    });
  }

  /**
   * Handles form submission for add/edit income
   * Shows loader and error/success feedback
   */
  onSubmit() {
    if (this.incomeForm.invalid) return;
    this.savingIncome.set(true);
    const formValue = this.incomeForm.value as IncomeSourceModel;
    // Ensure userId and uniqueId are numbers
    formValue.userId = formValue.userId ?? Number(localStorage.getItem('userId') || '0');
    formValue.uniqueId = formValue.uniqueId ?? 0;

    // Determine if adding or editing
    const request$ = formValue.uniqueId && formValue.uniqueId > 0
      ? this.incomeService.editIncomeSource(formValue)
      : this.incomeService.addIncomeSource(formValue);

    request$.subscribe({
      next: () => {
        this.successMsg.set('Income saved successfully!');
        this.errorMsg.set('');
        this.savingIncome.set(false);
        this.incomeForm.reset({ sourceName: '', incomeAmount: 0, uniqueId: 0, userId: 0 });
        this.incomeService.selectedIncomeIdForEdit.set(null);
      },
      error: () => {
        this.errorMsg.set('Error saving income.');
        this.successMsg.set('');
        this.savingIncome.set(false);
        this.errorFading = false;
        setTimeout(() => {
          this.errorFading = true;
        }, 500);
        setTimeout(() => {
          this.errorMsg.set('');
          this.errorFading = false;
        }, 3000);
      },
    });
  }


}
