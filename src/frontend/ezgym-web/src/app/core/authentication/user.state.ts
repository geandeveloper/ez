import { AccountModel } from "../ezgym/models/accout.model"
import { GymModel } from "../ezgym/models/gym.model"

export interface UserInfoState {
  accounts: AccountModel[],
  gym?: GymModel
}

export interface UserState {
  authenticated: boolean,
  id: string,
  accessToken: string,
  activeAccount?: AccountModel
  userInfo?: UserInfoState
}