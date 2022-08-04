import { ModalStore } from 'src/app/shared/components/modal/modal.store';
import { PreLoaderStore } from 'src/app/shared/components/pre-loader/pre-loader.store';
import { AddressModel } from '../../../core/ezgym/models/address.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { finalize, tap, filter } from 'rxjs/operators';
import { Component } from '@angular/core';
import { debounceTime, switchMap } from 'rxjs';
import { Store } from 'src/app/core/state/store';
import { AccountService } from 'src/app/core/ezgym/account.service';
import { UserStore } from 'src/app/core/authentication/user.store';
import { Router } from '@angular/router';
import { AccountTypeEnum } from 'src/app/core/ezgym/models/accout.model';

interface CreateAccountComponentState {
    fantasyName: string,
    accountName: string,
    cnpj: string,
    address: AddressModel
}


@Component({
    selector: 'create-account',
    templateUrl: 'create-account.component.html'
})
export class CreateAccountComponent extends Store<CreateAccountComponentState> {

    createAccountFromGroup: FormGroup
    ui = {
        gymAccountNameStatusClassName: 'bg-primary'
    }

    constructor(
        private userStore: UserStore,
        private accountService: AccountService,
        private fb: FormBuilder,
        private preloader: PreLoaderStore,
        private modal: ModalStore,
        private router: Router,
    ) {
        super()

        this.createAccountFromGroup = fb.group({
            fantasyName: '',
            accountName: this.fb.control('', [Validators.pattern(`^[a-z0-9_-]{2,15}$`), Validators.required]),
            accountType: [AccountTypeEnum.Gym]

        });


        this.createAccountFromGroup.get('accountName')
            ?.valueChanges
            .pipe(
                filter(value => {
                    const gymId = value?.trim()
                    if (gymId.length)
                        return true;

                    this.ui = {
                        ...this.ui,
                        gymAccountNameStatusClassName: gymId == '' ? 'bg-primary' : 'bg-danger'
                    }

                    return false;
                }),
                debounceTime(500),
                switchMap(gymId => {
                    return this
                        .accountService
                        .verifyAccount(gymId)
                        .pipe(
                            tap(response => {
                                this.ui = {
                                    ...this.ui,
                                    gymAccountNameStatusClassName: response.exists ? 'bg-danger' : 'bg-success'
                                }
                            })
                        )
                }))
            .subscribe()
    }


    createAccount() {
        this.preloader.show()

        this.accountService
            .createAccount({
                ...this.createAccountFromGroup.value
            })
            .pipe(
                tap(response => {
                    this.userStore.setState(user => ({
                        ...user,
                        userInfo: {
                            ...user.userInfo!,
                            accounts: [
                                ...user?.userInfo?.accounts!,
                                {
                                    id: response.id,
                                    accountName: response.accountName,
                                    accountType: response.accountType,
                                    isDefault: false
                                }]
                        }
                    }))
                    this.userStore.setActiveAccount(response.accountName)
                    this.router.navigate(['/', response.accountName])
                    this.modal.success({
                        title: `Conta ${response.accountName} criada com sucesso`,
                        description: 'Agora você pode gerenciar sua academia de maneira simples :)'
                    })

                }),
                finalize(() => {
                    this.preloader.close()
                })
            ).subscribe()

    }
}