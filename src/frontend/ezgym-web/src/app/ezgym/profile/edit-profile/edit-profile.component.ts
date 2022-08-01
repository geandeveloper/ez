import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
    selector: 'edit-profile',
    templateUrl: './edit-profile.component.html',
    styleUrls: ['edit-profile.component.scss']
})

export class EditProfileComponent implements OnInit {
    constructor(public modal: MatDialogRef<EditProfileComponent>) { }

    ngOnInit() { }

    close() {
        this.modal.close()
    }
}