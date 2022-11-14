import { Component, OnDestroy, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Subscription } from "rxjs";
import { AlertService } from "src/app/shared/alerts/alertService";
import { UserService } from "src/app/users/user.service";
import { ShoppingCartService } from "../shoppingCart.service";
import { IGetUserBalanceModel } from "../shoppingCartModels/getUserBalanceModel";
import { IGetUserCartModel } from "../shoppingCartModels/getUserCartModel";
import { IInCartProductModel } from "../shoppingCartModels/inCartProductsModel";

@Component({
    selector: 'loggeduser-cart',
    templateUrl: './loggeduser-cart.component.html'
})

export class LoggedUserCartComponent implements OnInit, OnDestroy{
    constructor(private cartService: ShoppingCartService,
                private alertService: AlertService,
                private userService: UserService,
                private router: Router) {
                this.loggedUserCartModel = {cartId: 'Unavailable', creatorName: 'Unavailable', createdAt: 'Unavailable', totalSum: 0};
                this.loggedUserInfoModel = {balance: ""};
                this.confirmProductInCartRemovalSubscription();
                this.confirmCartCheckoutSubscribe();
                this.confirmDeleteLoggedUserCartSubscription();
                this.executeGetLoggedUserBalance();
            }
    
    private loggedUserInfoSubscription: Subscription
    private confirmLoggedUserCartRemoveSubscription: Subscription;
    private removeLoggedUserCartSubscription: Subscription
    private confirmCartCheckoutSubscription: Subscription;
    private cartCheckoutSubscription: Subscription
    private confirmProductRemoveFromCartSubscription: Subscription;
    private deleteProductFromCartSubscription: Subscription;
    private getLoggedUserCartProductsSubscription: Subscription;
    private getLoggedUserCartSubscription: Subscription;
    loggedUserCartModel: IGetUserCartModel;
    loggedUserCartProducts: IInCartProductModel[] = [];
    loggedUserInfoModel: IGetUserBalanceModel;
    private producId: string;

    ngOnDestroy(): void {
        if (this.loggedUserInfoSubscription) {
            this.loggedUserInfoSubscription.unsubscribe();
        }

        if (this.getLoggedUserCartSubscription) {
            this.getLoggedUserCartSubscription.unsubscribe();
        }

        if (this.getLoggedUserCartProductsSubscription) {
            this.getLoggedUserCartProductsSubscription.unsubscribe();
        }

        if (this.confirmProductRemoveFromCartSubscription) {
            this.confirmProductRemoveFromCartSubscription.unsubscribe();
        }

        if (this.deleteProductFromCartSubscription) {
            this.deleteProductFromCartSubscription.unsubscribe();
        }

        if (this.confirmCartCheckoutSubscription) {
            this.confirmCartCheckoutSubscription.unsubscribe();
        }

        if (this.cartCheckoutSubscription) {
            this.cartCheckoutSubscription.unsubscribe();
        }

        if (this.removeLoggedUserCartSubscription) {
            this.removeLoggedUserCartSubscription.unsubscribe();
        }

        if (this.confirmLoggedUserCartRemoveSubscription) {
            this.confirmLoggedUserCartRemoveSubscription.unsubscribe();
        }
    }

    ngOnInit(): void {
        this.router.routeReuseStrategy.shouldReuseRoute = () => false;
        this.router.onSameUrlNavigation = 'reload';

        this.executeGetLoggedUserCart();
        this.executeGetLoggedUserCartProducts();
    }
    
    private executeGetLoggedUserCart() {
        this.getLoggedUserCartSubscription = this.cartService.getLoggedUserCart().subscribe({
            next: (userCart) => this.loggedUserCartModel = userCart,
            error: ()=> this.alertService.errorAlert("Your cart is empty")
        })
    }

    private executeGetLoggedUserCartProducts() {
        this.getLoggedUserCartProductsSubscription = this.cartService.getLoggedUserInCartProducts().subscribe({
            next: (productList) => this.loggedUserCartProducts = productList
        })
    }

    executeRemoveProductInCartConfirmation(id: string) {
        this.producId = id;
        this.alertService.confirmationAlertRemoveProductFromCart();
    }

    private executeDeleteProductFromCart() {
        this.deleteProductFromCartSubscription = this.cartService.removeProductFromCart(this.producId).subscribe({
            next: ()=>  
            {
                this.router.navigate(['/LoggedUserCart']),
                this.alertService.successfulProductRemoveFromCart()
            },
            error: (err)=> this.alertService.errorAlert(err)
        })
    }

    private confirmProductInCartRemovalSubscription() {
        this.confirmProductRemoveFromCartSubscription = this.alertService.getSubjectAlertEvent().subscribe({
            next: (isEventConfirmed) => {
                if (isEventConfirmed) {
                    this.executeDeleteProductFromCart();
                }
            }
        })
    }

    executeConfirmCartCheckout() {
        this.alertService.confirmationAlertCartCheckout();
    }

    private confirmCartCheckoutSubscribe() {
        this.confirmCartCheckoutSubscription = this.alertService.getCartCheckoutAlertEvent().subscribe({
            next: (isEventConfirmed)=> {
                if (isEventConfirmed) {
                    this.executeCartCheckout();
                }
            }
        })
    }

    private executeCartCheckout() {
        this.cartCheckoutSubscription = this.cartService.cartCheckout().subscribe({
            next: ()=> 
            {
            this.alertService.successfulCartCheckout(),
            this.router.navigate(['/Products'])
            },
            error: (err)=> this.alertService.errorAlert(err)
        })
    }

    executeConfirmationDeleteLoggedUserCart() {
        this.alertService.confirmationAlertLoggedUserCartDelete();
    }

    private confirmDeleteLoggedUserCartSubscription() {
        this.confirmLoggedUserCartRemoveSubscription = this.alertService.getLoggedUserCartRemovalEvent().subscribe({
            next: (isEventConfirmed) => {
                if (isEventConfirmed) {
                    this.executeLoggedUserCartRemoval();
                }
            }
        })
    }

    private executeLoggedUserCartRemoval() {
        this.removeLoggedUserCartSubscription = this.cartService.removeLoggedUserCart().subscribe({
            next: ()=> {
                this.alertService.successfulLoggedUserCartRemoval(),
                this.router.navigate(['/Products'])
            },
            error: (err) => this.alertService.errorAlert(err)
        })
    }

    private executeGetLoggedUserBalance() {
        this.loggedUserInfoSubscription = this.userService.getLoggedUser().subscribe( {
            next: (userInfo) => this.loggedUserInfoModel = userInfo,
            error: (err) => this.alertService.errorAlert(err)
        })
    }
}