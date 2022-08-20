import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AccountModel, AccountTypeEnum } from 'src/app/core/ezgym/models/accout.model';
import { Store } from 'src/app/core/state/store';
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
        public dialog: MatDialog
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