import { AccountModel, AccountTypeEnum } from "../models/accout.model";

export interface AccountCreatedEvent {
    id: string,
    userId: string,
    accountName: string,
    accountType: AccountTypeEnum,
    isDefault: boolean
}

export interface AvatarImageAccountChanged {
    accountId: string,
    avatarUrl: string
}

export interface StartFollowAccountEvent {
    accountId: string,
    account: AccountModel
}