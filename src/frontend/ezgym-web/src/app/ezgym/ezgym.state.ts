import { AccountModel } from "./core/models/accout.model";
import { GymModel } from "./core/models/gym.model";

export interface EzGymState {
    ui?: {
        showTopNavBar: boolean
    },
    accounts: AccountModel[],
    accountActive: AccountModel,
    activeGym?: GymModel
}