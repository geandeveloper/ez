import { Observable, of } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { VerifyAccountResponseDto } from '../dto/verify-account-response.dto';
import {
  AccountCreatedEvent,
  AccountFollowedEvent,
  AccountUnfollowedEvent,
  AccountWalletChangedEvent,
  AvatarImageAccountChanged,
  StartFollowAccountEvent,
} from '../events/account.events';
import { AccountModel } from '../models/accout.model';
import { ProfileChangedEvent } from '../events/profile.events';
import { GymModel } from '../models/gym.model';
import { Pix, WalletModel } from '../models/wallet.model';
import { AccountProfileProjection } from '../projections/account-profile.projection';
import { PlayerModel } from '../models/player.model';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  constructor(private http: HttpClient) {}

  verifyAccount(accountName: string): Observable<VerifyAccountResponseDto> {
    return this.http.get<VerifyAccountResponseDto>(
      `accounts/${accountName}/verify`
    );
  }

  createAccount(command: any): Observable<AccountCreatedEvent> {
    return this.http.post<AccountCreatedEvent>('accounts', command);
  }

  changeAvatar(command: {
    accountId: string;
    avatar: Blob;
  }): Observable<AvatarImageAccountChanged> {
    const formMultiPart = new FormData();
    formMultiPart.append('avatar', command.avatar);

    return this.http.post<AvatarImageAccountChanged>(
      `accounts/${command.accountId}/avatar`,
      formMultiPart
    );
  }

  upInsertProfile(command: any): Observable<ProfileChangedEvent> {
    return this.http.put<ProfileChangedEvent>(
      `accounts/${command.accountId}/profile`,
      command
    );
  }

  loadAccountProfile(accountName: string) {
    return this.http.get<AccountProfileProjection>(
      `accounts/${accountName}/profile`
    );
  }

  loadTotalFollowers(id: string) {
    return this.http.get<{ total: number }>(`accounts/${id}/followers/count`);
  }

  isFollowing(id: string, followerAccountId: string) {
    return this.http.get<{ isFollowing: boolean }>(
      `accounts/${id}/followers/${followerAccountId}`
    );
  }

  loadTotalFollowing(id: string) {
    return this.http.get<{ total: number }>(`accounts/${id}/following/count`);
  }

  searchAccounts(query: string) {
    return this.http.get<AccountModel[]>(`accounts?query=${query}`);
  }

  searchFollowers(accountName: string, query: string) {
    return this.http.get<AccountModel[]>(
      `accounts/${accountName}/followers?query=${query}`
    );
  }

  searchFollowing(accountName: string, query: string) {
    return this.http.get<AccountModel[]>(
      `accounts/${accountName}/following?query=${query}`
    );
  }

  followAccount(command: {
    userAccountId: string;
    followAccountId: string;
  }): Observable<AccountFollowedEvent> {
    return this.http.post<AccountFollowedEvent>(
      `accounts/${command.followAccountId}/follow`,
      command
    );
  }

  unfollowAccount(command: {
    userAccountId: string;
    unfollowAccountId: string;
  }): Observable<AccountUnfollowedEvent> {
    return this.http.post<AccountUnfollowedEvent>(
      `accounts/${command.unfollowAccountId}/unfollow`,
      command
    );
  }

  updateWallet(command: {
    accountId: string;
    pix: Pix;
  }): Observable<AccountWalletChangedEvent> {
    return this.http.post<AccountWalletChangedEvent>(
      `accounts/${command.accountId}/wallet`,
      command
    );
  }

  getWallet(accountId: string): Observable<WalletModel> {
    return this.http.get<WalletModel>(`accounts/${accountId}/wallet`);
  }

  getGym(accountId: string): Observable<GymModel> {
    return this.http.get<GymModel>(`accounts/${accountId}/gym`);
  }

  getPlayer(playerId: string) {
    return this.http.get<PlayerModel>(`accounts/${playerId}/player`);
  }

  //refactoring....

  myAccounts() {
    return this.http.get<AccountModel[]>(`my/accounts`);
  }
}
