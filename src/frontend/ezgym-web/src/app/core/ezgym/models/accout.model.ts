export interface AccountModel {
    id: string,
    accountName: string,
    active: false,
    accountType: AccounTypeEnum
}


export enum AccounTypeEnum {
    User = 1,
    Gym = 2
}