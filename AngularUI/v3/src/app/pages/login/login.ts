import { link } from 'fs/promises';

import { Component, signal } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.html',
  styleUrl: './login.scss',
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  standalone: true,
})
export class Login {
  loginForm: FormGroup;
  notification = signal<string | null>(null);
  isSuccess = signal(false);
  isLoading = signal(false);

  constructor(private fb: FormBuilder, private auth: AuthService, private router: Router) {
    if (this.auth.isLoggedIn()) {
      this.router.navigate(['/income']);
    }
    this.loginForm = this.fb.group({
      loginName: ['Ankit.sahay101@example.com', [Validators.required, Validators.email]],
      password: ['Ankit.sahay101@example.com', [Validators.required, Validators.minLength(6),]]
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      this.isLoading.set(true);
      this.notification.set(null);
      this.auth.loginUser(this.loginForm.value).subscribe({
        next: (response) => {
          this.isLoading.set(false);
          if (response && response.isLoginSuccess) {
            this.auth.setUser(response); // update AuthService, triggers App update
            this.notification.set('Login successful!');
            this.isSuccess.set(true);
            setTimeout(() => this.router.navigate(['/categories']), 1000);
          } else {
            this.notification.set('Login failed. Please check your credentials.');
            this.isSuccess.set(false);
          }
        },
        error: (err) => {
          this.isLoading.set(false);
          this.notification.set('Something went wrong. Please try again.');
          this.isSuccess.set(false);
        }
      });
    }
  }
}
