import { ProfileModel } from './profile.model';

export interface AccountModel {
  id: string;
  accountType: AccountTypeEnum;
  profile?: ProfileModel;
  gymId?: string;
  accountName: string;
  isDefault: boolean;
  avatarUrl?: string;
  followingCount?: number;
  followersCount?: number;
}

export interface FollowerModel {
  accountId: string;
}

export enum AccountTypeEnum {
  User = 1,
  Gym = 2,
}

export interface AccountMemberShipModel {
  id: string;
  accountId: string;
  gymAccountId: string;
  gymAccountName: string;
  gymAvatarUrl: string;
  paymentDateTime: Date;
  days: number;
  missingDays: number;
  active: boolean;
}
