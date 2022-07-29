import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { tap } from 'rxjs';
import { UserStore } from 'src/app/core/authentication/user.store';

@Component({
    selector: 'profile',
    templateUrl: 'profile.component.html'
})

export class ProfileComponent implements OnInit {
    userName: string = ""

    constructor(
        private userStore: UserStore,
        private activeRoute: ActivatedRoute
    ) {

        this.activeRoute.params
            .pipe(
                tap(params => {
                    this.userName = params['accountName']
                })
            ).subscribe()
    }

    ngOnInit() {

    }
}