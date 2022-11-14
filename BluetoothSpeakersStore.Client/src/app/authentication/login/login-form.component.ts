import { Component, OnDestroy, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { Subscription } from "rxjs";
import { AlertService } from "../../shared/alerts/alertService";
import { SessionService } from "../../shared/sessionInfo/session.service";
import { LoginService } from "../login.service";
import { LoginModel } from "./loginModel";
import { EncryptionService } from "../encryption.service";
import { ILoginOutputModel } from "./loginOuputModel";
import { CookieService } from "ngx-cookie-service";

@Component ({
    selector: 'log-form',
    templateUrl: './login-form.component.html'
})

export class LoginFormComponent implements OnInit, OnDestroy{

    private loginSubscription: Subscription;
    private logoutSubscription: Subscription;
    signupForm: FormGroup;
    loginModel = new LoginModel();
    isLoggedIn: boolean = false;
    private loginOutputInfo: ILoginOutputModel;

    get userName() { return this.signupForm.get('userName')}
    get password() { return this.signupForm.get('password')}

    constructor(private loginService: LoginService,
                private sessionService: SessionService,
                private alertService: AlertService,
                private formBuilder: FormBuilder,
                private router: Router,
                private encryptionService: EncryptionService,
                private cookieService: CookieService) {
                    this.loginOutputInfo = {accessToken: '', roles: '', refreshToken: ''};
    }
    ngOnDestroy(): void {
        if (this.loginSubscription) {
            this.loginSubscription.unsubscribe();
        }

        if (this.logoutSubscription) {
            this.logoutSubscription.unsubscribe();
        }
    }
    
    ngOnInit(): void {
        this.router.routeReuseStrategy.shouldReuseRoute = () => false;
        this.router.onSameUrlNavigation = 'reload';
        
        this.initializeForm();
        this.checkLoginStatus();
        this.setFormFieldsForLoggedIn();
    }

    private checkLoginStatus() {
        console.log(this.encryptionService.decryptionAES(localStorage.getItem('infoa')!));
        if (this.encryptionService.decryptionAES(localStorage.getItem('infoa')!).includes('true')) {
            this.isLoggedIn = true;
        }
    }

    private setFormFieldsForLoggedIn() {
        if (this.isLoggedIn) {
            this.signupForm.patchValue({
                userName: "Fields inactive - you are logged in!"
            })
        }
    }

    private initializeForm() {
        this.signupForm = this.formBuilder.group({
            userName: ['Enter username', Validators.required],
            password: ['',Validators.required]
        });
    }

    executeLogIn() {
        this.ExtractFormData();

        this.loginSubscription = this.loginService.logIn(this.loginModel).subscribe({
            next: (loggingData) => 
            {
                this.loginOutputInfo = loggingData;
                this.sessionService.setLoggedInformation(true,this.loginOutputInfo.roles),
                this.isLoggedIn = true,
                this.SetCookieAfterLogin(this.loginOutputInfo.accessToken, this.loginOutputInfo.refreshToken),
                this.alertService.successLoginAlert(),
                this.router.navigate(['/Products'])
            },
            error: err => {
                this.alertService.errorAlert(err)
            }
        })
    }
    
    private ExtractFormData() {
        this.loginModel.username = this.signupForm.controls['userName'].value;
        this.loginModel.password = this.signupForm.controls['password'].value;
    }

    executeLogOut() {
        this.cookieService.delete('cookieAlpha');
        this.cookieService.delete('cookieRomeo');
        this.sessionService.setLoggedInformation(false, ''),
        this.isLoggedIn = false,
        this.alertService.successLogoutAlert(),
        this.router.navigate(['/Products'])
    }

    SetCookieAfterLogin(accessToken: string, refreshToken: string) {
        let authCookie = 'Bearer ' + accessToken;
        let refreshCookie = refreshToken;
        
        this.cookieService.set('cookieAlpha', authCookie, {sameSite: "Strict", secure: true});
        this.cookieService.set('cookieRomeo', refreshCookie, {sameSite: "Strict", secure: true});
    }
}