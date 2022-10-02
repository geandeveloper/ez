import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { catchError, finalize, forkJoin, switchMap, tap } from 'rxjs';
import { AccountService } from 'src/app/ezgym/core/services/account.service';
import { Store } from 'src/app/core/state/store';
import { FollowerListComponent } from '../follower-list/follower-list.component';
import { EditProfileComponent } from './edit-profile/edit-profile.component';
import { EzGymStore } from '../../ezgym.store';
import { AccountProfileProjection } from '../../core/projections/account-profile.projection';

interface ProfileComponentState {
  accountProfile: AccountProfileProjection;
  friendship: {
    totalFollowers: number;
    totalFollowing: number;
  };
  ui: {
    isOwner: boolean;
    isFollowing: boolean;
    loading?: boolean;
  };
}

@Component({
  selector: 'profile',
  templateUrl: 'profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent
  extends Store<ProfileComponentState>
  implements OnInit
{
  constructor(
    private activeRoute: ActivatedRoute,
    private router: Router,
    private accountService: AccountService,
    private ezGymStore: EzGymStore,
    public dialog: MatDialog
  ) {
    super({
      accountProfile: {} as AccountProfileProjection,
      friendship: {
        totalFollowers: 0,
        totalFollowing: 0,
      },
      ui: {
        isOwner: false,
        isFollowing: false,
      },
    });
  }

  ngOnInit() {
    setTimeout(() => {
      this.ezGymStore.showTopNavBar(true);
    });

    this.ezGymStore.active$.subscribe((activeAccount) => {
      if (activeAccount.id == this.state.accountProfile.id)
        this.setState((state) => ({
          ...state,
          accountProfile: {
            ...activeAccount,
          },
        }));
    });

    this.activeRoute.params.subscribe((params) => {
      this.setLoading(true);
      this.loadAccount(params['accountName'])
        .pipe(
          finalize(() => {
            this.setLoading(false);
          })
        )
        .subscribe();
    });
  }

  loadFriendShip(id: string) {
    return forkJoin([
      this.accountService.loadTotalFollowers(id),
      this.accountService.loadTotalFollowing(id),
    ]).pipe(
      tap(([followers, following]) => {
        this.setState((state) => ({
          ...state,
          friendship: {
            totalFollowers: followers.total,
            totalFollowing: following.total,
          },
        }));
      })
    );
  }

  loadAccount(accountName: string) {
    return this.accountService.loadAccountProfile(accountName).pipe(
      tap((accountProfile) => {
        this.setState((state) => ({
          ...state,
          accountProfile: accountProfile,
        }));
      }),
      switchMap((account) => {
        return this.loadFriendShip(account.id);
      }),
      switchMap((_) => {
        return this.accountService
          .isFollowing(
            this.state.accountProfile.id,
            this.ezGymStore.state.accountActive.id
          )
          .pipe(
            tap((response) => {
              this.setState((state) => ({
                ...state,
                ui: {
                  ...state.ui,
                  isFollowing: response.isFollowing,
                  isOwner:
                    this.ezGymStore.state.accountActive.id ==
                    state.accountProfile.id,
                },
              }));
            })
          );
      }),
      catchError((error) => {
        this.router.navigate(['/404']);
        return error;
      })
    );
  }

  editProfile() {
    this.dialog
      .open(EditProfileComponent, {
        disableClose: true,
        data: {
          accountProfile: this.state?.accountProfile,
        },
        panelClass: 'fullscreen-dialog',
        maxWidth: '935px',
        maxHeight: '100vh',
        height: '100%',
        width: '100%',
      })
      .afterClosed()
      .subscribe();
  }

  followAccount(accountId: string) {
    this.accountService
      .followAccount({
        userAccountId: this.ezGymStore.state.accountActive?.id!,
        followAccountId: accountId,
      })
      .pipe(
        tap((_response) => {
          this.setState((state) => ({
            ...state,
            accountProfile: {
              ...state.accountProfile,
            },
            ui: {
              ...state.ui,
              isFollowing: true,
            },
            friendship: {
              ...state.friendship,
              totalFollowers: state.friendship.totalFollowers + 1,
            },
          }));
        }),
        finalize(() => {})
      )
      .subscribe();
  }

  unfollowAccount(accountId: string) {
    this.accountService
      .unfollowAccount({
        userAccountId: this.ezGymStore.state.accountActive.id!,
        unfollowAccountId: accountId,
      })
      .pipe(
        tap(() => {
          this.setState((state) => ({
            ...state,
            accountProfile: {
              ...state.accountProfile,
            },
            friendship: {
              ...state.friendship,
              totalFollowers: state.friendship.totalFollowers - 1,
            },
            ui: {
              ...state.ui,
              isFollowing: false,
            },
          }));
        }),
        finalize(() => {})
      )
      .subscribe();
  }

  openFollowerList() {
    this.dialog
      .open(FollowerListComponent, {
        disableClose: true,
        data: {
          accountProfile: this.state?.accountProfile,
          ui: {
            activeTab: 'followers',
          },
        },
        panelClass: 'fullscreen-dialog',
        maxWidth: '935px',
        maxHeight: '100vh',
        height: '100%',
        width: '100%',
      })
      .afterClosed()
      .subscribe(() => {});
  }

  openFollowingList() {
    this.dialog
      .open(FollowerListComponent, {
        disableClose: true,
        data: {
          accountProfile: this.state?.accountProfile,
          ui: {
            activeTab: 'following',
          },
        },
        panelClass: 'fullscreen-dialog',
        maxWidth: '935px',
        maxHeight: '100vh',
        height: '100%',
        width: '100%',
      })
      .afterClosed()
      .subscribe(() => {});
  }

  setLoading(loading: boolean) {
    this.setState((state) => ({
      ...state,
      ui: {
        ...state.ui,
        loading: loading,
      },
    }));
  }
}

function of(response: { isFollowing: boolean }): any {
  throw new Error('Function not implemented.');
}
