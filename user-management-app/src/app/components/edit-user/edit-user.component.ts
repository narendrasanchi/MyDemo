import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { UpdateUserRequest, UserResponse } from '../../models/user.model';

@Component({
  selector: 'app-edit-user',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatSnackBarModule
  ],
  templateUrl: './edit-user.component.html',
  styleUrl: './edit-user.component.scss'
})
export class EditUserComponent implements OnInit {
  editForm: FormGroup;
  isLoading = false;
  username: string = '';
  originalUser: UserResponse | null = null;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private snackBar: MatSnackBar,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.editForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.pattern(/^\+?[\d\s\-\(\)]+$/)]],
      givenName: [''],
      familyName: ['']
    });
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.username = params['username'];
      this.loadUser();
    });
  }

  loadUser(): void {
    if (!this.username) {
      this.router.navigate(['/users']);
      return;
    }

    this.isLoading = true;
    this.userService.getUser(this.username).subscribe({
      next: (user) => {
        this.originalUser = user;
        this.editForm.patchValue({
          email: user.email,
          phoneNumber: user.phoneNumber || '',
          givenName: user.givenName || '',
          familyName: user.familyName || ''
        });
        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
        console.error('Error loading user:', error);
        this.snackBar.open('User not found', 'Close', {
          duration: 5000,
          panelClass: ['error-snackbar']
        });
        this.router.navigate(['/users']);
      }
    });
  }

  onSubmit(): void {
    if (this.editForm.valid && this.originalUser) {
      this.isLoading = true;
      const formValue = this.editForm.value;
      const updateData: UpdateUserRequest = {};

      // Only include fields that have changed
      if (formValue.email !== this.originalUser.email) {
        updateData.email = formValue.email;
      }
      if (formValue.phoneNumber !== (this.originalUser.phoneNumber || '')) {
        updateData.phoneNumber = formValue.phoneNumber || undefined;
      }
      if (formValue.givenName !== (this.originalUser.givenName || '')) {
        updateData.givenName = formValue.givenName || undefined;
      }
      if (formValue.familyName !== (this.originalUser.familyName || '')) {
        updateData.familyName = formValue.familyName || undefined;
      }

      // Check if there are any changes
      if (Object.keys(updateData).length === 0) {
        this.snackBar.open('No changes to save', 'Close', {
          duration: 3000,
          panelClass: ['error-snackbar']
        });
        this.isLoading = false;
        return;
      }

      this.userService.updateUser(this.username, updateData).subscribe({
        next: (response) => {
          this.isLoading = false;
          this.snackBar.open('User updated successfully!', 'Close', {
            duration: 3000,
            panelClass: ['success-snackbar']
          });
          this.router.navigate(['/users']);
        },
        error: (error) => {
          this.isLoading = false;
          console.error('Update error:', error);
          const errorMessage = error.error?.message || 'Update failed. Please try again.';
          this.snackBar.open(errorMessage, 'Close', {
            duration: 5000,
            panelClass: ['error-snackbar']
          });
        }
      });
    } else {
      this.markAllFieldsAsTouched();
    }
  }

  private markAllFieldsAsTouched(): void {
    Object.keys(this.editForm.controls).forEach(key => {
      this.editForm.get(key)?.markAsTouched();
    });
  }

  getErrorMessage(fieldName: string): string {
    const field = this.editForm.get(fieldName);
    if (field?.hasError('required')) {
      return `${fieldName} is required`;
    }
    if (field?.hasError('email')) {
      return 'Please enter a valid email address';
    }
    if (field?.hasError('pattern')) {
      return 'Please enter a valid phone number';
    }
    return '';
  }

  cancel(): void {
    this.router.navigate(['/users']);
  }
}
