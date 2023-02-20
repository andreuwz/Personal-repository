import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { catchError, Observable, tap, throwError } from "rxjs";
import { SessionService } from "../shared/sessionInfo/session.service";
import { IGetUserCartModel } from "./shoppingCartModels/getUserCartModel";
import { IInCartProductModel } from "./shoppingCartModels/inCartProductsModel";

@Injectable()
export class ShoppingCartService {
    constructor(private httpClient: HttpClient,
                private sessionService: SessionService) {}
    
    private getLoggedUserCartUrl = 'https://localhost:7024/Cart/LoggedUser';
    private getLoggedUserCartProductsUrl= 'https://localhost:7024/Cart/LoggedUser/ProductsInCart';
    private removeProductFromCartUrl = 'https://localhost:7024/Cart/LoggedUser/ProductInCart';
    private cartCheckoutUrl = 'https://localhost:7024/Cart/LoggedUser/CartCheckout';
    private removeLoggedUserCartUrl = 'https://localhost:7024/Cart/LoggedUser';
    private getUserCartUrl = 'https://localhost:7024/Cart/Admin';
    private getUserProductsInCartUrl = 'https://localhost:7024/Cart/Admin/ProductsInCart';
    private deleteCartByIdUrl= 'https://localhost:7024/Cart/Admin';

    removeCartById(id: string) :Observable<any>{
        this.sessionService.isSecurityNeeded = true;

        return this.httpClient.delete(`${this.deleteCartByIdUrl}/${id}`)
        .pipe(
            tap(data => console.log('All:',JSON.stringify(data))),
            catchError(this.handleError)
        );
    }

    getUserInCartProducts(id: string) :Observable<IInCartProductModel[]> {
        this.sessionService.isSecurityNeeded = true;
        
        return this.httpClient.get<IInCartProductModel[]>(`${this.getUserProductsInCartUrl}/${id}`)
        .pipe(
            tap(data => console.log('All:',JSON.stringify(data))),
            catchError(this.handleError)
        );
    }

    getUserCart(id: string) :Observable<IGetUserCartModel> {
        this.sessionService.isSecurityNeeded = true;

        return this.httpClient.get<IGetUserCartModel>(`${this.getUserCartUrl}/${id}`)
        .pipe(
            tap(data => console.log('All:',JSON.stringify(data))),
            catchError(this.handleError)
        );
    }

    removeLoggedUserCart() :Observable<any>{
        this.sessionService.isSecurityNeeded = true;

        return this.httpClient.delete(this.removeLoggedUserCartUrl)
        .pipe(
            tap(data => console.log('All:',JSON.stringify(data))),
            catchError(this.handleError)
        );
    }

    getLoggedUserCart() :Observable<IGetUserCartModel>{
        this.sessionService.isSecurityNeeded = true;

        return this.httpClient.get<IGetUserCartModel>(this.getLoggedUserCartUrl)
        .pipe(
            tap(data => console.log('All:',JSON.stringify(data))),
            catchError(this.handleError)
        );
    }

    getLoggedUserInCartProducts() :Observable<IInCartProductModel[]>{
        this.sessionService.isSecurityNeeded = true;

        return this.httpClient.get<IInCartProductModel[]>(this.getLoggedUserCartProductsUrl)
        .pipe(
            tap(data => console.log('All:',JSON.stringify(data))),
            catchError(this.handleError)
        );
    }

    removeProductFromCart(id: string) :Observable<any> {
        this.sessionService.isSecurityNeeded = true;

        return this.httpClient.delete(`${this.removeProductFromCartUrl}/${id}`)
        .pipe(
            tap(data => console.log('All:',JSON.stringify(data))),
            catchError(this.handleError)
        );
    }

    cartCheckout() :Observable<any> {
        this.sessionService.isSecurityNeeded = true;
        
        return this.httpClient.post(this.cartCheckoutUrl, null)
        .pipe (
            tap(data => console.log('All:',JSON.stringify(data))),
            catchError(this.handleError)
        );
    }

    private handleError(err: HttpErrorResponse) {
        let errorMessage = '';
    
        if(err.error instanceof ErrorEvent) {
            errorMessage = `An error occured: ${err.error.message}`;
        }
        else {
            errorMessage = `${err.error}`
        }
        console.error(errorMessage);
        return throwError(()=> errorMessage);
    }
}