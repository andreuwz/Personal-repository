import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavigationBarComponent } from './navigation-bar.component';
import { RoutingModule } from 'src/app/shared/routing/routing.module';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    NavigationBarComponent
  ],
  imports: [
    CommonModule,
    RoutingModule
  ],
  exports: [
    NavigationBarComponent
  ]

})
export class NavigationBarModule { }
