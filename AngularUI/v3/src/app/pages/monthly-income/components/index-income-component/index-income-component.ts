import { Component, effect, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MonthlyIncomeService } from '../../../../services/monthly-income-service';
import { AuthService } from '../../../../services/auth.service';

@Component({
  selector: 'app-index-income-component',
  standalone: true,
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
   * MonthlyIncomeService and AuthService injected via Angular signals
   */
  incomeService = inject(MonthlyIncomeService);
  authService = inject(AuthService);

  /**
   * Setup error fade-out effect and load income sources
   */
  constructor() {
    this.incomeService.getIncomeSources();
    effect(() => {
      if (this.incomeService.errorMsg()) {
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
   * Deletes an income source and shows loader/error feedback
   */
  deleteIncome(id: number) {
    this.deletingIds.add(id);
    this.incomeService.deleteIncomeSource(id).subscribe({
      next: () => {
        this.deletingIds.delete(id);
      },
      error: (_err: unknown) => {
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
   */
  getTotalIncome(): number {
    const sources = this.incomeService.myIncomeSources();
    return sources.reduce((sum: number, x: any) => sum + (x.incomeAmount || 0), 0);
  }
}
