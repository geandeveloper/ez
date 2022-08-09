import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { debounceTime } from 'rxjs';
import { AccountService } from 'src/app/core/ezgym/account.service';
import { AccountModel } from 'src/app/core/ezgym/models/accout.model';
import { Store } from 'src/app/core/state/store';


interface FollowerListComponentState {
    ui: {
        activeTab: string,
        tabIndex: number
    },
    followers: AccountModel[],
    following: AccountModel[]
}

@Component({
    selector: 'follower-list',
    templateUrl: 'follower-list.component.html',
    styleUrls: ['./follower-list.component.scss']
})

export class FollowerListComponent extends Store<FollowerListComponentState> implements OnInit {
    searchForm: FormGroup;

    constructor(
        private modal: MatDialogRef<FollowerListComponent>,
        private accountService: AccountService,
        private fb: FormBuilder,
        private router: Router,
        @Inject(MAT_DIALOG_DATA) public data: { account: AccountModel, ui: any }
    ) {
        super({
            ui: {
                ...data.ui,
                tabIndex: data.ui.activeTab === 'followers' ? 0 : 1
            },
            followers: [],
            following: []
        })

        if (data.ui.activeTab == 'followers') {
            this.searchFollowers(data.account.accountName, '')
                .subscribe(followers => {
                    this.setState(state => ({ ...state, followers: followers }))
                })
        }

        if (data.ui.activeTab == 'following') {
            this.searchFollowing(data.account.accountName, '').subscribe(following => {
                this.setState(state => ({ ...state, following: following }))
            })
        }

        this.searchForm = this.fb.group({
            followerQuery: [''],
            followingQuery: ['']
        })

        this.searchForm
            .get('followerQuery')
            ?.valueChanges
            .pipe(
                debounceTime(500)
            )
            .subscribe(query => {
                this.searchFollowers(data.account.accountName, query)
                    .subscribe(followers => {
                        this.setState(state => ({ ...state, followers: followers }))
                    })
            })

        this.searchForm
            .get('followingQuery')
            ?.valueChanges
            .pipe(
                debounceTime(500)
            )
            .subscribe(query => {
                this.searchFollowing(data.account.accountName, query)
                    .subscribe(following => {
                        this.setState(state => ({ ...state, following: following }))
                    })
            })
    }

    ngOnInit() { }

    navigateToAccount(accountName: string) {
        this.router.navigate(['/', accountName])
        this.close();
    }

    close() {
        this.modal.close();
    }


    tabChanged(tabIndex: number) {
        this.setState(state => ({
            ...state,
            ui: {
                activeTab: tabIndex === 0 ? 'followers' : 'following',
                tabIndex: tabIndex
            }
        }))

        if (this.state.ui.activeTab == 'followers') {
            this.searchFollowers(this.data.account.accountName, '')
                .subscribe(followers => {
                    this.setState(state => ({ ...state, followers: followers }))
                })
        }

        if (this.state.ui.activeTab == 'following') {
            this.searchFollowing(this.data.account.accountName, '').subscribe(following => {
                this.setState(state => ({ ...state, following: following }))
            })
        }

    }

    private searchFollowers(accountName: string, query: string) {
        return this.accountService
            .searchFollowers(accountName, query)
    }

    private searchFollowing(accountName: string, query: string) {
        return this.accountService
            .searchFollowing(accountName, query)
    }


}