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

  constructor(public incomeService: MonthlyIncomeService, private authService: AuthService) {

  }
  ngOnInit() {
    this.incomeService.getIncomeSources();
  }

  deletingIds: Set<number> = new Set();



  deleteIncome(id: number) {
    this.deletingIds.add(id);
    this.incomeService.deleteIncomeSource(id).subscribe({
      next: () => this.deletingIds.delete(id),
      error: () => this.deletingIds.delete(id)
    });
  }

  getTotalIncome(): number {
    const sources = this.incomeService.myIncomeSources();
    return sources.reduce((sum, x) => sum + (x.incomeAmount || 0), 0);
  }


}
