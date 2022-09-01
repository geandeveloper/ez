import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { catchError, finalize, tap } from 'rxjs';
import { UserStore } from 'src/app/core/authentication/user.store';
import { AccountService } from 'src/app/ezgym/core/services/account.service';
import { Store } from 'src/app/core/state/store';
import { PixTypeEnum, WalletModel } from 'src/app/ezgym/core/models/wallet.model';
import { WalletService } from 'src/app/ezgym/core/services/wallet.service';
import { PreLoaderStore } from 'src/app/shared/components/pre-loader/pre-loader.store';
import { PaymentAccountStatusEnum } from 'src/app/ezpayment/core/models/payment-account.model';
import { ModalStore } from 'src/app/shared/components/modal/modal.store';
import { EzGymStore } from 'src/app/ezgym/ezgym.store';


interface GymWalletComponentState {
    ui: {
        keyTypeSelected?: { key: string, value?: PixTypeEnum } | null
        keyTypeOptions: { key: string, value?: PixTypeEnum }[]
    },
    wallet?: WalletModel
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
        private modalStore: ModalStore,
        private ezGymStore: EzGymStore,
        private walletService: WalletService,
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
            this.ezGymStore.state.accountActive?.id!
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

    setupPaymentAccount() {
        if (this.state.wallet?.paymentAccount?.paymentAccountStatus == PaymentAccountStatusEnum.Approved) {

            this.modalStore.success({
                title: "Sua conta ja foi aprovada!",
                description: "Caso queira modificar seus dados voce pode clicar no botao continuar",
                confirmButtonLabel: "Continuar",
                onConfirm: () => {
                    this.redirectToConfigureAccount().subscribe();
                }
            })

        } else {
            this.redirectToConfigureAccount().subscribe();
        }

    }

    redirectToConfigureAccount() {
        this.preLoader.show();
        return this.walletService
            .setupPaymentAccount({
                walletId: this.state.wallet?.id!,
                refreshUrl: `http://localhost:4200/${this.ezGymStore.state.accountActive?.accountName}`,
                returnUrl: `http://localhost:4200/${this.ezGymStore.state.accountActive?.accountName}`
            })
            .pipe(
                tap(paymentAccountEvent => {
                    if (this.state.wallet?.paymentAccount?.paymentAccountStatus == PaymentAccountStatusEnum.Pending &&
                        paymentAccountEvent.paymentAccountStatus == PaymentAccountStatusEnum.Approved) {
                        this.preLoader.close();
                        this.modalStore.success({
                            title: "Sua conta ja foi aprovada!",
                            description: "Caso queira modificar seus dados voce pode clicar no botao continuar",
                            confirmButtonLabel: "Continuar",
                            onConfirm: () => {
                                window.location.href = paymentAccountEvent.onBoardingLink;
                            }
                        })

                        this.setState(state => ({
                            ...state,
                            wallet: {
                                ...state.wallet!,
                                paymentAccount: {
                                    paymentAccountStatus: paymentAccountEvent.paymentAccountStatus,
                                    id: paymentAccountEvent.paymentAccountId,
                                }
                            }
                        }))
                        return;
                    }

                    this.setState(state => ({
                        ...state,
                        wallet: {
                            ...state.wallet!,
                            paymentAccount: {
                                paymentAccountStatus: paymentAccountEvent.paymentAccountStatus,
                                id: paymentAccountEvent.paymentAccountId,
                            }
                        }
                    }))

                    window.location.href = paymentAccountEvent.onBoardingLink;
                }),
                finalize(() => {
                    this.modalStore.close();
                }),
                catchError((error) => {
                    return error;
                })
            )
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
            accountId: this.ezGymStore.state.accountActive?.id!,
            pix: this.pixFormGroup.value
        }).pipe(
            finalize(() => {
                this.preLoader.close()
            })
        ).subscribe()
    }
}