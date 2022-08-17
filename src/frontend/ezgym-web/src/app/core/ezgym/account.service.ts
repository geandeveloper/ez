import { Observable, of } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { VerifyAccountResponseDto } from './dto/verify-account-response.dto';
import { AccountCreatedEvent, AccountFollowedEvent, AccountUnfollowedEvent, AvatarImageAccountChanged, StartFollowAccountEvent } from './events/account.events';
import { ProfileChangedEvent } from './events/profile.events';
import { AccountModel } from './models/accout.model';

@Injectable({
    providedIn: 'root'
})
export class AccountService {
    constructor(private http: HttpClient) { }

    verifyAccount(accountName: string): Observable<VerifyAccountResponseDto> {
        return this.http
            .get<VerifyAccountResponseDto>(`accounts/${accountName}/verify`)
    }

    createAccount(command: any): Observable<AccountCreatedEvent> {
        return this.http
            .post<AccountCreatedEvent>("accounts", command)
    }

    changeAvatar(command: {
        accountId: string,
        avatar: Blob,
    }): Observable<AvatarImageAccountChanged> {

        const formMultiPart = new FormData();
        formMultiPart.append('avatar', command.avatar);

        return this.http
            .post<AvatarImageAccountChanged>(`accounts/${command.accountId}/avatar`, formMultiPart)
    }

    upInsertProfile(command: any): Observable<ProfileChangedEvent> {
        return this.http
            .put<ProfileChangedEvent>(`accounts/${command.accountId}/profile`, command)
    }

    loadAccount(accountName: string) {
        return this.http
            .get<AccountModel>(`accounts/${accountName}`)
    }

    searchAccounts(query: string) {
        return this.http
            .get<AccountModel[]>(`accounts?query=${query}`)
    }

    searchFollowers(accountName: string, query: string) {
        return this.http
            .get<AccountModel[]>(`accounts/${accountName}/followers?query=${query}`)
    }

    searchFollowing(accountName: string, query: string) {
        return this.http
            .get<AccountModel[]>(`accounts/${accountName}/following?query=${query}`)
    }

    followAccount(command: { userAccountId: string, followAccountId: string }): Observable<AccountFollowedEvent> {
        return this.http
            .post<AccountFollowedEvent>(`accounts/${command.followAccountId}/follow`, command)
    }

    unfollowAccount(command: { userAccountId: string, unfollowAccountId: string }): Observable<AccountUnfollowedEvent> {
        return this.http
            .post<AccountUnfollowedEvent>(`accounts/${command.unfollowAccountId}/unfollow`, command)
    }
}