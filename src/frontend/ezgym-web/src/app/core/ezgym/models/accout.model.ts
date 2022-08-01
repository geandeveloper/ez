export interface AccountModel {
    id: string,
    accountName: string,
    isDefault: false,
    accountType: AccounTypeEnum,
    avatarUrl?: string
}


export enum AccounTypeEnum {
    User = 1,
    Gym = 2
}