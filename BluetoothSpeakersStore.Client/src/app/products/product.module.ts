import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService } from './product.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AdminProductsMenuComponent } from './adminProductsMenu/admin-productsMenu.component';
import { CreateProductComponent } from './createProducts/create-product.component';
import { EditProductComponent } from './editProduct/edit-product.component';
import { ProductListComponent } from './listProducts/product-list.component';
import { RoutingModule } from '../shared/routing/routing.module';



@NgModule({
  declarations: [
    ProductListComponent,
    EditProductComponent,
    CreateProductComponent,
    AdminProductsMenuComponent
  ],
  imports: [
    CommonModule,
    RoutingModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    ProductService
  ]
})
export class ProductModule { }
