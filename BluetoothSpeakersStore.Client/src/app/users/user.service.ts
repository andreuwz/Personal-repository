import { HttpClient, HttpErrorResponse} from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, catchError, Observable, tap, throwError } from "rxjs";
import { SessionService } from "../shared/sessionInfo/session.service";
import { IEditLoggedUserModel } from "./userModels/editLoggedUserModel";
import { IEditUserModel } from "./userModels/editUserModel";
import { IGetUserModel } from "./userModels/getUserModel";
import { IRegisterUserModel } from "./userModels/registerUserModel";

@Injectable({
    providedIn: 'root'
})

export class UserService {
    constructor(private httpClient: HttpClient,
                private sessionService: SessionService) {
        this.editUserEvent = new BehaviorSubject<IGetUserModel>(this.sampleUserModel);
        this.userIdGetCartAlertEvent = new BehaviorSubject<string>('');
    }
    private getAllUsersUrl = 'https://localhost:7193/User';
    private deleteUserUrl = 'https://localhost:7193/User';
    private editUserUrl = 'https://localhost:7193/User';
    private registerUserUrl = 'https://localhost:7193/User/Register';
    private assignUserAdminUrl = 'https://localhost:7193/User/AssignAdminRole/ToUser';
    private unassignUserAdminUrl = 'https://localhost:7193/User/UnassignAdminRole/FromUser';
    private editLoggedUserUrl = 'https://localhost:7193/LoggedUser';
    private getLoggedUserUrl = 'https://localhost:7193/LoggedUser';

    editUserEvent: BehaviorSubject<IGetUserModel>;
    sampleUserModel: IGetUserModel
    userIdGetCartAlertEvent: BehaviorSubject<string>;

    getUserIdGetCartEvent() :Observable<string> {
        return this.userIdGetCartAlertEvent.asObservable();
    }

    getEditUserInfoEvent(): Observable<IGetUserModel> {
        return this.editUserEvent.asObservable();
    }

    promptEditUserInforEvent(userModel: IGetUserModel) {
        this.editUserEvent.next(userModel);
    }

    registerUser(registerUserModel: IRegisterUserModel): Observable<any> {
        this.sessionService.isSecurityNeeded = false;

        return this.httpClient.post(this.registerUserUrl, registerUserModel).
        pipe(
            tap(data => console.log('All:',JSON.stringify(data))),
            catchError(this.handleError)
        );
    }
    
    getAllUsers(): Observable<any> {
        this.sessionService.isSecurityNeeded = true;

        return this.httpClient.get<IGetUserModel[]>(this.getAllUsersUrl).
        pipe(
            tap(data => console.log('All:',JSON.stringify(data))),
            catchError(this.handleError)
        );
    }

    getUserById(id: string): Observable<any> {
        this.sessionService.isSecurityNeeded = true;

        return this.httpClient.get<IGetUserModel>(`${this.getAllUsersUrl}/${id}`).
        pipe (
            tap(data => console.log('All:',JSON.stringify(data))),
            catchError(this.handleError)
        );
    }
    
    deleteUser(id: string): Observable<any> {
        this.sessionService.isSecurityNeeded = true;

        return this.httpClient.delete(`${this.deleteUserUrl}/${id}`).
        pipe(
            tap(data => console.log('All:',JSON.stringify(data))),
            catchError(this.handleError)
        );
    }
    
    editUser(id: string, editUserModel: IEditUserModel): Observable<any> {
        this.sessionService.isSecurityNeeded = true;

        return this.httpClient.put(`${this.editUserUrl}/${id}`, editUserModel).
        pipe (
            tap(data => console.log('All:',JSON.stringify(data))),
            catchError(this.handleError)
        );
    }

    assignUserAsAdmin(id: string): Observable<any>{
        this.sessionService.isSecurityNeeded = true;

        return this.httpClient.post(`${this.assignUserAdminUrl}/${id}`, null)
        .pipe (
            tap(data => console.log('All:',JSON.stringify(data))),
            catchError(this.handleError)
        );
    }

    unassignUserAsAdmin(id: string): Observable<any> {
        this.sessionService.isSecurityNeeded = true;

        return this.httpClient.post(`${this.unassignUserAdminUrl}/${id}`, null)
        .pipe (
            tap(data => console.log('All:',JSON.stringify(data))),
            catchError(this.handleError)
        );
    }

    editLoggedUser(loggedUserModel: IEditLoggedUserModel): Observable<any> {
        this.sessionService.isSecurityNeeded = true;

        return this.httpClient.put(this.editLoggedUserUrl, loggedUserModel)
        .pipe(
            tap(data => console.log('All:',JSON.stringify(data))),
            catchError(this.handleError)
        );
    }

    getLoggedUser(): Observable<any> {
        this.sessionService.isSecurityNeeded = true;
        
        return this.httpClient.get(this.getLoggedUserUrl)
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
            errorMessage = `${err.error}`
        }
        console.error(errorMessage);
        return throwError(()=> errorMessage);
    }
}