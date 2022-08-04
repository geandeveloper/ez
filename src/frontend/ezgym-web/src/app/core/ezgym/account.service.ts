import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { VerifyAccountResponseDto } from './dto/verify-account-response.dto';
import { AccountCreatedEvent, AvatarImageAccountChanged, StartFollowAccountEvent } from './events/account.events';
import { ProfileChangedEvent } from './events/profile.events';
import { ProfileModel } from './models/profile.model';
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
        avatar: Blob
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
            .get<{
                account: AccountModel,
                profile: ProfileModel
            }>(`accounts/${accountName}`)
    }

    searchAccounts(accountName: string) {
        return this.http
            .get<AccountModel[]>(`accounts?accountName=${accountName}`)
    }

    followAccount(command: { userAccountId: string, followAccountId: string }): Observable<StartFollowAccountEvent> {
        return this.http
            .post<StartFollowAccountEvent>(`accounts/${command.followAccountId}/followers`, command)
    }
}