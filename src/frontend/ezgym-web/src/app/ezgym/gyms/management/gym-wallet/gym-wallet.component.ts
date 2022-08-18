import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { finalize, tap } from 'rxjs';
import { UserStore } from 'src/app/core/authentication/user.store';
import { AccountService } from 'src/app/core/ezgym/account.service';
import { PixTypeEnum, Wallet } from 'src/app/core/ezgym/models/wallet.model';
import { Store } from 'src/app/core/state/store';
import { PreLoaderStore } from 'src/app/shared/components/pre-loader/pre-loader.store';


interface GymWalletComponentState {
    ui: {
        keyTypeSelected?: { key: string, value?: PixTypeEnum } | null
        keyTypeOptions: { key: string, value?: PixTypeEnum }[]
    },
    wallet?: Wallet
}


@Component({
    selector: 'gym-wallet',
    templateUrl: 'gym-wallet.component.html',
    styleUrls: ['./gym-wallet.component.scss']
})

export class GymWalletComponent extends Store<GymWalletComponentState> implements OnInit {

    pixFormGroup: FormGroup;

    constructor(
        private modal: MatDialogRef<GymWalletComponent>,
        private fb: FormBuilder,
        private accountService: AccountService,
        private preLoader: PreLoaderStore,
        private userStore: UserStore
    ) {
        super({
            ui: {
                keyTypeSelected: null,
                keyTypeOptions: [
                    { key: "E-mail", value: PixTypeEnum.Email },
                    { key: "Celular", value: PixTypeEnum.PhoneNumber },
                    { key: "Aleatorio", value: PixTypeEnum.Random }
                ]
            }
        })


        this.pixFormGroup = this.fb.group({
            type: [''],
            value: ['']
        })


        this.store$.subscribe(state => {

            if (state.wallet?.pix != null) {
                this.pixFormGroup.patchValue({
                    type: state.wallet.pix.type,
                    value: state.wallet.pix.value
                })
            }
        })
    }

    ngOnInit() {
        setTimeout(() => {
            this.preLoader.show();
        })

        this.accountService.getWallet(
            this.userStore.state.activeAccount?.id!
        ).pipe(
            tap(wallet => {
                this.setState(state => ({
                    ...state,
                    ui: {
                        ...state.ui,
                        keyTypeSelected: state.ui.keyTypeOptions.find(k => k.value == wallet.pix.type)
                    },
                    wallet: wallet
                }))
            }),
            finalize(() => {
                this.preLoader.close();
            })
        ).subscribe()


    }

    keyTypeChanged(pixType: PixTypeEnum) {
        this.setState(state => ({
            ...state,
            ui: {
                ...state.ui,
                keyTypeSelected: state.ui.keyTypeOptions.find(k => k.value == pixType)
            }
        }))

        this.pixFormGroup.patchValue({
            type: this.state.ui.keyTypeSelected?.value
        })
    }

    close() {
        this.modal.close();
    }

    saveChanges() {
        this.preLoader.show();

        this.accountService.updateWallet({
            accountId: this.userStore.state.activeAccount?.id!,
            pix: this.pixFormGroup.value
        }).pipe(
            finalize(() => {
                this.preLoader.close()
            })
        ).subscribe()
    }
}