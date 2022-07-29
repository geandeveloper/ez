import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { VerifyAccountResponseDto } from './dto/verify-account-response.dto';

@Injectable({
    providedIn: 'root'
})
export class AccountService {
    constructor(private http: HttpClient) { }

    verifyAccount(accountName: string): Observable<VerifyAccountResponseDto> {
        return this.http
            .get<VerifyAccountResponseDto>(`accounts/${accountName}/verify`)
    }
}