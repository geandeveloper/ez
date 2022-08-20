import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { switchMap, tap } from 'rxjs';
import { UserStore } from 'src/app/core/authentication/user.store';
import { AccountService } from 'src/app/core/ezgym/account.service';
import { GymService } from 'src/app/core/ezgym/gym.service';
import { GymModel, GymPlanModel } from 'src/app/core/ezgym/models/gym.model';
import { Store } from 'src/app/core/state/store';


interface ComponentState {
    ui?: {
        planSelected: boolean
    }
    gym: GymModel,
    plans: { value: GymPlanModel, selected: boolean }[]
    form: {
        planId?: string
    }
}

@Component({
    selector: 'register-membership',
    templateUrl: './register-membership.component.html',
    styleUrls: ['./register-membership.component.scss']
})

export class RegisterMembershipComponent extends Store<ComponentState> implements OnInit {

    planFormGroup: FormGroup

    constructor(
        private modal: MatDialogRef<RegisterMembershipComponent>,
        private fb: FormBuilder,
        private accountService: AccountService,
        private gymService: GymService,
        private userStore: UserStore
    ) {
        super({
            gym: {} as GymModel,
            plans: [],
            form: {}
        })

        this.planFormGroup = this.fb.group({
            planId: ['']
        })

        this.accountService
            .getGym(this.userStore.state.activeAccount?.id!)
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
                        plans: plans.map((p, index) => ({ value: p, selected: false }))
                    }))
                })
            ).subscribe()
    }

    planSelected(planId: string) {
        this.setState(state => ({
            ...state,
            ui: {
                planSelected: true
            },
            plans: state.plans.map(p => ({
                ...p,
                selected: p.value.id == planId
            }))
        }))
    }

    ngOnInit() { }

    close() {
        this.modal.close();
    }
}