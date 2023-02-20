import { Component, OnDestroy, OnInit } from "@angular/core";
import { FormGroup} from "@angular/forms";
import { Subscription } from "rxjs";
import { EncryptionService } from "src/app/shared/encryption.service";
import { AlertService } from "src/app/shared/alerts/alertService";
import { IGetProductModel } from "../productModels/getProductModel";
import { ProductService } from "../product.service";

@Component({
    selector: 'product-list',
    templateUrl: './product-list.component.html'
})

export class ProductListComponent implements OnInit, OnDestroy{
    constructor(private productService: ProductService,
                private alertService: AlertService,
                private encryptionService: EncryptionService) {
                    this.addToCartSubscription()
    }
    
    private confirmAddToCartSubscription: Subscription
    private addToCartEventSubscription: Subscription;
    private getAllProductsSubscription: Subscription;
    quantityInput: FormGroup;
    productList: IGetProductModel[] = [];
    isUserLoggedIn: boolean = false;
    productId: string;

    ngOnDestroy(): void {
        if ( this.getAllProductsSubscription) {
            this.getAllProductsSubscription.unsubscribe();
        }

        if ( this.addToCartEventSubscription) {
            this.addToCartEventSubscription.unsubscribe();
        }

        if ( this.confirmAddToCartSubscription) {
            this.confirmAddToCartSubscription.unsubscribe();
        }
    }

    ngOnInit(): void {
        this.isLoggedInConfirmed();

        this.getAllProductsSubscription = this.productService.getAllProducts().subscribe({
            next: (productData) => 
            {
                this.productList = productData,
                console.log(productData)
            }
        })
    }

    isLoggedInConfirmed():boolean {
        if (this.encryptionService.decryptionAES(localStorage.getItem('infoa')!) == 'true') {
            return true;
        }
       return false;
    }

    executeAddToCartConfirmation(id: string) {
        this.productId = id;
        this.alertService.addToCartDialogBox();
    }

    private addToCartSubscription() {
        this.confirmAddToCartSubscription = this.alertService.getQuantityAlertEvent().subscribe({
            next: (quantity)=> {
                    this.executeAddToCart(quantity)
            }
        })
    }
    
    private executeAddToCart(quantity: number) {
        this.addToCartEventSubscription = this.productService.addProductToCart(this.productId, quantity).subscribe({
            next: ()=> this.alertService.successfulProductAddToCart(),
            error: (err)=> this.alertService.errorAlert(err)
        })
    }
}