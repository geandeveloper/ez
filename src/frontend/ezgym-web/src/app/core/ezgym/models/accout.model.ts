import { ProfileModel } from "./profile.model"

export interface AccountModel {
    id: string,
    accountType: AccountTypeEnum,
    profile?: ProfileModel,
    accountName: string,
    isDefault: false,
    avatarUrl?: string,
    followers?: FollowerModel[],
    following?: FollowerModel[],
    followingCount?: number,
    followersCount?: number
}

export interface FollowerModel {
    accountId: string,
}

export enum AccountTypeEnum {
    User = 1,
    Gym = 2
}