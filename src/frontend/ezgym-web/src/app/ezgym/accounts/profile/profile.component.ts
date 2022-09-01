import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { catchError, finalize, switchMap, tap } from 'rxjs';
import { AccountService } from 'src/app/ezgym/core/services/account.service';
import { AccountModel } from 'src/app/ezgym/core/models/accout.model';
import { Store } from 'src/app/core/state/store';
import { PreLoaderStore } from 'src/app/shared/components/pre-loader/pre-loader.store';
import { FollowerListComponent } from '../follower-list/follower-list.component';
import { EditProfileComponent } from './edit-profile/edit-profile.component';
import { EzGymStore } from '../../ezgym.store';

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
        private ezGymStore: EzGymStore,
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
            this.ezGymStore.showTopNavBar(true)
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
                    this.setState(state => ({
                        ...state,
                        account: response,
                        ui: {
                            isOwner: response.id == this.ezGymStore.state.accountActive.id,
                            isFollowing: this.ezGymStore.state.accountActive?.following?.some(f => f.accountId === response.id)!
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
                setTimeout(() => {
                    this.loadProfile()
                })
            })
    }

    followAccount(accountId: string) {
        this.preloader.show();
        this.accountService.followAccount({
            userAccountId: this.ezGymStore.state.accountActive?.id!,
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
            }),
            finalize(() => {
                this.preloader.close()
            })
        ).subscribe()
    }

    unfollowAccount(accountId: string) {
        this.preloader.show();
        this.accountService.unfollowAccount({
            userAccountId: this.ezGymStore.state.accountActive.id!,
            unfollowAccountId: accountId
        }).pipe(
            tap(() => {
                this.setState(state => ({
                    ...state,
                    account: {
                        ...state.account,
                        followersCount: state.account.followersCount! - 1
                    },
                    ui: {
                        ...state.ui,
                        isFollowing: false
                    }
                }))
            }),
            finalize(() => {
                this.preloader.close()
            })
        ).subscribe()
    }

    openFollowerList() {
        this.dialog.open(FollowerListComponent, {
            data: {
                account: this.state?.account,
                ui: {
                    activeTab: 'followers',
                }
            },
            panelClass: 'fullscreen-dialog',
            maxWidth: '935px',
            maxHeight: '100vh',
            height: '100%',
            width: '100%',
        }).afterClosed()
            .subscribe(() => {
            })

    }

    openFollowingList() {
        this.dialog.open(FollowerListComponent, {
            data: {
                account: this.state?.account,
                ui: {
                    activeTab: 'following',
                }
            },
            panelClass: 'fullscreen-dialog',
            maxWidth: '935px',
            maxHeight: '100vh',
            height: '100%',
            width: '100%',
        }).afterClosed()
            .subscribe(() => {
            })

    }
}