import { AccountModel } from "./core/models/accout.model";

export interface EzGymState {
    ui?: {
        showTopNavBar: boolean
    },
    accounts: AccountModel[],
    accountActive: AccountModel
}