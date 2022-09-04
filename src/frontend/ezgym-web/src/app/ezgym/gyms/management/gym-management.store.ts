import { Injectable } from "@angular/core";
import { tap } from "rxjs";
import { Store } from "src/app/core/state/store";
import { WalletModel } from "../../core/models/wallet.model";
import { AccountService } from "../../core/services/account.service";


export interface GymManagementState {
    wallet: WalletModel
}

@Injectable()
export class GymManagementStore extends Store<GymManagementState> {

    constructor(
        private readonly accountService: AccountService
    ) {
        super();
    }

    loadWallet(id: string) {
        return this.accountService.getWallet(id)
            .pipe(
                tap(wallet => {
                    this.setState(state => ({ ...state, wallet: wallet }))
                })
            )
    }

}