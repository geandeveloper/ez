import { AccountModel } from "../../ezgym/core/models/accout.model"

export interface UserInfoState {
  accounts: AccountModel[]
}

export interface UserState {
  authenticated: boolean,
  id: string,
  accessToken: string,
  activeAccount?: AccountModel
  userInfo?: UserInfoState
}