import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { UserStore } from 'src/app/core/authentication/user.store';
import { GymService } from 'src/app/core/ezgym/gym.service';
import { GymPlanModel } from 'src/app/core/ezgym/models/gym.model';
import { Store } from 'src/app/core/state/store';
import { PreLoaderStore } from 'src/app/shared/components/pre-loader/pre-loader.store';

interface GymPlansComponentState {
    plans: GymPlanModel[]
}

@Component({
    selector: 'gym-plans-component',
    templateUrl: 'gym-plans.component.html',
    styleUrls: ['gym-plans.component.scss']
})

export class GymPlansComponent extends Store<GymPlansComponentState>  {

    planFormGroup: FormGroup

    constructor(
        private modal: MatDialogRef<GymPlansComponent>,
        private fb: FormBuilder,
        private gymService: GymService,
        private userStore: UserStore,
        private preloader: PreLoaderStore,
    ) {
        super({
            plans: []
        })


        this.planFormGroup = this.fb.group({
            name: [''],
            gymId: [this.userStore.state.userInfo?.gym?.id!],
            days: [30],
            price: [70],
            active: [true]
        })

        this.gymService
            .loadPlans(this.userStore.state.userInfo?.gym?.id!)
            .subscribe(plans => {
                this.setState(state => ({
                    ...state,
                    plans: plans
                }))
            })
    }

    createPlan() {
        this.preloader.show();
        this.gymService.createPlan({
            ...this.planFormGroup.value
        }).subscribe(response => {
            this.setState(state => ({
                ...state,
                plans: [
                    ...state.plans,
                    {
                        id: response.id,
                        accountId: response.command.accountId,
                        active: Boolean(response.command.active),
                        days: parseInt(response.command.days),
                        name: response.command.name,
                        price: parseFloat(response.command.price)
                    }
                ]
            }))

            this.preloader.close();
        })
    }


    close() {
        this.modal.close();
    }
}