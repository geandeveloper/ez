import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AccountModel, AccountTypeEnum } from 'src/app/ezgym/core/models/accout.model';
import { Store } from 'src/app/core/state/store';
import { GymPlansComponent } from './gym-plans/gym-plans.component';
import { GymWalletComponent } from './gym-wallet/gym-wallet.component';
import { EzGymStore } from '../../ezgym.store';

import { filter, first, switchMap, tap } from 'rxjs';
import { GymManagementStore } from './gym-management.store';
import { WalletModel } from '../../core/models/wallet.model';

interface GymManagementComponentState {
    checkinsToValidate: AccountModel[],
    checkinsValidated: AccountModel[],
    wallet: WalletModel,
    activeAccount: AccountModel
}

@Component({
    selector: 'gym-management',
    templateUrl: 'gym-management.component.html',
    styleUrls: ['./gym-management.component.scss']
})
export class GymManagementComponent extends Store<GymManagementComponentState> implements OnInit {
    constructor(
        private dialog: MatDialog,
        private gymManagementStore: GymManagementStore,
        private ezGymStore: EzGymStore,
    ) {
        super({
            checkinsToValidate: [
                {
                    accountName: "@gean",
                    accountType: AccountTypeEnum.User,
                    id: "123",
                    isDefault: true,
                }
            ],
            checkinsValidated: [],
            wallet: {} as WalletModel,
            activeAccount: {} as AccountModel
        })

    }

    ngOnInit() {
        this.ezGymStore.active$
            .pipe(
                filter(activeAccount => {
                    return activeAccount.id != this.state.activeAccount.id
                }),
                tap(activeAccount => {
                    this.setState(state => ({ ...state, activeAccount: activeAccount }))
                }),
                switchMap(activeAccount => this.gymManagementStore.loadWallet(activeAccount.id)),
                tap(wallet => {
                    this.setState(state => ({ ...state, wallet: wallet }))
                })
            ).subscribe()

        this.store$.subscribe()
    }

    openPlans() {
        this.dialog.open(GymPlansComponent, {
            disableClose: true,
            data: {
            },
            panelClass: 'fullscreen-dialog',
            maxWidth: '935px',
            maxHeight: '100vh',
            height: '100%',
            width: '100%',
        })
            .afterClosed()
            .subscribe()

    }

    openWallet() {
        this.dialog.open(GymWalletComponent, {
            disableClose: true,
            data: {
            },
            panelClass: 'fullscreen-dialog',
            maxWidth: '935px',
            maxHeight: '100vh',
            height: '100%',
            width: '100%',
        })
            .afterClosed()
            .subscribe()
    }

}