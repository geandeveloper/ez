import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { catchError, switchMap, tap } from 'rxjs';
import { UserStore } from 'src/app/core/authentication/user.store';
import { AccountService } from 'src/app/core/ezgym/account.service';
import { AccountModel } from 'src/app/core/ezgym/models/accout.model';
import { Store } from 'src/app/core/state/store';
import { PreLoaderStore } from 'src/app/shared/components/pre-loader/pre-loader.store';
import { EzGymComponentStore } from '../ezgym.component.store';
import { EditProfileComponent } from './edit-profile/edit-profile.component';

interface ProfileComponentState {
    account: AccountModel,
    ui: {
        isOwner: boolean,
        isFollowing: boolean
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
        super({
            account: {} as AccountModel,
            ui: {
                isOwner: false,
                isFollowing: false,
            }
        })

    }

    ngOnInit() {
        setTimeout(() => {
            this.ezGymComponentStore.showTopNavBar(true)
        })
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
                    debugger
                    this.setState(state => ({
                        ...state,
                        account: response,
                        ui: {
                            isOwner: response.id == this.userStore.user.activeAccount?.id,
                            isFollowing: this.userStore.user.activeAccount?.following?.some(f => f.accountId === response.id)!
                        }
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
                profile: this.state?.account.profile || {}
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

    followAccount(accountId: string) {
        this.accountService.followAccount({
            userAccountId: this.userStore.state.activeAccount?.id!,
            followAccountId: accountId
        }).pipe(
            tap((response) => {
                this.setState(state => ({
                    ...state,
                    account: {
                        ...state.account,
                        followersCount: state.account.followersCount! + 1
                    },
                    ui: {
                        ...state.ui,
                        isFollowing: true
                    }
                }))

                this.userStore.setState(state => ({
                    ...state,
                    activeAccount: {
                        ...state.activeAccount!,
                        following: [...state.activeAccount?.followers!, { accountId: response.accountId }],
                        followingCount: state.activeAccount?.followersCount! + 1
                    }
                }))
            })
        ).subscribe()
    }
}