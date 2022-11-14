import { Component, OnDestroy, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { AlertService } from "src/app/shared/alerts/alertService";
import { SessionService } from "src/app/shared/sessionInfo/session.service";
import { IRegisterUserModel } from "../userModels/registerUserModel";
import { UserService } from "../user.service";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Subscription } from "rxjs";
import { ILoginOutputModel } from "src/app/authentication/login/loginOuputModel";
import { CookieService } from "ngx-cookie-service";
import { EncryptionService } from "src/app/authentication/encryption.service";

@Component({
    selector: 'register-user',
    templateUrl: 'register-user.component.html'
})

export class RegisterUserComponent implements OnInit, OnDestroy{

    constructor(private formBuilder: FormBuilder,
                private userService: UserService,
                private router: Router,
                private alertService: AlertService,
                private sessionService: SessionService,
                private cookieService: CookieService,
                private encryptionService: EncryptionService) {
    this.registerUserModel = {userName:'Enter username', firstname: 'Enter first name', lastname: 'Enter last name', 
    email: 'Enter email', password: '', repeatPassword: '', balance: 0};
    this.registerOutputModel = {accessToken: '', refreshToken: '', roles: ''};              
    }

    private registerOutputModel: ILoginOutputModel;
    private registerUserSubscription: Subscription;
    registerUserForm: FormGroup;
    registerUserModel: IRegisterUserModel;

    ngOnDestroy(): void {
        if (this.registerUserSubscription) {
            this.registerUserSubscription.unsubscribe();
        }
    }

    ngOnInit(): void {
        this.registerUserForm = this.formBuilder.group({
            userName: [this.registerUserModel.userName, [Validators.required, Validators.maxLength(15)]],
            firstname: [this.registerUserModel.firstname, [Validators.required, Validators.maxLength(25), Validators.pattern(/^[A-Za-z ]+$/)]],
            lastname: [this.registerUserModel.lastname, [Validators.required, Validators.maxLength(25), Validators.pattern(/^[A-Za-z ]+$/)]],
            password: [this.registerUserModel.password,  [Validators.required, Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$/)]],
            repeatPassword: [this.registerUserModel.repeatPassword,  [Validators.required, Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$/)]],
            email: [this.registerUserModel.email, [Validators.required, Validators.minLength(5), Validators.email]],
            balance: [0, [Validators.required, Validators.min(0)]]
        })
    }

    get userName() {  return this.registerUserForm.get('userName');}
    get firstname() {  return this.registerUserForm.get('firstname');}
    get lastname() {  return this.registerUserForm.get('lastname');}
    get password() {  return this.registerUserForm.get('password');}
    get repeatPassword() {  return this.registerUserForm.get('repeatPassword');}
    get email() {  return this.registerUserForm.get('email');}
    get balance() {  return this.registerUserForm.get('balance');}
    
    executeRegistration() {
        this.registerUserModel = this.registerUserForm.value;

        this.registerUserSubscription = this.userService.registerUser(this.registerUserModel).subscribe({
            next: (registerOutputInfo)=> {
                this.registerOutputModel = registerOutputInfo;
                this.SetCookieAfterLogin(this.registerOutputModel.accessToken, this.registerOutputModel.refreshToken);
                this.sessionService.setLoggedInformation(true, 'RegularUser')
                this.executeRegsiterProcedures();
            },  
            error: (err)=>this.alertService.errorAlert(err)
        })
    }

    private executeRegsiterProcedures() {
        this.alertService.successRegisterAlert();
        this.router.navigate(['/Products']);
    }

    SetCookieAfterLogin(accessToken: string, refreshToken: string) {
        let authCookie = 'Bearer ' + accessToken;
        let refreshCookie = refreshToken;
        
        this.cookieService.set('cookieAlpha', authCookie, {sameSite: "Strict", secure: true});
        this.cookieService.set('cookieRomeo', refreshCookie, {sameSite: "Strict", secure: true});
    }
}