import { Injectable } from "@angular/core";
import { filter, map, tap } from "rxjs";
import { Store } from "src/app/core/state/store";
import { AccountModel } from "./core/models/accout.model";
import { AccountService } from "./core/services/account.service";
import { EzGymState } from "./ezgym.state";

@Injectable()
export class EzGymStore extends Store<EzGymState> {

    active$ = this.store$.pipe(
        filter(a => Boolean(a?.accountActive)),
        map(a => a.accountActive)
    )

    constructor(
        private readonly accountService: AccountService
    ) {
        super();
    }


    showTopNavBar(show: boolean) {

        this.setState(state => ({
            ...state,
            ui: {
                showTopNavBar: show
            }
        }))
    }

    loadAccounts() {
        return this
            .accountService.myAccounts().pipe(
                tap(accounts => {
                    this.setState(state => ({
                        ...state,
                        accounts,
                        accountActive: state?.accountActive || accounts.find(a => a.isDefault)!
                    }))
                })
            )

    }

    addAccount(account: AccountModel) {
        this.setState(state => ({
            ...state,
            accounts: [
                ...state.accounts,
                account
            ]
        }))
    }

    setActiveAccount(accountName: string) {
        this.setState(state => ({
            ...state,
            accountActive: state.accounts.find(a => a.accountName == accountName)!,
        }))
    }

    updateAccountActive(updateFn: (state: AccountModel) => AccountModel) {
        this.setState(state => ({
            ...state,
            accountActive: updateFn(state.accountActive)
        }))
    }
}