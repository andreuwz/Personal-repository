import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { AlertService } from "src/app/shared/alerts/alertService";
import { EncryptionService } from "../encryption.service";

@Injectable({
    providedIn: 'root'
})

export class AuthGuardLogOut implements CanActivate{
    constructor(private alertService: AlertService,
                private router: Router,
                private encryptionService: EncryptionService) {}

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
        
        if (this.encryptionService.decryptionAES(localStorage.getItem('isLogged')!)=='' 
        || this.encryptionService.decryptionAES(localStorage.getItem('isLogged')!) == null
        || this.encryptionService.decryptionAES(localStorage.getItem('isLogged')!)== 'false') {
            return true;
        }

        this.alertService.authGuardForbiddenAlert();
        this.router.navigate(['/Products']);
        return false;
    }

}