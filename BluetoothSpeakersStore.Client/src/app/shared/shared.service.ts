import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, catchError, Observable, tap, throwError } from "rxjs";
import { SessionService } from "./sessionInfo/session.service";

@Injectable({
    providedIn: 'root'
})
export class SharedService {
    
    constructor(private httpClient : HttpClient,
                private sessionService: SessionService) {
        this.userIdGetCartAlertEvent = new BehaviorSubject<string>('');
    }
    
    userIdGetCartAlertEvent: BehaviorSubject<string>;
    private getLoggedUserUrl = 'https://localhost:7024/LoggedUser';
    
    isLoggedUserMasterAdmin() :boolean {
        if (this.sessionService.isLoggedUserMasterAdmin()) {
            return true;
        }
        
        return false;
    }

    getUserIdGetCartEvent() :Observable<string> {
        return this.userIdGetCartAlertEvent.asObservable();
    }

    getLoggedUser(): Observable<any> {
        this.sessionService.isSecurityNeeded = true;
        
        return this.httpClient.get(this.getLoggedUserUrl)
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