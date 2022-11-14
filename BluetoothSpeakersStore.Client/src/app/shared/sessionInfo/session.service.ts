import { Injectable } from "@angular/core";
import { CookieService } from "ngx-cookie-service";
import { Observable, Subject } from "rxjs";
import { EncryptionService } from "src/app/authentication/encryption.service";

@Injectable({
    providedIn: 'root'
})

export class SessionService {
    loggedInEvent: Subject<boolean>;
    private _isSecurityNeeded: boolean;
    authCookie: string;
    refreshCookie: string;

    constructor(private encryptionService: EncryptionService,
                private cookieService: CookieService) {
        this.loggedInEvent = new Subject<boolean>();
    }

    extractCookieData() {
        this.authCookie = this.cookieService.get('cookieAlpha');
        this.refreshCookie = this.cookieService.get('cookieRomeo');
    }

    getLoggedInformation(): Observable<boolean>{
        return this.loggedInEvent.asObservable();
    }
    
    setLoggedInformation(isLogged:boolean, roles:string): void {
       if (isLogged) {
        localStorage.setItem('infoa', this.encryptionService.encryptionAES('true'));
        localStorage.setItem('infor', this.encryptionService.encryptionAES(roles));
        this.loggedInEvent.next(true);
       } 
       else if(!isLogged) {
        localStorage.setItem('infoa', this.encryptionService.encryptionAES('false'));
        localStorage.setItem('infor', this.encryptionService.encryptionAES(''));
        this.loggedInEvent.next(false);
       }
    }

    public get isSecurityNeeded(): boolean {
        return this._isSecurityNeeded;
    }

    public set isSecurityNeeded(value: boolean) {
        this._isSecurityNeeded = value;
    }
}