export enum PixTypeEnum {
    PhoneNumber = 1,
    Email = 2,
    Random = 3
}

export interface Pix {
    type: PixTypeEnum
    value: string
}

export interface Wallet {
    balance: number
    pix: Pix
}