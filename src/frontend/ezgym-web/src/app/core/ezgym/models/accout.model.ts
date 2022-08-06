import { ProfileModel } from "./profile.model"

export interface AccountModel {
    id: string,
    accountType: AccountTypeEnum,
    profile?: ProfileModel,
    accountName: string,
    isDefault: false,
    avatarUrl?: string,
    followingCount?: number
    followersCount?: number
}


export enum AccountTypeEnum {
    User = 1,
    Gym = 2
}