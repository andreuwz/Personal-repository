import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EditUserComponent } from './editUsers/edit-user.component';
import { UserListComponent } from './listUsers/users-list.component';
import { RegisterUserComponent } from './registerUsers/register-user.component';
import { EditLoggedUserComponent } from './editLoggedUser/edit-loggeduser.component';
import { UserService } from './user.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RoutingModule } from '../shared/routing/routing.module';

@NgModule({
  declarations: [
    EditUserComponent,
    UserListComponent,
    RegisterUserComponent,
    EditLoggedUserComponent
  ],
  imports: [
    CommonModule,
    RoutingModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    UserService
  ]
})
export class UserModule { }
