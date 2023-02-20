import { HttpClient, HttpErrorResponse} from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, catchError, Observable, tap, throwError } from "rxjs";
import { SessionService } from "../shared/sessionInfo/session.service";
import { ICreateProductModel } from "./productModels/createProductModel";
import { IGetProductAdminModel } from "./productModels/getProductAdminModel";
import { IGetProductModel } from "./productModels/getProductModel";

@Injectable()
export class ProductService {
  constructor(private httpClient: HttpClient,
              private sessionService: SessionService) {
    this.productEditEvent = new BehaviorSubject<IGetProductAdminModel>(this.sampleProductAdminModel);
  }
  
  private getAllProductsUrl = 'https://localhost:7024/Product';
  private getAllProductsAsAdminUrl = 'https://localhost:7024/Product/Admin';
  private createProductUrl = 'https://localhost:7024/Product';
  private deleteProductUrl = 'https://localhost:7024/Product';
  private updateProductUrl = 'https://localhost:7024/Product';
  private getAdminProductUrl = 'https://localhost:7024/Product/Admin';
  private addProductToCartUrl = 'https://localhost:7024/Product/AddToCart';

  productEditEvent: BehaviorSubject<IGetProductAdminModel>;
  sampleProductAdminModel: IGetProductAdminModel;

  getProductEditEvent():Observable<IGetProductAdminModel> {
    return this.productEditEvent.asObservable();
  }

  promptProductEditEvent(productModel: IGetProductAdminModel) {
    this.productEditEvent.next(productModel);
  }

  addProductToCart(id: string, quantity:number) :Observable<any> {
    this.sessionService.isSecurityNeeded = true;

    return this.httpClient.post(`${this.addProductToCartUrl}/${id}/Quantity/${quantity}`,null)
    .pipe(
        tap(data => console.log('All:',JSON.stringify(data))),
        catchError(this.handleError)
    );
  }
  
  getAdminProductById(id: string) :Observable<any>{
    this.sessionService.isSecurityNeeded = true;

    return this.httpClient.get<IGetProductAdminModel>(`${this.getAdminProductUrl}/${id}`)
    .pipe(
        tap(data => console.log('All:',JSON.stringify(data))),
        catchError(this.handleError)
    );
  }
  
  getAllProducts():Observable<any> {
    this.sessionService.isSecurityNeeded = false;

    return this.httpClient.get<IGetProductModel[]>(this.getAllProductsUrl)
    .pipe(
        tap(data => console.log('All:',JSON.stringify(data))),
        catchError(this.handleError)
    );
}

   getAllProductsAsAdmin(): Observable<any>{
    this.sessionService.isSecurityNeeded = true;
    
    return this.httpClient.get<IGetProductAdminModel[]>(this.getAllProductsAsAdminUrl)
    .pipe(
        tap(data => console.log('All:',JSON.stringify(data))),
        catchError(this.handleError)
    );
}

    createProduct(createProductModel: ICreateProductModel) :Observable<any> {
        this.sessionService.isSecurityNeeded = true;

        return this.httpClient.post(this.createProductUrl, createProductModel)
        .pipe(
            tap(data => console.log('All:',JSON.stringify(data))),
            catchError(this.handleError)
        );
    }

    deleteProduct(id:string): Observable<any> {
        this.sessionService.isSecurityNeeded = true;

        return this.httpClient.delete(`${this.deleteProductUrl}/${id}`)
        .pipe(
            tap(data => console.log('All:',JSON.stringify(data))),
            catchError(this.handleError)
        );
    }
    
    updateProduct(id:string, productModel: ICreateProductModel): Observable<any> {
        this.sessionService.isSecurityNeeded = true;

        return this.httpClient.put(`${this.updateProductUrl}/${id}`, productModel)
        .pipe(
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