import { Component, effect, inject, Injector, signal, runInInjectionContext } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule, } from '@angular/forms';
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
  private injector = inject(Injector);
  private fb = inject(FormBuilder);
  public incomeService = inject(MonthlyIncomeService);

  // Signals
  savingIncome = signal(false);
  successMsg = signal('');
  errorMsg = signal('');

  // Reactive form
  incomeForm = this.fb.group({
    sourceName: ['', Validators.required],
    incomeAmount: [0, [Validators.required, Validators.min(1)]],
    uniqueId: [0],
    userId: [0],
  });

  constructor() {
    // ✅ Run the effect safely inside an injection context
    runInInjectionContext(this.injector, () => {
      effect(() => {
        const incomeId = this.incomeService.selectedIncomeIdForEdit();
        console.log('Effect triggered: selected ID =', incomeId);
        const income = incomeId ? this.incomeService.getIncomeById(incomeId) : null;
        if (income) {
          console.log('Populating form for edit:', income);
          this.incomeForm.patchValue({
            sourceName: income.sourceName,
            incomeAmount: income.incomeAmount,
            uniqueId: income.uniqueId,
            userId: income.userId,
          });
        } else {
          console.log('Resetting form (no income selected)');
          this.incomeForm.reset({ sourceName: '', incomeAmount: 0, uniqueId: 0, userId: 0, });
        }
      });
    });
  }

  onSubmit() {
    if (this.incomeForm.invalid) return;
    this.savingIncome.set(true);
    const formValue = this.incomeForm.value as IncomeSourceModel;
    if (formValue.userId === null) formValue.userId = Number(localStorage.getItem('userId') || '0');
    if (formValue.uniqueId === null) formValue.uniqueId = Number(localStorage.getItem('uniqueId') || '0');

    // Determine if adding or editing based on presence of uniqueId{
    const request$ = formValue.uniqueId && formValue.uniqueId > 0
      ? this.incomeService.editIncomeSource(formValue)
      : this.incomeService.addIncomeSource(formValue);

    request$.subscribe({
      next: () => {
        this.successMsg.set(`Income saved successfully!`);
        this.errorMsg.set('');
        this.savingIncome.set(false);
        this.incomeForm.reset({ sourceName: '', incomeAmount: 0, uniqueId: 0, userId: 0 });
        this.incomeService.selectedIncomeIdForEdit.set(null);

      },
      complete: () => { this.errorMsg.set(''); this.savingIncome.set(false); this.incomeForm.reset(); this.incomeService.selectedIncomeIdForEdit.set(null); },
      error: () => {
        this.errorMsg.set('Error saving income.');
        this.successMsg.set('');
        this.savingIncome.set(false);
      },
    });
  }


}
