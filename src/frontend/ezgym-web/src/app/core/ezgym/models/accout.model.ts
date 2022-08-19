import { ProfileModel } from "./profile.model"

export interface AccountModel {
    id: string,
    accountType: AccountTypeEnum,
    profile?: ProfileModel,
    gymId?: string,
    accountName: string,
    isDefault: boolean,
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