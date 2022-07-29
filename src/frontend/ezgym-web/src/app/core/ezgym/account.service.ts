import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { VerifyAccountResponseDto } from './dto/verify-account-response.dto';
import { AccountCreatedEvent } from './events/account.events';

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
}