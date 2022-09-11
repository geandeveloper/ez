import { AccountTypeEnum } from "../models/accout.model";
import { ProfileModel } from "../models/profile.model";

export interface AccountProfileProjection {
    id: string,
    accountType: AccountTypeEnum,
    profile?: ProfileModel,
    accountName: string,
    avatarUrl?: string
}