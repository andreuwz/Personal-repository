import { Component, OnDestroy, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Observable, Subscription } from "rxjs";
import { AlertService } from "src/app/shared/alerts/alertService";
import { IGetProductAdminModel } from "../productModels/getProductAdminModel";
import { ProductService } from "../product.service";

@Component ({
    selector: 'admin-productsmenu',
    templateUrl: 'admin-productsMenu.component.html'
})

export class AdminProductsMenuComponent implements OnInit, OnDestroy{
    constructor(private productService: ProductService,
                private alertService: AlertService,
                private router: Router) {
                this.subscribeToDeleteProductEvents();
    }

    private getAllProductsSubscription: Subscription;
    private deleteProductEventSubscription: Subscription
    productsAdminModel: IGetProductAdminModel[] = [];
    private productId: string;

    ngOnDestroy(): void {
        if (this.deleteProductEventSubscription) {
            this.deleteProductEventSubscription.unsubscribe();
        }

        if (this.getAllProductsSubscription) {
            this.getAllProductsSubscription.unsubscribe();
        }
    }
    
    ngOnInit(): void {
        this.router.routeReuseStrategy.shouldReuseRoute = () => false;
        this.router.onSameUrlNavigation = 'reload';
        this.executeGetAllProductsAsAdmin();
    }

    executeGetAllProductsAsAdmin() {
        this.getAllProductsSubscription = this.productService.getAllProductsAsAdmin().subscribe({
            next: (productData) => this.productsAdminModel = productData
        })
    }

    private subscribeToDeleteProductEvents(){
     this.deleteProductEventSubscription = this.alertService.getSubjectAlertEvent().subscribe({
            next: (isConfirmed) => {

                if (isConfirmed) {
                    this.productService.deleteProduct(this.productId).subscribe({
                        next: () => {
                            this.reloadCurrentResources();
                        }
                    });
                }
            }
        });
    }
    
    private reloadCurrentResources(): void {
        this.router.navigate(['/AdminProducts']);
    }

    executeProductDelete(id: string) {
        this.productId = id;
        this.alertService.confirmationAlertProductDelete();
    }

    executeGetProductById(id: string) {
        this.productService.getAdminProductById(id).subscribe({
            next: (productData)=> 
            {
                this.productService.promptProductEditEvent(productData),
                this.router.navigate(['/EditProduct']);
            },
            error: (err)=> this.alertService.errorAlert(err)
        })
    }
}