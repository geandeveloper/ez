import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ImageCroppedEvent, base64ToFile } from 'ngx-image-cropper';
import { finalize, tap } from 'rxjs';
import { UserStore } from 'src/app/core/authentication/user.store';
import { AccountService } from 'src/app/core/ezgym/account.service';
import { AccountModel } from 'src/app/core/ezgym/models/accout.model';
import { ProfileModel } from 'src/app/core/ezgym/models/profile.model';
import { PreLoaderStore } from 'src/app/shared/components/pre-loader/pre-loader.store';

@Component({
    selector: 'edit-profile',
    templateUrl: './edit-profile.component.html',
    styleUrls: ['edit-profile.component.scss']
})

export class EditProfileComponent implements OnInit {

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

        this.account = data.account;
        this.profile = data.profile;

        this.editProfileForm = this.fb.group({
            avatar: [this.account.avatarUrl],
            name: [this.profile?.name],
            accountId: [this.account.id],
            jobDescription: [this.profile?.jobDescription],
            bioDescription: [this.profile?.bioDescription]
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

    changeAvatar() {
        this.preloader.show();
        this.accountService.changeAvatar({
            accountId: this.data.account.id,
            avatar: this.editProfileForm.get('avatar')?.value
        }).pipe(
            tap(response => {
                this.userStorage.updateActiveAccount(account => ({ ...account, avatarUrl: response.avatarUrl }))
            }),
            finalize(() => {
                this.showPreview = false;
                this.preloader.close()
            })
        ).subscribe()
    }

    saveProfile() {
        this.preloader.show();
        this.accountService.upInsertProfile({ ...this.editProfileForm.value }).pipe(
            tap(response => {
                this.userStorage.updateActiveAccount(account => ({ ...account, profile: { ...response } }))
            }),
            finalize(() => {
                this.preloader.close()
                this.modal.close()
            })
        ).subscribe()
    }
}

