import { HttpClient, HttpErrorResponse} from "@angular/common/http";
import { Injectable } from "@angular/core";
import { catchError, Observable, tap, throwError } from "rxjs";
import { SessionService } from "../shared/sessionInfo/session.service";
import { LoginModel } from "./login/loginModel";

@Injectable( {
    providedIn: 'root'
})

export class LoginService {

    constructor(private httpClient: HttpClient,
                private sessionService: SessionService) {}

    private logInUrl = 'https://localhost:7193/User/Login';

    logIn(loginModel: LoginModel): Observable<any> {
        this.sessionService.isSecurityNeeded = false;

        return this.httpClient.post(this.logInUrl,loginModel)
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
            errorMessage = `${err.error}`;
        }
        console.error(errorMessage);
        return throwError(()=> errorMessage);
    }
}