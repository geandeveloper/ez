import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { finalize, switchMap, tap } from 'rxjs';
import { UserStore } from 'src/app/core/authentication/user.store';
import { AccountService } from 'src/app/ezgym/core/services/account.service';
import { GymService } from 'src/app/ezgym/core/services/gym.service';
import { Store } from 'src/app/core/state/store';
import { PreLoaderStore } from 'src/app/shared/components/pre-loader/pre-loader.store';
import { GymModel, GymPlanModel } from 'src/app/ezgym/core/models/gym.model';
import { EzGymStore } from 'src/app/ezgym/ezgym.store';

interface GymPlansComponentState {
    gym: GymModel,
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
        private accountService: AccountService,
        private preloader: PreLoaderStore,
        private ezGymStore: EzGymStore
    ) {
        super({
            gym: {} as GymModel,
            plans: []
        })


        this.planFormGroup = this.fb.group({
            name: [''],
            gymId: [],
            days: [30],
            amount: [70],
            active: [true]
        })

        this.store$.subscribe(state => {
            this.planFormGroup.patchValue({
                gymId: state.gym.id
            })
        })


        this.accountService
            .getGym(this.ezGymStore.state.accountActive?.id!)
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
                        plans: plans
                    }))
                })
            ).subscribe()
    }

    createPlan() {
        this.preloader.show();
        this.gymService.createPlan({
            ...this.planFormGroup.value
        }).pipe(
            tap(response => {
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
                            amount: parseFloat(response.command.amount)
                        }
                    ]
                }))
            }),
            finalize(() => {
                this.preloader.close();
            })
        ).subscribe()
    }

    close() {
        this.modal.close();
    }
}