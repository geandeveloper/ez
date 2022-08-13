import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { RegisterMembershipComponent } from '../register-membership/register-membership.component';

@Component({
    selector: 'gym-profile',
    templateUrl: 'gym-profile.component.html',
    styleUrls: ['gym-profile.component.scss']
})

export class GymProfileComponent implements OnInit {
    constructor(
        public dialog: MatDialog
    ) { }

    ngOnInit() {
    }
    registerNewMemberShip() {

        this.dialog.open(RegisterMembershipComponent, {
            data: {
            },
            panelClass: 'fullscreen-dialog',
            maxWidth: '935px',
            maxHeight: '100vh',
            height: '100%',
            width: '100%',
        })
        .afterClosed()
        .subscribe()
    }
}