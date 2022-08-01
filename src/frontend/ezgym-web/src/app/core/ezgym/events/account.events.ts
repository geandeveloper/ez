import { AccounTypeEnum } from "../models/accout.model";

export interface AccountCreatedEvent {
    id: string,
    userId: string,
    accountName: string,
    accountType: AccounTypeEnum,
    isDefault: boolean
}

export interface AvatarImageAccountChanged {
    accountId: string,
    avatarUrl: string
}