import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { catchError, finalize, mergeMap, switchMap, tap } from 'rxjs';
import { UserStore } from 'src/app/core/authentication/user.store';
import { AccountService } from 'src/app/core/ezgym/account.service';
import { AccountModel } from 'src/app/core/ezgym/models/accout.model';
import { ProfileModel } from 'src/app/core/ezgym/models/profile.model';
import { PreLoaderStore } from 'src/app/shared/components/pre-loader/pre-loader.store';
import { EditProfileComponent } from './edit-profile/edit-profile.component';

@Component({
    selector: 'profile',
    templateUrl: 'profile.component.html',
    styleUrls: ['./profile.component.scss']
})

export class ProfileComponent implements OnInit {
    account = {} as AccountModel
    profile?= {} as ProfileModel

    constructor(
        private activeRoute: ActivatedRoute,
        private router: Router,
        private preloader: PreLoaderStore,
        private accountService: AccountService,
        public dialog: MatDialog
    ) {

    }

    ngOnInit() {
        this.loadProfile()
    }

    loadProfile() {
        this.preloader.show()
        this.activeRoute.params
            .pipe(
                switchMap(params => {
                    return this.accountService.loadAccount(params["accountName"])
                }),
                tap(response => {
                    this.account = response.account
                    this.profile = response.profile
                    this.preloader.close();
                }),
                catchError((error) => {
                    this.router.navigate(['/404'])
                    this.preloader.close();
                    return error
                })
            ).subscribe()
    }

    editProfile() {
        this.dialog.open(EditProfileComponent, {
            data: {
                account: this.account,
                profile: this.profile || {}
            },
            panelClass: 'fullscreen-dialog',
            maxWidth: '935px',
            maxHeight: '100vh',
            height: '100%',
            width: '100%',
        }).afterClosed()
            .subscribe(() => {
                this.loadProfile()
            })
    }
}