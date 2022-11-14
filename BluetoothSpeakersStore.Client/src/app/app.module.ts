import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import {HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http'

import { AppComponent } from './app.component';
import { LoginFormComponent } from './authentication/login/login-form.component';
import { NavigationBarComponent } from './shared/navigationBar/navigation-bar.component';

import { UserListComponent } from './users/listUsers/users-list.component';
import { EditUserComponent } from './users/editUsers/edit-user.component';
import { RegisterUserComponent } from './users/registerUsers/register-user.component';
import { EditLoggedUserComponent } from './users/editLoggedUser/edit-loggeduser.component';
import { ProductListComponent } from './products/listProducts/product-list.component';
import { AdminProductsMenuComponent } from './products/adminProductsMenu/admin-productsMenu.component';
import { CreateProductComponent } from './products/createProducts/create-product.component';

import { CommonModule } from '@angular/common';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { MatButtonModule } from '@angular/material/button';

import { EditProductComponent } from './products/editProduct/edit-product.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoggedUserCartComponent } from './shoppingCart/loggedUserCart/loggeduser-cart.component';
import { UserCartComponent } from './shoppingCart/userCart/user-cart.component';
import { AuthGuardLogIn } from './authentication/authGuard/authGuard-login.service';
import { AuthGuardRoles } from './authentication/authGuard/authGuard-roles-service';
import { AuthGuardLogOut } from './authentication/authGuard/authGuard-logout.service';
import { CookieService } from 'ngx-cookie-service';
import { securityHttpInterceptor } from './authentication/securityHttpInterceptor';


@NgModule({
  declarations: [
    AppComponent,
    NavigationBarComponent,
    LoginFormComponent,
    UserListComponent,
    EditUserComponent,
    RegisterUserComponent,
    EditLoggedUserComponent,
    ProductListComponent,
    AdminProductsMenuComponent,
    CreateProductComponent,
    EditProductComponent,
    LoggedUserCartComponent,
    UserCartComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot([
      {path: 'Home', component: NavigationBarComponent},
      {path: 'Login', component: LoginFormComponent},
      {path: 'Users', component: UserListComponent, canActivate: [AuthGuardLogIn, AuthGuardRoles]},
      {path: 'EditUser', component: EditUserComponent, canActivate: [AuthGuardLogIn, AuthGuardRoles]},
      {path: 'Register', component: RegisterUserComponent, canActivate: [AuthGuardLogOut]},
      {path: 'EditLoggedUser', component: EditLoggedUserComponent,canActivate: [AuthGuardLogIn]},
      {path: 'Products', component: ProductListComponent},
      {path: 'AdminProducts', component: AdminProductsMenuComponent, canActivate: [AuthGuardLogIn, AuthGuardRoles]},
      {path: 'CreateProduct', component: CreateProductComponent, canActivate: [AuthGuardLogIn, AuthGuardRoles]},
      {path: 'EditProduct', component: EditProductComponent, canActivate: [AuthGuardLogIn, AuthGuardRoles]},
      {path: 'LoggedUserCart', component: LoggedUserCartComponent, canActivate: [AuthGuardLogIn]},
      {path: 'UserCart', component: UserCartComponent,canActivate: [AuthGuardLogIn, AuthGuardRoles]}
    ]),
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
        CommonModule,
        MatTableModule,
        MatSortModule,
        MatFormFieldModule,
        MatInputModule,
        MatDatepickerModule,
        MatNativeDateModule,
        MatIconModule,
        MatSelectModule,
        MatRadioModule,
        MatButtonModule,
    
  ],
  exports: [
    CommonModule,
    MatTableModule,
    MatSortModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatIconModule,
    MatSelectModule,
    MatRadioModule,
    MatButtonModule
],
  providers: [
    CookieService,
    {provide: HTTP_INTERCEPTORS, useClass: securityHttpInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
