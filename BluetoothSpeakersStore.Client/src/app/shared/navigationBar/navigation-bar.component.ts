import { Component, OnDestroy, OnInit } from "@angular/core";
import { Subscription } from "rxjs";
import { EncryptionService } from "src/app/authentication/encryption.service";
import { SessionService } from "../sessionInfo/session.service";

@Component({
    selector: 'nav-bar',
    templateUrl: './navigation-bar.component.html',
    styleUrls: ['navigation-bar.component.css']
})

export class NavigationBarComponent implements OnInit, OnDestroy{

    constructor(private sessionService: SessionService,
                private encryptionService :EncryptionService) {
        this.subscribeLoggedInformation();
    }

    private getLoggedInfoSubscription: Subscription;
    isLoggedIn: boolean = false;
    isLoggedInAdmin: boolean = false
    
    ngOnDestroy(): void {
        if (this.getLoggedInfoSubscription) {
            this.getLoggedInfoSubscription.unsubscribe();
        }
    }

    ngOnInit(): void {
        if (this.encryptionService.decryptionAES(localStorage.getItem('infoa')!).includes('true')) {
            this.isLoggedIn = true;

            if (this.encryptionService.decryptionAES(localStorage.getItem('infor')!).includes('Administrator')
                || this.encryptionService.decryptionAES(localStorage.getItem('infor')!).includes('MasterAdmin')) {
            this.isLoggedInAdmin = true;
            }
        }
    }

    private subscribeLoggedInformation() {
        this.getLoggedInfoSubscription = this.sessionService.getLoggedInformation().subscribe({
            next: (eventData) => this.handleLoggingEventData(eventData)
        });
    }

    private handleLoggingEventData(value: boolean): void {
        if (value) {
            this.isLoggedIn = true;
            if
            (
                this.encryptionService.decryptionAES(localStorage.getItem('infor')!).includes('Administrator') ||
                this.encryptionService.decryptionAES(localStorage.getItem('infor')!).includes('MasterAdmin')
            ) 
                {
                    this.isLoggedInAdmin = true;
                }
                else {
                    this.isLoggedInAdmin = false;
                }
        }
        else {
            this.isLoggedIn = false;
            this.isLoggedInAdmin = false;
        }
    }
}