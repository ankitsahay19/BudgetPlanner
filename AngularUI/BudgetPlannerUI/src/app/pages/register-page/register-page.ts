import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { UserRegistrationModel } from '../../models/UserRegistrationModel';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register-page',
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './register-page.html',
  styleUrl: './register-page.scss',
})


export class RegisterPage {
  registerForm: FormGroup;
  isSuccess = signal(false);
  isLoading = signal(false);
  notificationMessage = signal<string | null>(null);

  // Define the form controls and their initial values

  constructor(private fb: FormBuilder, private auth: AuthService, private router: Router) {
    this.registerForm = this.fb.group({
      firstName: ['Ankit', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      lastName: ['Sahay', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      phoneNumber: ['9999999999', [Validators.required, Validators.pattern(/^[0-9]{10,15}$/)]],
      email: ['Ankit.sahay1@example.com', [Validators.required, Validators.email, Validators.maxLength(100)]],
      password: ['Ankit.sahay1@example.com', [
        Validators.required,
        Validators.minLength(6),
        Validators.maxLength(30),
        this.createPasswordStrengthValidator()
      ]],
      confirmPassword: ['Ankit.sahay1@example.com', Validators.required],
      roles: ['2'] // Default role set to '2' AS  APP USER.
    }, { validators: this.passwordMatchValidator });
  }

  // Custom password strength validator
  private createPasswordStrengthValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;
      if (!value) return null;
      const hasUpperCase = /[A-Z]/.test(value);
      const hasLowerCase = /[a-z]/.test(value);
      const hasNumeric = /[0-9]/.test(value);
      const hasSpecialChar = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/.test(value);
      const passwordValid = hasUpperCase && hasLowerCase && hasNumeric && hasSpecialChar;
      return !passwordValid ? { passwordStrength: true } : null;
    };
  }

  // Custom validator to check if password and confirm password match
  private passwordMatchValidator: ValidatorFn = (formGroup: AbstractControl): ValidationErrors | null => {
    const password = formGroup.get('password')?.value;
    const confirmPassword = formGroup.get('confirmPassword')?.value;
    return password && confirmPassword && password !== confirmPassword
      ? { passwordMismatch: true }
      : null;
  };



  onSubmit() {
    if (this.registerForm.valid) {
      this.isLoading.set(true);
      this.notificationMessage.set(null); // Reset notification message
      console.log('Form Submitted', this.registerForm.value);
      const formValue = this.registerForm.value;
      const user: UserRegistrationModel = {
        ...formValue,
        roles: formValue.roles ? formValue.roles.split(',').map((role: string) => role.trim()) : []
      };

      this.auth.registerUser(user).subscribe({
        next: (response) => {
          this.isLoading.set(false);
          console.log('Registration Response:', response);
          if (response.isCreated) {
            this.notificationMessage.set(response.successMessages?.join(', ') || 'Registration Successful!');
            this.isSuccess.set(true);
            this.registerForm.reset();
            // Automatically log in the user after successful registration
            this.auth.loginUser({ loginName: user.email, password: user.password }).subscribe({
              next: (response) => {
                console.log('Login Response:', response);
                setTimeout(() => this.router.navigate(['/home']), 2000);
              }
            });

          } else {
            this.notificationMessage.set(response.errorMessages?.join(', ') || 'Registration Failed');
            this.isSuccess.set(false);
          }
        },
        error: (err) => {
          this.isLoading.set(false);
          console.error('Registration Error:', err);
          this.notificationMessage.set('Something went wrong. Please try again.');
          this.isSuccess.set(false);
        }
      });
    }
  }
}
