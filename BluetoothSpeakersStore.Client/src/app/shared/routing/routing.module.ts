import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule} from '@angular/router';
import { NavigationBarComponent } from '../navigationBar/navigation-bar.component';
import { LoginFormComponent } from '../../authentication/login/login-form.component';
import { UserListComponent } from '../../users/listUsers/users-list.component';
import { AuthGuardLogIn } from '../../authentication/authGuard/authGuard-login.service';
import { AuthGuardLogOut } from '../../authentication/authGuard/authGuard-logout.service';
import { AuthGuardRoles } from '../../authentication/authGuard/authGuard-roles-service';
import { AdminProductsMenuComponent } from '../../products/adminProductsMenu/admin-productsMenu.component';
import { CreateProductComponent } from '../../products/createProducts/create-product.component';
import { EditProductComponent } from '../../products/editProduct/edit-product.component';
import { ProductListComponent } from '../../products/listProducts/product-list.component';
import { LoggedUserCartComponent } from '../../shoppingCart/loggedUserCart/loggeduser-cart.component';
import { UserCartComponent } from '../../shoppingCart/userCart/user-cart.component';
import { EditLoggedUserComponent } from '../../users/editLoggedUser/edit-loggeduser.component';
import { EditUserComponent } from '../../users/editUsers/edit-user.component';
import { RegisterUserComponent } from '../../users/registerUsers/register-user.component';

const routes: Routes = [
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
];

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forRoot(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class RoutingModule { }
