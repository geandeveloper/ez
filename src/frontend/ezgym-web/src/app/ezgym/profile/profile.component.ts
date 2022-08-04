import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { catchError, switchMap, tap } from 'rxjs';
import { UserStore } from 'src/app/core/authentication/user.store';
import { AccountService } from 'src/app/core/ezgym/account.service';
import { AccountModel } from 'src/app/core/ezgym/models/accout.model';
import { ProfileModel } from 'src/app/core/ezgym/models/profile.model';
import { Store } from 'src/app/core/state/store';
import { PreLoaderStore } from 'src/app/shared/components/pre-loader/pre-loader.store';
import { EzGymComponentStore } from '../ezgym.component.store';
import { EditProfileComponent } from './edit-profile/edit-profile.component';

interface ProfileComponentState {
    account: AccountModel,
    profile: ProfileModel,
    ui: {
        isOwner: boolean
    }
}

@Component({
    selector: 'profile',
    templateUrl: 'profile.component.html',
    styleUrls: ['./profile.component.scss']
})
export class ProfileComponent extends Store<ProfileComponentState> implements OnInit {

    constructor(
        private activeRoute: ActivatedRoute,
        private router: Router,
        private preloader: PreLoaderStore,
        private accountService: AccountService,
        private userStore: UserStore,
        private ezGymComponentStore: EzGymComponentStore,
        public dialog: MatDialog
    ) {
        super()

        this.store$.subscribe(state => {
            if (state.account)
                this.setState(() => ({
                    ...state,
                    ui: {
                        isOwner: state.account.id == this.userStore.user.activeAccount?.id
                    }
                }))
        })

    }

    ngOnInit() {
        this.loadProfile()
        this.ezGymComponentStore.showTopNavBar(true)
    }

    loadProfile() {
        this.preloader.show()
        this.activeRoute.params
            .pipe(
                switchMap(params => {
                    return this.accountService.loadAccount(params["accountName"])
                }),
                tap(response => {
                    this.setState(state => ({
                        ...state,
                        account: response.account,
                        profile: response.profile,
                    }))

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
                account: this.state?.account,
                profile: this.state?.profile || {}
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