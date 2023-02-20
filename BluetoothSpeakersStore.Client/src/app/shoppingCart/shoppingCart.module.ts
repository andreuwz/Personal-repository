import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShoppingCartService } from './shoppingCart.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { UserCartComponent } from './userCart/user-cart.component';
import { LoggedUserCartComponent } from './loggedUserCart/loggeduser-cart.component';
import { RoutingModule } from '../shared/routing/routing.module';



@NgModule({
  declarations: [
    UserCartComponent,
    LoggedUserCartComponent
  ],
  imports: [
    CommonModule,
    RoutingModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    ShoppingCartService
  ]
})
export class ShoppingCartModule { }
