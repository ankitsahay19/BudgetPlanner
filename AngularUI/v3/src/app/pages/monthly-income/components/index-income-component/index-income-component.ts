import { Component, Input, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IncomeSourceModel } from '../../../../models/IncomeSourceModel';
import { MonthlyIncomeService } from '../../../../services/monthly-income-service';
import { AuthService } from '../../../../services/auth.service';

@Component({
  selector: 'app-index-income-component',
  imports: [CommonModule],
  templateUrl: './index-income-component.html',
  styleUrl: './index-income-component.scss'
})
export class IndexIncomeComponent {
  editIncome(id: number) {
    this.incomeSources().find(income => {
      if (income.uniqueId === id) {
        this.editableIncomeSource.set(income);
        console.log(this.editableIncomeSource());
      }
    });
  }
  @Input({ required: true }) incomeSources = signal<IncomeSourceModel[]>([]);
  @Input({ required: true }) editableIncomeSource = signal<IncomeSourceModel | null>(null);
  source: string = '';
  amount: number | null = null;
  message: string = '';
  loadingIncomes = signal(false);
  deletingIds: Set<number> = new Set();

  constructor(private monthlyIncomeService: MonthlyIncomeService, private authService: AuthService) {
    this.loadIncomeSources();
  }

  private loadIncomeSources() {

    this.loadingIncomes.set(true);
    this.monthlyIncomeService.getIncomeSources().subscribe({
      next: (data) => {
        var loggedinUser = this.authService.userTokenData();
        var filteredIncomes = data.filter(income => income.userId == loggedinUser?.userId);
        this.incomeSources.set(filteredIncomes);
        console.log(this.incomeSources());
        this.message = 'Income sources loaded successfully';
      },
      error: (err) => {
        this.message = 'Error loading income sources';
      }
    });
    this.loadingIncomes.set(false);
  }

  deleteIncome(id: number) {
    this.deletingIds.add(id);
    this.monthlyIncomeService.DeleteIncomeSource(id).subscribe({
      next: () => {
        this.incomeSources.set(this.incomeSources().filter(income => income.uniqueId !== id));
        this.message = 'Income entry deleted 🗑️';
        this.deletingIds.delete(id);
      },
      error: (err) => {
        this.message = 'Error deleting income entry';
        this.deletingIds.delete(id);
      }
    });
  }

}
