import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { tap } from 'rxjs';
import { UserStore } from 'src/app/core/authentication/user.store';
import { EditProfileComponent } from './edit-profile/edit-profile.component';

@Component({
    selector: 'profile',
    templateUrl: 'profile.component.html',
    styleUrls: ['./profile.component.scss']
})

export class ProfileComponent implements OnInit {
    userName: string = ""

    constructor(
        private userStore: UserStore,
        private activeRoute: ActivatedRoute,
        public dialog: MatDialog
    ) {

        this.activeRoute.params
            .pipe(
                tap(params => {
                    this.userName = params['accountName']
                    if (this.userStore.user.userInfo?.accounts.some(a => a.accountName == this.userName))
                        this.userStore.setActiveAccount(this.userName)
                })
            ).subscribe()
    }

    ngOnInit() {

    }

    myFooList = ['Some Item', 'Item Second', 'Other In Row', 'What to write', 'Blah To Do']
    editProfile() {
        const editProfileDialog = this.dialog.open(EditProfileComponent, {
            data: this.myFooList,
            panelClass: 'fullscreen-dialog',
            maxWidth: '935px',
            maxHeight: '100vh',
            height: '100%',
            width: '100%',
        });
        editProfileDialog.afterClosed().subscribe((res) => {

        });
    }
}