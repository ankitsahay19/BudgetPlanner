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
  /**
   * Controls fade-out animation for error message
   */
  errorFading = false;
  /**
   * Tracks which income rows are currently being deleted (for loader UI)
   */
  deletingIds: Set<number> = new Set();

  /**
   * Injects the MonthlyIncomeService and AuthService
   */
  constructor(public incomeService: MonthlyIncomeService, private authService: AuthService) { }

  /**
   * Loads income sources on component initialization
   */
  ngOnInit() {
    this.incomeService.getIncomeSources();
  }

  /**
   * Deletes an income source and shows loader/error feedback
   * @param id Unique ID of the income source to delete
   */
  deleteIncome(id: number) {
    this.deletingIds.add(id);
    this.incomeService.deleteIncomeSource(id).subscribe({
      next: () => {
        this.deletingIds.delete(id);
      },
      error: (err) => {
        this.deletingIds.delete(id);
        this.incomeService.errorMsg.set('Failed to delete income. Please try again.');
        this.errorFading = false;
        setTimeout(() => {
          this.errorFading = true;
        }, 500);
        setTimeout(() => {
          this.incomeService.errorMsg.set('');
          this.errorFading = false;
        }, 3000);
      }
    });
  }

  /**
   * Calculates the total income from all sources
   * @returns Total income amount
   */
  getTotalIncome(): number {
    const sources = this.incomeService.myIncomeSources();
    return sources.reduce((sum, x) => sum + (x.incomeAmount || 0), 0);
  }
}
