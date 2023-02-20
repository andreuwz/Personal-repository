import { Component, OnDestroy, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { Subscription } from "rxjs";
import { AlertService } from "src/app/shared/alerts/alertService";
import { ICreateProductModel } from "../productModels/createProductModel";
import { IGetProductAdminModel } from "../productModels/getProductAdminModel";
import { ProductService } from "../product.service";

@Component({
    selector:'edit-product',
    templateUrl: './edit-product.component.html'
})

export class EditProductComponent implements OnInit, OnDestroy{

    constructor(private productService: ProductService,
                private formBuilder: FormBuilder,
                private alertService: AlertService,
                private router: Router) {
                    this.createProductModel = {Name: '', Price: 0, Quantity: 0};
                    this.subscribeConfirmDialogResultEvent();
                }

    private productEditSubscription: Subscription;
    private confirmaDialogSubscription: Subscription;
    editProductForm: FormGroup;
    createProductModel: ICreateProductModel;
    productId: string;

    get formProductName() { return this.editProductForm.get('productName'); }
    get formProductPrice() { return this.editProductForm.get('productPrice');}
    get formProductQuantity() { return this.editProductForm.get('productQuantity'); }

    ngOnDestroy(): void {
      if (this.productEditSubscription) {
        this.productEditSubscription.unsubscribe();
      }
      
      if (this.confirmaDialogSubscription) {
        this.confirmaDialogSubscription.unsubscribe();
      }
    }

    ngOnInit(): void {
        this.router.routeReuseStrategy.shouldReuseRoute = () => false;
        this.router.onSameUrlNavigation = 'reload';

        this.initializeForm();
        this.subscribeEditFormInformationEvent();
    }

    private initializeForm() {
        this.editProductForm = this.formBuilder.group({
            productName: ['Enter product name', [Validators.required, Validators.maxLength(50)]],
            productPrice: ['Enter product price', [Validators.required, Validators.min(0)]],
            productQuantity: ['Enter product quantity', [Validators.required, Validators.min(0)]]
        });
    }

    private extractFormValues() {   
        this.createProductModel.Name = this.editProductForm.controls['productName'].value;
        this.createProductModel.Price = this.editProductForm.controls['productPrice'].value;
        this.createProductModel.Quantity = this.editProductForm.controls['productQuantity'].value;
    }

    private setInitialFormValues(productData: IGetProductAdminModel){
        this.productId = productData.id;

        this.editProductForm.patchValue({
            productName: productData.name,
            productPrice: productData.price,
            productQuantity: productData.quantity
        })
    }

    executeEditProduct() {
        this.extractFormValues();
        this.alertService.confirmationAlertProductEdit();
    }

    private subscribeEditFormInformationEvent() {
        this.productEditSubscription = this.productService.getProductEditEvent().subscribe({
            next: (productData) => this.setInitialFormValues(productData)
        })
    }

    private subscribeConfirmDialogResultEvent() {
        this.confirmaDialogSubscription = this.alertService.getSubjectAlertEvent().subscribe({
            next: (isEventConfirmed) => {
    
                if (isEventConfirmed) {
                    this.productService.updateProduct(this.productId, this.createProductModel).subscribe( {
                        next: ()=> {
                            this.alertService.successfulProductEdit(),
                            this.router.navigate(['/AdminProducts'])
                        },
                        error: (err)=> this.alertService.errorAlert(err)
                    })
                } else {
                    this.router.navigate(['/AdminProducts'])
                }
            }
        })
    }
}