import { Component, OnDestroy, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Subscription } from "rxjs";
import { AlertService } from "src/app/shared/alerts/alertService";
import { SharedService } from "src/app/shared/shared.service";
import { ShoppingCartService } from "../shoppingCart.service";
import { IGetUserCartModel } from "../shoppingCartModels/getUserCartModel";
import { IInCartProductModel } from "../shoppingCartModels/inCartProductsModel";

@Component({
    selector:'user-cart',
    templateUrl: './user-cart.component.html'
})
 
export class UserCartComponent implements OnInit, OnDestroy{

    constructor(private cartService: ShoppingCartService,
                private alertService: AlertService,
                private sharedService: SharedService,
                private router: Router) {
        this.userCartModel = {cartId: 'Unavailable', createdAt: 'Unavailable', creatorName: 'Unavailable', totalSum: 0};
        this.subscribeGetUserIdEvent();
    }

    private deleteCartByIdSubscription: Subscription;
    private getUserIdSubscription: Subscription;
    private getUserCartSubscription: Subscription;
    private getUserCartProductsSubscription: Subscription
    userCartModel: IGetUserCartModel;
    userCartProductsModel: IInCartProductModel[] = [];
    private userId: string;

    ngOnDestroy(): void {
        if (this.getUserCartSubscription) {
            this.getUserCartSubscription.unsubscribe();
        }

        if (this.getUserCartProductsSubscription) {
            this.getUserCartProductsSubscription.unsubscribe();
        }

        if(this.getUserIdSubscription) {
            this.getUserIdSubscription.unsubscribe();
        } 

        if(this.deleteCartByIdSubscription) {
            this.deleteCartByIdSubscription.unsubscribe();
        } 
    }

    ngOnInit(): void {
        this.executeGetCartByUserId(this.userId);
    }

    private executeGetCartByUserId(id: string) {
        this.getUserCartSubscription = this.cartService.getUserCart(id).subscribe({
            next: (cartData)=> {
                this.userCartModel = cartData,
                this.executeGetUserProductsInCart(this.userCartModel.cartId)
            },
            error: ()=> this.alertService.errorAlert(`This cart is empty!`)
        })
    }

    private executeGetUserProductsInCart(id: string) {
        this.getUserCartProductsSubscription = this.cartService.getUserInCartProducts(id).subscribe({
            next: (productsData) => this.userCartProductsModel = productsData
        })
    }

    private subscribeGetUserIdEvent() {
        this.getUserIdSubscription = this.sharedService.getUserIdGetCartEvent().subscribe({
            next: (userIdInfo)=> this.userId = userIdInfo
        })
    }

    executeDeleteCartById(id: string) {
        this.deleteCartByIdSubscription = this.cartService.removeCartById(id).subscribe({
            next: ()=> {
                this.alertService.successfulUserCartRemoval(),
                this.router.navigate(['/Users'])
            }
        })
    }
}