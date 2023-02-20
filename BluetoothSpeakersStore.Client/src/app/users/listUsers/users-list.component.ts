import { Component, OnDestroy, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { AlertService } from "../../shared/alerts/alertService";
import { UserService } from "../user.service";
import { IGetUserModel } from "../userModels/getUserModel";
import { Subscription } from "rxjs";
import { SharedService } from "src/app/shared/shared.service";

@Component({
    selector: 'user-list',
    templateUrl: './user-list.component.html',
    styleUrls: ['user-list.component.css']
})

export class UserListComponent implements OnInit, OnDestroy{
    constructor(private userService: UserService,
                private sharedSerice: SharedService,
                          private router: Router,
               private alertService: AlertService) {
            this.subscribeUserDeleteEvent();
        }
    
    private userId: string;
    private getAllUsersSubscription: Subscription;
    private userAssignSubscription: Subscription;
    private userUnassignSubscription: Subscription;
    private getUserSubscription: Subscription;
    private userDeleteSubscription: Subscription;
    userList: IGetUserModel[] = [];

    ngOnInit(): void {
        this.executeGetAllUsers();
    }

    private executeGetAllUsers() {
        this.getAllUsersSubscription = this.userService.getAllUsers().subscribe({
            next: userData => this.userList = userData
        });
    }

    ngOnDestroy(): void {
       if (this.getAllUsersSubscription) {
        this.getAllUsersSubscription.unsubscribe();
       }

       if (this.userAssignSubscription) {
        this.userAssignSubscription.unsubscribe();
       }
       
       if (this.userUnassignSubscription) {
        this.userUnassignSubscription.unsubscribe();
       }
       
       if (this.getUserSubscription) {
        this.getUserSubscription.unsubscribe();
       }
       
       if (this.userDeleteSubscription) {
        this.userDeleteSubscription.unsubscribe();
       }
    }

    isLoggedUserMasterAdmin() :boolean {
        if (this.sharedSerice.isLoggedUserMasterAdmin()) {
            return true;
        }
        
        return false;
    }
    
    executeGetUserById(id: string) {
        this.getUserSubscription = this.userService.getUserById(id).subscribe({
            next: (userData)=> {
                this.userService.promptEditUserInforEvent(userData),
                this.router.navigate(['/EditUser'])
            },
            error: (err)=> this.alertService.errorAlert(err)
        })
    }

    executeDeleteUser(id:string) {
        this.userId = id;
        this.alertService.confirmationAlertUserDelete();
    }

    private subscribeUserDeleteEvent() {
        this.userDeleteSubscription = this.alertService.getSubjectAlertEvent().subscribe({
            next: (isConfirmed) => {
                if (isConfirmed) {
                    this.userService.deleteUser(this.userId).subscribe({
                        next: () => this.reloadCurrentResources()
                    });
                }
                else {
                    this.reloadCurrentResources();
                }
            }
        });
    }

    private reloadCurrentResources(): void {
           // save current route first
        this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
        this.router.navigate(['/Users']); // navigate to same route
    }); 
    }

    executeUserAdminRoleAssign(id: string) {
        this.userAssignSubscription = this.userService.assignUserAsAdmin(id).subscribe( {
            next: ()=> {
                this.alertService.successAdminRoleAssignAlert();
                this.reloadCurrentResources();
            },
            error: (err)=> this.alertService.errorAlert(err)
        })
    }

    executeUserAdminRoleUnassign(id: string) {
        this.userUnassignSubscription = this.userService.unassignUserAsAdmin(id).subscribe( {
            next: ()=> {
                this.alertService.successAdminRoleUnassignAlert();
                this.reloadCurrentResources();
            },
            error: (err)=> this.alertService.errorAlert(err)
        })
    }

    isUserInAdminRole(roles: string):boolean {
        if (roles.includes('Administrator') || roles.includes('MasterAdmin')) {
            return true;
        }
        return false;
    }

    executeGetUserIdForCartInfo(id: string) {
        this.sharedSerice.userIdGetCartAlertEvent.next(id);
        this.router.navigate(['/UserCart']);
    }
}