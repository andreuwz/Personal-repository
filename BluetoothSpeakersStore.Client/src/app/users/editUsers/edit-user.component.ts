import { Component, OnDestroy, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { AlertService } from "src/app/shared/alerts/alertService";
import { IEditUserModel } from "../userModels/editUserModel";
import { IGetUserModel } from "../userModels/getUserModel";
import { UserService } from "../user.service";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Subscription } from "rxjs";

@Component({
    selector:'edit-user',
    templateUrl: './edit-user.component.html'
})

export class EditUserComponent implements OnInit, OnDestroy{
    
    constructor (private userService: UserService,
                private router: Router,
                private alertService: AlertService,
                private formBuilder: FormBuilder) {
           this.userModel = {email: '', firstname: '', lastname: '', userName: '' }
           this.subscribeAlertConfirmation();
    }
    
    private userEditConfirmationSubscription: Subscription
    private alertEventSubscription: Subscription;
    private userEditSubscription: Subscription
    userModel: IEditUserModel;
    editUserForm:FormGroup;
    private id: string;

    private subscribeAlertConfirmation() {
        this.alertEventSubscription = this.alertService.getSubjectAlertEvent().subscribe({
            next: () => {
                this.executeUserEdit();
            }
        });
    }

    private executeUserEdit() {
        this.userEditSubscription = this.userService.editUser(this.id, this.userModel).subscribe({
            next: () => this.reloadResources(),
            error: (err) => this.alertService.errorAlert(err)
        });
    }
    
    ngOnDestroy(): void {
        if (this.userEditConfirmationSubscription) {
            this.userEditConfirmationSubscription.unsubscribe();
        }

        if (this.alertEventSubscription) {
            this.alertEventSubscription.unsubscribe();
        }

        if(this.userEditSubscription){
            this.userEditSubscription.unsubscribe();
        }
    }

    ngOnInit(): void {
        this.initializeForm();
        this.subscribeGetUserEditInfo();
    }
    
    private subscribeGetUserEditInfo() {
        this.userEditConfirmationSubscription = this.userService.getEditUserInfoEvent().subscribe({
            next: (userInfo) => {
                this.setEditFormEventValues(userInfo)
            }
        });
    }

    private initializeForm() {
        this.editUserForm = this.formBuilder.group({
            userName: ['Enter username', [Validators.required, Validators.maxLength(15)]],
            firstname: ['Enter first name', [Validators.required, Validators.maxLength(25), Validators.pattern(/^[A-Za-z ]+$/)]],
            lastname: ['Enter last name', [Validators.required, Validators.maxLength(25), Validators.pattern(/^[A-Za-z ]+$/)]],
            email: ['Enter email', [Validators.required, Validators.minLength(5), Validators.email]]
        });
    }

    get userName() { return this.editUserForm.get('userName')}
    get firstname() { return this.editUserForm.get('firstname')}
    get lastname() { return this.editUserForm.get('lastname')}
    get email() { return this.editUserForm.get('email')}

    private setEditFormEventValues(userInfo: IGetUserModel) {
      this.id = userInfo.id;

      this.editUserForm.patchValue({
        userName: userInfo.userName,
        firstname: userInfo.firstname,
        lastname: userInfo.lastname,
        email: userInfo.email
      })
    }
   
    executeEditUser() {
        this.extractFormValues();
        this.alertService.confirmationAlertUserEdit();
    }

    private extractFormValues() {
        this.userModel.userName = this.editUserForm.controls['userName'].value;
        this.userModel.firstname = this.editUserForm.controls['firstname'].value;
        this.userModel.lastname = this.editUserForm.controls['lastname'].value;
        this.userModel.email = this.editUserForm.controls['email'].value;
    }

    private reloadResources(): void {
        this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
        this.router.navigate(['/Users']); // navigate to same route
    }); 
    }
}