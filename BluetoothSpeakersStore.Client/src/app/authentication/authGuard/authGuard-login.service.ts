import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { AlertService } from "src/app/shared/alerts/alertService";
import { EncryptionService } from "../encryption.service";

@Injectable({
    providedIn: 'root'
})

export class AuthGuardLogIn implements CanActivate{
    constructor(private router: Router,
                private alertService: AlertService,
                private encryptionService: EncryptionService) {}
    
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
        if (this.encryptionService.decryptionAES(localStorage.getItem('infoa')!) == "true") {
            return true
        }
        
        this.alertService.authGuardUnauthenticatedAlert();
        this.router.navigate(['/Login']);
        return false;
    }

}