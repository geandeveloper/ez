import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { catchError, debounce, debounceTime, filter, finalize, switchMap, tap } from 'rxjs';
import { AccountService } from 'src/app/ezgym/core/services/account.service';
import { AccountModel } from 'src/app/ezgym/core/models/accout.model';
import { Store } from 'src/app/core/state/store';
import { PreLoaderStore } from 'src/app/shared/components/pre-loader/pre-loader.store';
import { FollowerListComponent } from '../follower-list/follower-list.component';
import { EditProfileComponent } from './edit-profile/edit-profile.component';
import { EzGymStore } from '../../ezgym.store';
import { AccountProfileProjection } from '../../core/projections/account-profile.projection';

interface ProfileComponentState {
    accountProfile: AccountProfileProjection,
    ui: {
        isOwner: boolean,
        isFollowing: boolean,
        loading?: boolean
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
        private accountService: AccountService,
        private ezGymStore: EzGymStore,
        public dialog: MatDialog
    ) {
        super({
            accountProfile: {} as AccountProfileProjection,
            ui: {
                isOwner: false,
                isFollowing: false
            }
        })
    }

    ngOnInit() {
        setTimeout(() => {
            this.ezGymStore.showTopNavBar(true)
        })

        this.ezGymStore.active$.subscribe(activeAccount => {
            if (activeAccount.id == this.state.accountProfile.id)
                this.setState(state => ({
                    ...state,
                    accountProfile: {
                        ...activeAccount
                    }
                }))
        })

        this.activeRoute.params
            .pipe(
                switchMap(params => {
                    return this.loadAccount(params['accountName'])
                }),
            ).subscribe()
    }

    loadAccount(accountName: string) {
        this.setLoading(true)
        return this.accountService
            .loadAccountProfile(accountName)
            .pipe(
                tap(accountProfile => {
                    this.setState(state => ({
                        ...state,
                        accountProfile: accountProfile,
                    }))


                }),
                switchMap(() => this.ezGymStore.active$),
                tap(activeAccount => {
                    this.setState(state => ({
                        ...state,
                        ui: {
                            isOwner: activeAccount.id == state.accountProfile.id,
                            isFollowing: activeAccount?.following?.some(f => f.accountId === activeAccount.id)!
                        }
                    }))
                }),
                catchError((error) => {
                    this.router.navigate(['/404'])
                    return error
                }),
                finalize(() => {
                    this.setLoading(false)
                })
            )
    }

    editProfile() {
        this.dialog.open(EditProfileComponent, {
            disableClose: true,
            data: {
                accountProfile: this.state?.accountProfile,
            },
            panelClass: 'fullscreen-dialog',
            maxWidth: '935px',
            maxHeight: '100vh',
            height: '100%',
            width: '100%',
        }).afterClosed()
            .subscribe()
    }

    followAccount(accountId: string) {
        this.accountService.followAccount({
            userAccountId: this.ezGymStore.state.accountActive?.id!,
            followAccountId: accountId
        }).pipe(
            tap((_response) => {
                this.setState(state => ({
                    ...state,
                    accountProfile: {
                        ...state.accountProfile,
                        followersCount: state.accountProfile.followersCount! + 1
                    },
                    ui: {
                        ...state.ui,
                        isFollowing: true
                    }
                }))
            }),
            finalize(() => {
            })
        ).subscribe()
    }

    unfollowAccount(accountId: string) {
        this.accountService.unfollowAccount({
            userAccountId: this.ezGymStore.state.accountActive.id!,
            unfollowAccountId: accountId
        }).pipe(
            tap(() => {
                this.setState(state => ({
                    ...state,
                    accountProfile: {
                        ...state.accountProfile,
                        followersCount: state.accountProfile.followersCount! - 1
                    },
                    ui: {
                        ...state.ui,
                        isFollowing: false
                    }
                }))
            }),
            finalize(() => {
            })
        ).subscribe()
    }

    openFollowerList() {
        this.dialog.open(FollowerListComponent, {
            disableClose: true,
            data: {
                accountProfile: this.state?.accountProfile,
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
            disableClose: true,
            data: {
                accountProfile: this.state?.accountProfile,
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

    setLoading(loading: boolean) {
        this.setState(state => ({
            ...state,
            ui: {
                ...state.ui,
                loading: loading
            }
        }))
    }
}