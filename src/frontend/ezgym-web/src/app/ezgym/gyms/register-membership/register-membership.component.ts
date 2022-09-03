import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { finalize, switchMap, tap } from 'rxjs';
import { AccountService } from 'src/app/ezgym/core/services/account.service';
import { GymService } from 'src/app/ezgym/core/services/gym.service';
import { Store } from 'src/app/core/state/store';
import { PreLoaderStore } from 'src/app/shared/components/pre-loader/pre-loader.store';
import { GymPlanModel, GymModel, PaymentMethodEnum } from '../../core/models/gym.model';
import { EzGymStore } from '../../ezgym.store';


interface ComponentState {
    ui?: {
        planSelected: GymPlanModel,
        paymentSelected: any
    }
    accountId: string,
    gym: GymModel,
    plans: { value: GymPlanModel, selected: boolean }[]
    paymentMethods: {
        value: PaymentMethodEnum,
        name: string,
        description: string,
        selected: boolean
    }[]
}

@Component({
    selector: 'register-membership',
    templateUrl: './register-membership.component.html',
    styleUrls: ['./register-membership.component.scss']
})

export class RegisterMembershipComponent extends Store<ComponentState> implements OnInit {

    constructor(
        private modal: MatDialogRef<RegisterMembershipComponent>,
        private accountService: AccountService,
        private gymService: GymService,
        private ezGymStore: EzGymStore,
        private preloader: PreLoaderStore,
        private router: Router,
        @Inject(MAT_DIALOG_DATA) public data: { accountId: string }) {

        super({
            accountId: data.accountId,
            gym: {} as GymModel,
            plans: [],
            paymentMethods: [
                {
                    name: "PIX",
                    description: "Pagamento instantaneo",
                    value: PaymentMethodEnum.Pix,
                    selected: false
                },
                {
                    name: "Cartao de Credito",
                    description: "Pagamento com cartao",
                    value: PaymentMethodEnum.CreditCard,
                    selected: false
                }
            ]
        })

        this.accountService
            .getGym(this.data.accountId)
            .pipe(
                switchMap(gym => {
                    this.setState(state => ({
                        ...state,
                        gym: gym
                    }))

                    return this.gymService.loadPlans(gym.id)
                }),
                tap(plans => {
                    this.setState(state => ({
                        ...state,
                        plans: plans.map((p) => ({ value: p, selected: false }))
                    }))
                })
            ).subscribe()
    }

    planSelected(planId: string) {
        this.setState(state => ({
            ...state,
            ui: {
                ...state.ui!,
                planSelected: state.plans.find(p => p.value.id == planId)?.value!
            },
            plans: state.plans.map(p => ({
                ...p,
                selected: p.value.id == planId
            }))
        }))
    }

    paymentSelected(paymentType: PaymentMethodEnum) {
        this.setState(state => ({
            ...state,
            ui: {
                ...state.ui!,
                paymentSelected: state.paymentMethods.find(p => p.value == paymentType)
            },
            paymentMethods: state.paymentMethods.map(p => ({
                ...p,
                selected: p.value == paymentType
            }))
        }))
    }

    confirmRegister() {
        this.preloader.show();
        this.gymService.registerMemberShip({
            payerAccountId: this.ezGymStore.state.accountActive?.id,
            gymId: this.state.gym.id,
            planId: this.state.ui?.planSelected.id,
            paymentMethod: this.state.ui?.paymentSelected.value,
            redirectUrl: `http://192.168.15.136:4200/${this.ezGymStore.state.accountActive?.accountName}`,
        }).pipe(
            tap(paymentEvent => {
                this.router.navigate([`/ezpayment/${paymentEvent.paymentId}/credit-card`])
                this.close();
            }),
            finalize(() => {
                this.preloader.close()
            })
        ).subscribe()
    }

    ngOnInit() { }

    close() {
        this.modal.close();
    }
}