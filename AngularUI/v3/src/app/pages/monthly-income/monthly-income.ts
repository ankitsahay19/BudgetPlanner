import { Component, Input, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MonthlyIncomeService } from '../../services/monthly-income-service';
import { AuthService } from '../../services/auth.service';
import { IndexIncomeComponent } from "./components/index-income-component/index-income-component";
import { AddEditIncomeComponent } from "./components/add-edit-income-component/add-edit-income-component";
@Component({
  selector: 'app-monthly-income',
  standalone: true,
  imports: [CommonModule, FormsModule, IndexIncomeComponent, AddEditIncomeComponent],
  templateUrl: './monthly-income.html',
  styleUrls: ['./monthly-income.scss']
})
export class MonthlyIncome {
  constructor(private incomeService: MonthlyIncomeService, private authService: AuthService) { }
  ngOnInit() {
    this.incomeService.getIncomeSources();
  }
}
