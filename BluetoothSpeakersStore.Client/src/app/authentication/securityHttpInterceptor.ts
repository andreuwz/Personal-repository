import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { SessionService } from "../shared/sessionInfo/session.service";

@Injectable ()
export class SecurityHttpInterceptor implements HttpInterceptor{
    constructor(private sessionService: SessionService) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        if (this.sessionService.isSecurityNeeded == true) {
            this.sessionService.extractCookieData();

            const modifiedRequest = req.clone({
                headers: req.headers
                .set("Authorization", this.sessionService.authCookie)
                .set("RefreshAuth", this.sessionService.refreshCookie)
                
            });
            return next.handle(modifiedRequest);
        }
        
        return next.handle(req);        
    }
}