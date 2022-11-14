import { Component, OnDestroy, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { Subscription } from "rxjs";
import { AlertService } from "src/app/shared/alerts/alertService";
import { SessionService } from "src/app/shared/sessionInfo/session.service";
import { UserService } from "../user.service";
import { IEditLoggedUserModel } from "../userModels/editLoggedUserModel";
import { IGetUserModel } from "../userModels/getUserModel";


@Component({
    selector:'edit-logged-user',
    templateUrl: './edit-loggeduser.component.html'
})

export class EditLoggedUserComponent implements OnInit, OnDestroy{
    constructor(private formBuilder: FormBuilder,
                private userService: UserService,
                private alertService: AlertService,
                private router: Router,
                private sessionService: SessionService) {
        this.loggedUserModel= {userName: 'Edit your username', firstname: 'Edit your first name', 
        lastname: 'Edit your last name', password: '', repeatPassword: '', 
        email: 'Edit your email', balance: 0}

        this.getLoggedUser = {id: '',userName: '', firstname: '', lastname: '', email: '', createdAt: '', modifiedAt: '', userRoles: '', balance: 0};

        this.subscribeGetLoggedUserInfo();
    }

    private getLoggedUserSubscription: Subscription;
    private editLoggedUserSubscription: Subscription;
    getLoggedUser: IGetUserModel
    editLoggedUserForm:FormGroup
    loggedUserModel: IEditLoggedUserModel

    private subscribeGetLoggedUserInfo() {
        this.getLoggedUserSubscription = this.userService.getLoggedUser().subscribe({
            next: (loggedUser) => {
                this.patchFormValues(loggedUser);
            }
        });
    }

    ngOnDestroy(): void {
        if (this.getLoggedUserSubscription) {
            this.getLoggedUserSubscription.unsubscribe();
        }

        if (this.editLoggedUserSubscription) {
            this.editLoggedUserSubscription.unsubscribe();
        }
    }
    
    ngOnInit(): void {
        this.initializeForm();
    }

    private initializeForm() {
        this.editLoggedUserForm = this.formBuilder.group({
            userName: ['', [Validators.required, Validators.maxLength(15)]],
            firstname: ['', [Validators.required, Validators.maxLength(25), Validators.pattern(/^[A-Za-z ]+$/)]],
            lastname: ['', [Validators.required, Validators.maxLength(25), Validators.pattern(/^[A-Za-z ]+$/)]],
            email: ['', [Validators.required, Validators.minLength(5), Validators.email]],
            password: [this.loggedUserModel.password, [Validators.required, Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$/)]],
            repeatPassword: [this.loggedUserModel.repeatPassword, [Validators.required, Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$/)]],
            balance: [this.loggedUserModel.balance, [Validators.required, Validators.pattern(/^(-?)(0|([1-9][0-9]*))(\\.[0-9]+)?$/)]]
        });
    }

    get userName() { return this.editLoggedUserForm.get('userName')}
    get firstname() { return this.editLoggedUserForm.get('firstname')}
    get lastname() { return this.editLoggedUserForm.get('lastname')}
    get email() { return this.editLoggedUserForm.get('email')}
    get password() { return this.editLoggedUserForm.get('password')}
    get repeatPassword() { return this.editLoggedUserForm.get('repeatPassword')}
    get balance() { return this.editLoggedUserForm.get('balance')}

    executeLoggedUserAccountEdit() {
        this.extractFormValues();
        this.editLoggedUserSubscription = this.userService.editLoggedUser(this.loggedUserModel).subscribe({
            next: ()=> {
                this.alertService.successLoggedUserEdit(),
                this.sessionService.setLoggedInformation(false, ''),
                this.router.navigate(['/Products']);
            },
            error: (err)=> this.alertService.errorAlert(err)
        })
    }

    private patchFormValues(getUserModel: IGetUserModel) {
        this.editLoggedUserForm.patchValue({
            userName: getUserModel.userName,
            firstname: getUserModel.firstname,
            lastname: getUserModel.lastname,
            email: getUserModel.email,
            balance: getUserModel.balance
        })
    }

    private extractFormValues() {
        this.loggedUserModel.userName = this.editLoggedUserForm.controls['userName'].value;
        this.loggedUserModel.firstname = this.editLoggedUserForm.controls['firstname'].value;
        this.loggedUserModel.lastname = this.editLoggedUserForm.controls['lastname'].value;
        this.loggedUserModel.email = this.editLoggedUserForm.controls['email'].value;
        this.loggedUserModel.password = this.editLoggedUserForm.controls['password'].value;
        this.loggedUserModel.repeatPassword = this.editLoggedUserForm.controls['repeatPassword'].value;
        this.loggedUserModel.balance = this.editLoggedUserForm.controls['balance'].value;
    }
}