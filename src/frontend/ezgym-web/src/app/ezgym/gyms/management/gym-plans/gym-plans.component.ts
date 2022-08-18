import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
    selector: 'gym-plans-component',
    templateUrl: 'gym-plans.component.html',
    styleUrls: ['gym-plans.component.scss']
})

export class GymPlansComponent implements OnInit {

    planFormGroup: FormGroup

    constructor(
        private modal: MatDialogRef<GymPlansComponent>,
        private fb: FormBuilder
    ) {

        this.planFormGroup = this.fb.group({
            planId: ['']
        })
    }

    ngOnInit() { }


    close() {
        this.modal.close();
    }
}