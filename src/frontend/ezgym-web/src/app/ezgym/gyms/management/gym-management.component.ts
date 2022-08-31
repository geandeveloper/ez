import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AccountModel, AccountTypeEnum } from 'src/app/ezgym/core/models/accout.model';
import { Store } from 'src/app/core/state/store';
import { EzGymComponentStore } from '../../ezgym.component.store';
import { GymPlansComponent } from './gym-plans/gym-plans.component';
import { GymWalletComponent } from './gym-wallet/gym-wallet.component';



interface GymManagementComponentState {
    checkinsToValidate: AccountModel[],
    checkinsValidated: AccountModel[]
}

@Component({
    selector: 'gym-management',
    templateUrl: 'gym-management.component.html',
    styleUrls: ['./gym-management.component.scss']
})
export class GymManagementComponent extends Store<GymManagementComponentState> implements OnInit {
    constructor(
        private dialog: MatDialog,
        private ezGymComponentStorage: EzGymComponentStore,
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
            checkinsValidated: []
        })

        this.store$.subscribe()
    }

    ngOnInit() {
        this.openWallet()
        setTimeout(() => {
            this.ezGymComponentStorage.showTopNavBar(true);
        });
    }

    openPlans() {
        this.dialog.open(GymPlansComponent, {
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