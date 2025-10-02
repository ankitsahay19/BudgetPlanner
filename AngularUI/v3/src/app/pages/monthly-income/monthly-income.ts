import { Component, Input, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MonthlyIncomeService } from '../../services/monthly-income-service';
import { IncomeSourceModel } from '../../models/IncomeSourceModel';
import { AuthService } from '../../services/auth.service';
import { AddIncomeComponent } from "./components/add-income-component/add-income-component";
import { IndexIncomeComponent } from "./components/index-income-component/index-income-component";
@Component({
  selector: 'app-monthly-income',
  standalone: true,
  imports: [CommonModule, FormsModule, AddIncomeComponent, IndexIncomeComponent],
  templateUrl: './monthly-income.html',
  styleUrls: ['./monthly-income.scss']
})
export class MonthlyIncome {
  incomeSources = signal<IncomeSourceModel[]>([]);
  editableIncomeSource = signal<IncomeSourceModel | null>(null);
  source: string = '';
  amount: number | null = null;
  message: string = '';
  loadingIncomes = signal(false);

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
  addIncome() {
    if (this.source && this.amount && this.amount > 0) {
      // this.incomes.push({
      //   id: Date.now(),
      //   source: this.source,
      //   amount: this.amount
      // });

      this.message = 'Income added successfully ✅';

      // Clear inputs
      this.source = '';
      this.amount = null;
    }
  }

  deleteIncome(id: number) {
    //this.incomes = this.incomes.filter(income => income.id !== id);
    this.message = 'Income entry deleted 🗑️';
  }
}
