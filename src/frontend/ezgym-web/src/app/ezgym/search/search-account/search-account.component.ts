import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { debounceTime, tap } from 'rxjs';
import { AccountService } from 'src/app/ezgym/core/services/account.service';
import { AccountModel } from 'src/app/ezgym/core/models/accout.model';
import { Store } from 'src/app/core/state/store';
import { EzGymComponentStore } from '../../ezgym.component.store';

interface SearchAccountState {
    accounts: AccountModel[]
}

@Component({
    selector: 'search-account',
    templateUrl: './search-account.component.html',
    styleUrls: ['./search-account.component.scss']
})
export class SearchAccountComponent extends Store<SearchAccountState> {

    searchForm: FormGroup


    constructor(
        private ezGymComponentStorage: EzGymComponentStore,
        private accountService: AccountService,
        private fb: FormBuilder
    ) {
        super({ accounts: [] })

        this.ezGymComponentStorage.showTopNavBar(false)

        this.searchForm = this.fb.group({
            accountName: ['']
        })

        this.searchForm
            .get("accountName")
            ?.valueChanges
            .pipe(
                debounceTime(500)
            )
            .subscribe(accountNameValue => {
                if(accountNameValue == '') {
                    return this.setState(() => ({
                        accounts: []
                    }))
                }

                this.accountService.searchAccounts(accountNameValue)
                    .pipe(
                        tap(accounts => {
                            this.setState(state => ({ ...state, accounts: accounts }))
                        })
                    ).subscribe()
            })
    }

}