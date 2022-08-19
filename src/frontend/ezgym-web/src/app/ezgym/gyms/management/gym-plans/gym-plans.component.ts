import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { UserStore } from 'src/app/core/authentication/user.store';
import { GymService } from 'src/app/core/ezgym/gym.service';
import { GymPlanModel } from 'src/app/core/ezgym/models/gym.model';
import { Store } from 'src/app/core/state/store';

interface GymPlansComponentState {
    plans: GymPlanModel[]
}

@Component({
    selector: 'gym-plans-component',
    templateUrl: 'gym-plans.component.html',
    styleUrls: ['gym-plans.component.scss']
})

export class GymPlansComponent extends Store<GymPlansComponentState> implements OnInit {

    planFormGroup: FormGroup

    constructor(
        private modal: MatDialogRef<GymPlansComponent>,
        private fb: FormBuilder,
        private gymService: GymService,
        private userStore: UserStore,
    ) {
        super({
            plans: []
        })


        this.planFormGroup = this.fb.group({
            planId: ['']
        })
    }

    ngOnInit() {

        this.gymService.loadPlans(this.userStore.state.userInfo?.gym?.id!).subscribe(plans => {
            debugger
        })
    }


    close() {
        this.modal.close();
    }
}