import { Routes } from '@angular/router';
import { UserListComponent } from './components/user-list/user-list.component';
import { RegisterUserComponent } from './components/register-user/register-user.component';
import { EditUserComponent } from './components/edit-user/edit-user.component';

export const routes: Routes = [
  { path: '', redirectTo: '/users', pathMatch: 'full' },
  { path: 'users', component: UserListComponent },
  { path: 'users/register', component: RegisterUserComponent },
  { path: 'users/edit/:username', component: EditUserComponent },
  { path: '**', redirectTo: '/users' }
];
