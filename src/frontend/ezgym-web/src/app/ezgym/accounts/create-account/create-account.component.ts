import { ModalStore } from 'src/app/shared/components/modal/modal.store';
import { PreLoaderStore } from 'src/app/shared/components/pre-loader/pre-loader.store';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { finalize, tap, filter } from 'rxjs/operators';
import { Component } from '@angular/core';
import { debounceTime, switchMap } from 'rxjs';
import { Store } from 'src/app/core/state/store';
import { AccountService } from 'src/app/ezgym/core/services/account.service';
import { UserStore } from 'src/app/core/authentication/user.store';
import { Router } from '@angular/router';
import { AccountTypeEnum } from 'src/app/ezgym/core/models/accout.model';
import { AddressModel } from '../../core/models/address.model';

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
                                    accountName: response.command.accountName,
                                    accountType: response.command.accountType,
                                    isDefault: false
                                }]
                        }
                    }))
                    this.userStore.setActiveAccount(response.command.accountName)
                    this.router.navigate(['/', response.command.accountName])
                    this.modal.success({
                        title: `Conta ${response.command.accountName} criada com sucesso`,
                        description: 'Agora você pode gerenciar sua academia de maneira simples :)'
                    })

                }),
                finalize(() => {
                    this.preloader.close()
                })
            ).subscribe()

    }
}