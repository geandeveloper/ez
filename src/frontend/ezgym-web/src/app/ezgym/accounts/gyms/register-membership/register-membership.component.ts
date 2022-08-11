import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
    selector: 'register-membership',
    templateUrl: './register-membership.component.html',
    styleUrls: ['./register-membership.component.scss']
})

export class RegisterMembershipComponent implements OnInit {

    planFormGroup: FormGroup

    constructor(
        private modal: MatDialogRef<RegisterMembershipComponent>,
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