import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ImageCroppedEvent, base64ToFile } from 'ngx-image-cropper';
import { combineLatest, finalize, map, merge, switchMap, tap } from 'rxjs';
import { UserStore } from 'src/app/core/authentication/user.store';
import { AccountService } from 'src/app/ezgym/core/services/account.service';
import { AccountModel } from 'src/app/ezgym/core/models/accout.model';
import { Store } from 'src/app/core/state/store';
import { PreLoaderStore } from 'src/app/shared/components/pre-loader/pre-loader.store';
import { ProfileModel } from 'src/app/ezgym/core/models/profile.model';


interface EditProfileState {
    ui?: {
        avatarChanged: boolean
    },
    account: AccountModel,
    profile: ProfileModel
}

@Component({
    selector: 'edit-profile',
    templateUrl: './edit-profile.component.html',
    styleUrls: ['edit-profile.component.scss']
})

export class EditProfileComponent extends Store<EditProfileState> implements OnInit {

    editProfileForm: FormGroup;
    showPreview = false;
    imageChangedEvent: any = '';

    account: AccountModel
    profile: ProfileModel

    constructor(
        private accountService: AccountService,
        private userStorage: UserStore,
        private fb: FormBuilder,
        private modal: MatDialogRef<EditProfileComponent>,
        private preloader: PreLoaderStore,
        @Inject(MAT_DIALOG_DATA) public data: { account: AccountModel, profile: ProfileModel }) {
        super({
            account: { ...data.account },
            profile: { ...data.profile }
        })

        this.account = data.account;
        this.profile = data.profile;

        this.editProfileForm = this.fb.group({
            avatar: [this.state.account.avatarUrl],
            name: [this.state.profile?.name],
            accountId: [this.state.account.id],
            jobDescription: [this.state.profile?.jobDescription],
            bioDescription: [this.state.profile?.bioDescription]
        })
    }

    ngOnInit() { }


    close() {
        this.modal.close()
    }


    fileChangeEvent(event: Event): void {
        this.imageChangedEvent = event;
        this.showPreview = true;
    }

    imageCropped(event: ImageCroppedEvent) {
        this.account = {
            ...this.account,
            avatarUrl: event.base64!
        }

        this.editProfileForm.patchValue({
            avatar: base64ToFile(event.base64!)
        })
    }

    cutCancel() {
        this.showPreview = false;

        this.setState(state => ({
            ...state,
            ui: {
                ...state.ui,
                avatarChanged: false,
            },
            account: {
                ...state.account,
                avatarUrl: this.data.account.avatarUrl
            }
        }))
    }

    cutAvatar() {
        this.setState(state => ({
            ...state,
            ui: {
                ...state.ui,
                avatarChanged: true
            },
            account: {
                ...state.account,
                avatarUrl: this.account.avatarUrl
            }
        }))

        this.showPreview = false;
    }


    saveProfile() {

        this.preloader.show();

        if (this.state.ui?.avatarChanged)
            this.accountService.changeAvatar({
                accountId: this.data.account.id,
                avatar: this.editProfileForm.get('avatar')?.value
            }).subscribe(avatar => {
                this.setState(state => ({
                    ...state,
                    ui: {
                        avatarChanged: false
                    }
                }))

                this.userStorage
                    .updateActiveAccount(account => ({
                        ...account,
                        avatarUrl: avatar.avatarUrl,
                    }))
            })

        this.accountService
            .upInsertProfile({ ...this.editProfileForm.value })
            .subscribe(profile => {
                this.userStorage
                    .updateActiveAccount(account => ({
                        ...account,
                        profile: profile
                    }))
                this.preloader.close()
            })
    }
}

