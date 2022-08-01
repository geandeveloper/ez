import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { tap } from 'rxjs';
import { UserStore } from 'src/app/core/authentication/user.store';
import { AccountModel } from 'src/app/core/ezgym/models/accout.model';
import { EditProfileComponent } from './edit-profile/edit-profile.component';

@Component({
    selector: 'profile',
    templateUrl: 'profile.component.html',
    styleUrls: ['./profile.component.scss']
})

export class ProfileComponent implements OnInit {
    activeAccount = {} as AccountModel

    constructor(
        private userStore: UserStore,
        private activeRoute: ActivatedRoute,
        public dialog: MatDialog
    ) {

        this.activeRoute.params
            .pipe(
                tap(params => {
                    const userName = params['accountName']
                    const account = this.userStore.user.userInfo?.accounts.find(a => a.accountName == userName)
                    if (account) {
                        this.userStore.setActiveAccount(userName)
                        this.activeAccount = account
                    }
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