import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { switchMap, tap } from 'rxjs';
import { UserStore } from 'src/app/core/authentication/user.store';
import { AccountService } from 'src/app/core/ezgym/account.service';
import { GymService } from 'src/app/core/ezgym/gym.service';
import { GymModel, GymPlanModel, PaymentTypeEnum } from 'src/app/core/ezgym/models/gym.model';
import { Store } from 'src/app/core/state/store';


interface ComponentState {
    ui?: {
        planSelected: GymPlanModel,
        paymentSelected: PaymentTypeEnum
    }
    accountId: string,
    gym: GymModel,
    plans: { value: GymPlanModel, selected: boolean }[]
    paymentTypes: {
        value: PaymentTypeEnum,
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
        private fb: FormBuilder,
        private accountService: AccountService,
        private gymService: GymService,
        private userStore: UserStore,
        @Inject(MAT_DIALOG_DATA) public data: { accountId: string }) {

        super({
            accountId: data.accountId,
            gym: {} as GymModel,
            plans: [],
            paymentTypes: [
                {
                    name: "PIX",
                    description: "Pagamento instantaneo",
                    value: PaymentTypeEnum.Pix,
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

    paymentSelected(paymentType: PaymentTypeEnum) {
        this.setState(state => ({
            ...state,
            ui: {
                ...state.ui!,
                paymentSelected: paymentType
            },
            paymentTypes: state.paymentTypes.map(p => ({
                ...p,
                selected: p.value == paymentType
            }))
        }))
    }

    confirmRegister() { 
        this.gymService.registerMemberShip({
            gymId: this.state.gym.id,
            accountId: this.state.accountId,
            planId: this.state.ui?.planSelected.id
        }).subscribe(payment => {
            debugger
        })

    }

    ngOnInit() { }

    close() {
        this.modal.close();
    }
}