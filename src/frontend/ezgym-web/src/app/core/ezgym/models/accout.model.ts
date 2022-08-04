export interface AccountModel {
    id: string,
    accountName: string,
    isDefault: false,
    accountType: AccountTypeEnum,
    avatarUrl?: string
}


export enum AccountTypeEnum {
    User = 1,
    Gym = 2
}