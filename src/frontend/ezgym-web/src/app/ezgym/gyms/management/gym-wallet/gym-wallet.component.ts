import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
    selector: 'gym-wallet',
    templateUrl: 'gym-wallet.component.html',
    styleUrls: ['./gym-wallet.component.scss']
})

export class GymWalletComponent implements OnInit {

    pixFormGroup: FormGroup;

    constructor(
        private modal: MatDialogRef<GymWalletComponent>,
        private fb: FormBuilder
    ) {
        this.pixFormGroup = this.fb.group({
            keyType: [''],
            keyValue: ['']
        })

    }

    ngOnInit() { }

    close() {
        this.modal.close();
    }
}