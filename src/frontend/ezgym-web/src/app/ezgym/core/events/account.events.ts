import { AccountModel, AccountTypeEnum } from "../models/accout.model";

export interface AccountCreatedEvent {
    id: string,
    command: {
        userId: string,
        accountName: string,
        accountType: AccountTypeEnum,
        isDefault: boolean
    }
}

export interface AvatarImageAccountChanged {
    accountId: string,
    avatarUrl: string
}

export interface StartFollowAccountEvent {
    accountId: string,
    account: AccountModel
}

export interface AccountFollowedEvent {
    accountId: string
}

export interface AccountUnfollowedEvent {
    accountId: string
}

export interface AccountWalletChangedEvent {
    accountId: string
}