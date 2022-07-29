import { AccountModel } from "../ezgym/models/accout.model"

export interface UserInfoState {
  userName: string,
  accounts: AccountModel[]
}

export interface UserState {
  authenticated: boolean,
  id: string,
  accessToken: string,
  userInfo?: UserInfoState
}