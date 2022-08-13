import { Component, OnInit } from '@angular/core';
import { AccountModel, AccountTypeEnum } from 'src/app/core/ezgym/models/accout.model';
import { Store } from 'src/app/core/state/store';



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
    constructor() {
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

    ngOnInit() { }
}