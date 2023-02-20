import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http'

import { AppComponent } from './app.component';

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

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CookieService } from 'ngx-cookie-service';
import { SecurityHttpInterceptor } from './authentication/securityHttpInterceptor';
import { UserModule } from './users/user.module';
import { ProductModule } from './products/product.module';
import { ShoppingCartModule } from './shoppingCart/shoppingCart.module';
import { LoginModule } from './authentication/login.module';
import { NavigationBarModule } from './shared/navigationBar/navigationbar.module';
import { RoutingModule } from './shared/routing/routing.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    RoutingModule,
    NavigationBarModule,
    LoginModule,
    UserModule,
    ProductModule,
    ShoppingCartModule,
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
    {provide: HTTP_INTERCEPTORS, useClass: SecurityHttpInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
