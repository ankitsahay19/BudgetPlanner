import { Component, signal } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { AuthService } from './services/auth.service';
import { MenuComponent } from "./components/menu-component/menu-component";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink, MenuComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  constructor(public auth: AuthService, private router: Router) { }

  protected readonly title = signal('MyBudgetPlannerUI');

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

  get currentYear(): number {
    return new Date().getFullYear();
  }
}
