import { Component, OnDestroy, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { Subscription } from "rxjs";
import { AlertService } from "src/app/shared/alerts/alertService";
import { ICreateProductModel } from "../productModels/createProductModel";

import { ProductService } from "../product.service";

@Component({
    selector: 'product-list',
    templateUrl: './create-product.component.html'
})

export class CreateProductComponent implements OnInit, OnDestroy{
    constructor(private productService: ProductService,
                private formBuilder: FormBuilder,
                private alertService: AlertService,
                private router: Router) {
                    this.createProductModel = {Name: '', Price: 0, Quantity: 0}
    }

    private createProductSubscription: Subscription;
    createProductModel: ICreateProductModel;
    createProductForm: FormGroup;

    ngOnDestroy(): void {
        if (this.createProductSubscription) {
            this.createProductSubscription.unsubscribe();
        }
    }
    
    ngOnInit(): void {
        this.createProductForm = this.formBuilder.group({
            productName: ['Enter product name',[Validators.required, Validators.maxLength(50)]],
            productPrice: [0, [Validators.required, Validators.min(0)]],
            productQuantity: [0, [Validators.required, Validators.min(0)]]
        })
    }

    executeCreateProduct() {
        this.extractFormValues();

        this.createProductSubscription = this.productService.createProduct(this.createProductModel).subscribe( {
            next: () => {
                this.alertService.successfulProductCreation(),
                this.navigateAfterCreation()
            },
            error: (err) => this.alertService.errorAlert(err)
        })
    }

    get productName() { return this.createProductForm.get('productName');}
    get productPrice() { return this.createProductForm.get('productPrice');}
    get productQuantity() { return this.createProductForm.get('productQuantity');}

    private extractFormValues() {
        this.createProductModel.Name = this.createProductForm.controls['productName'].value;
        this.createProductModel.Price = this.createProductForm.controls['productPrice'].value;
        this.createProductModel.Quantity = this.createProductForm.controls['productQuantity'].value;
    }   

    private navigateAfterCreation(): void {
        this.router.navigate(['/AdminProducts']); 
    }
   
}