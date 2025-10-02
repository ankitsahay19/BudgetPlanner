//savingIncome = signal(false);
import { MonthlyIncomeService } from '../../../../services/monthly-income-service';
import { Component, Input, signal, effect } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { single, tap } from 'rxjs';
import { IncomeSourceModel } from '../../../../models/IncomeSourceModel';

@Component({
  selector: 'add-income-component',
  imports: [ReactiveFormsModule],
  templateUrl: './add-income-component.html',
  styleUrl: './add-income-component.scss'
})
export class AddIncomeComponent {
  savingIncome = signal(false);
  @Input() editableIncomeSource = signal<IncomeSourceModel | null>(null);
  @Input({ required: true }) incomeSources = signal<IncomeSourceModel[]>([]);

  incomeForm = new FormGroup({
    sourceName: new FormControl(this.editableIncomeSource()?.sourceName, Validators.required),
    incomeAmount: new FormControl(this.editableIncomeSource()?.incomeAmount, [Validators.required, Validators.min(1)]),
    uniqueId: new FormControl(this.editableIncomeSource()?.uniqueId),
    userId: new FormControl(this.editableIncomeSource()?.userId)
  });
  constructor(private fb: FormBuilder, private monthlyIncomeService: MonthlyIncomeService) {
    effect(() => {
      const income = this.editableIncomeSource();
      if (income) {
        this.incomeForm.patchValue({
          sourceName: income.sourceName,
          incomeAmount: income.incomeAmount,
          uniqueId: income?.uniqueId,
          userId: income?.userId
        });
      } else {
        this.incomeForm.reset();
      }
    });
  }

  onSubmit() {
    if (this.incomeForm.valid) {
      this.savingIncome.set(true);
      const formValue = this.incomeForm.value;
      const newIncome: IncomeSourceModel = {
        uniqueId: this.editableIncomeSource()?.uniqueId ?? 0, // 0 means new
        userId: this.editableIncomeSource()?.userId ?? 1,     // put current userId here
        sourceName: formValue.sourceName!,
        incomeAmount: formValue.incomeAmount!,
        createdDate: this.editableIncomeSource()?.createdDate ?? new Date(),
        lastUpdatedDate: new Date(),
      };

      this.monthlyIncomeService.addIncomeSource(newIncome).pipe(
        tap(response => {
          this.incomeSources.set([response, ...this.incomeSources()]);
          this.incomeForm.reset();
          this.editableIncomeSource.set(null);
          this.savingIncome.set(false);
        })
      ).subscribe({
        error: () => {
          this.savingIncome.set(false);
        }
      });
    } else {
      console.log('Form is invalid');
    }
  }




}



