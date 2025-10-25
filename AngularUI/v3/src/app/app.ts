import { Component, OnInit } from '@angular/core';
import { Router, RouterLink, RouterOutlet, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MenuComponent } from "./components/menu-component/menu-component";
import { TopMenuDatePickerComponent } from './components/top-menu-date-picker/top-menu-date-picker.component';
import { AuthService } from './services/auth.service';
import { UserMonthlyDataService } from './services/user-monthly-data.service';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, MenuComponent, TopMenuDatePickerComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})

export class App {
  // selected values from the top menu picker (month is 0-based internally)
  currentPickerYear = new Date().getFullYear();
  currentPickerMonth = new Date().getMonth();

  constructor(public auth: AuthService, private router: Router, private userMonthlyData: UserMonthlyDataService) { }

  protected title = 'MyBudgetPlannerUi';

  get userTokenData() {
    return this.auth.userTokenData();
  }

  get isLoggedIn() {
    return this.auth.isLoggedIn();
  }

  logout() {
    this.auth.logout();
    this.router.navigate(['/login']);
  }

  // Called when TopMenuDatePickerComponent emits a new year
  onYearChange(year: number) {
    this.currentPickerYear = year;
    this.callUserMonthlyData();
  }

  // Called when TopMenuDatePickerComponent emits a new month (0-based)
  onMonthChange(month: number) {
    this.currentPickerMonth = month;
    this.callUserMonthlyData();
  }

  private callUserMonthlyData() {
    // API expects 1-based month, so add 1
    const apiMonth = this.currentPickerMonth + 1;
    this.userMonthlyData.getUserMonthlyData(this.currentPickerYear, apiMonth).subscribe({
      next: (res) => {
        // service already logs via tap; keep this for quick inspection
        console.log('App received monthly data:', res);
      },
      error: (err) => console.error('Failed fetching user monthly data', err)
    });
  }

  get currentYear(): number {
    return new Date().getFullYear();
  }
}
