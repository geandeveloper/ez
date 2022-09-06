import { Injectable } from "@angular/core";
import { map, switchMap, tap } from "rxjs";
import { Store } from "src/app/core/state/store";
import { WalletModel } from "../../core/models/wallet.model";
import { AccountService } from "../../core/services/account.service";
import { WalletService } from "../../core/services/wallet.service";


export interface GymManagementState {
    wallet: WalletModel
}

@Injectable()
export class GymManagementStore extends Store<GymManagementState> {

    constructor(
        private readonly accountService: AccountService,
        private readonly walletService: WalletService
    ) {
        super();
    }

    loadWallet(id: string) {
        return this.accountService.getWallet(id)
            .pipe(
                tap(wallet => {
                    this.setState(state => ({ ...state, wallet: wallet }))
                }),
                switchMap(wallet => this.walletService.getStatement(wallet.id)),
                tap(statement => {
                    this.setState(state => ({
                        ...state,
                        wallet: {
                            ...state.wallet,
                            statement: statement
                        }
                    }))
                }),
                map(_ => this.state.wallet)
            )
    }

}