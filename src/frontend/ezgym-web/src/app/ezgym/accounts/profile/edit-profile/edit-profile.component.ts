import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ImageCroppedEvent, base64ToFile } from 'ngx-image-cropper';
import { AccountService } from 'src/app/ezgym/core/services/account.service';
import { Store } from 'src/app/core/state/store';
import { EzGymStore } from 'src/app/ezgym/ezgym.store';
import { finalize, of, switchMap, tap } from 'rxjs';
import { AccountProfileProjection } from 'src/app/ezgym/core/projections/account-profile.projection';

interface EditProfileState {
  ui?: {
    avatarChanged: boolean;
    loading?: boolean;
  };
  accountProfile: AccountProfileProjection;
}

@Component({
  selector: 'edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['edit-profile.component.scss'],
})
export class EditProfileComponent
  extends Store<EditProfileState>
  implements OnInit
{
  editProfileForm: FormGroup;
  showPreview = false;
  imageChangedEvent: any = '';

  accountProfile: AccountProfileProjection;

  constructor(
    private accountService: AccountService,
    private fb: FormBuilder,
    private modal: MatDialogRef<EditProfileComponent>,
    private ezGymStore: EzGymStore,
    @Inject(MAT_DIALOG_DATA)
    public data: { accountProfile: AccountProfileProjection }
  ) {
    super({
      accountProfile: { ...data.accountProfile },
    });

    this.accountProfile = data.accountProfile;

    this.editProfileForm = this.fb.group({
      avatar: [this.state.accountProfile.avatarUrl],
      name: [this.state.accountProfile?.profile?.name],
      accountId: [this.state.accountProfile.id],
      jobDescription: [this.state.accountProfile.profile?.jobDescription],
      bioDescription: [this.state.accountProfile.profile?.bioDescription],
    });
  }

  ngOnInit() {}

  close() {
    this.modal.close();
  }

  fileChangeEvent(event: Event): void {
    this.imageChangedEvent = event;
    this.showPreview = true;
  }

  imageCropped(event: ImageCroppedEvent) {
    this.accountProfile = {
      ...this.accountProfile,
      avatarUrl: event.base64!,
    };

    this.editProfileForm.patchValue({
      avatar: base64ToFile(event.base64!),
    });
  }

  cutCancel() {
    this.showPreview = false;

    this.setState((state) => ({
      ...state,
      ui: {
        ...state.ui,
        avatarChanged: false,
      },
      accountProfile: {
        ...state.accountProfile,
        avatarUrl: this.data.accountProfile.avatarUrl,
      },
    }));
  }

  cutAvatar() {
    this.setState((state) => ({
      ...state,
      ui: {
        ...state.ui,
        avatarChanged: true,
      },
      accountProfile: {
        ...state.accountProfile,
        avatarUrl: this.accountProfile.avatarUrl,
      },
    }));

    this.showPreview = false;
  }

  saveProfile() {
    this.setLoading(true);

    this.accountService
      .upInsertProfile({ ...this.editProfileForm.value })
      .pipe(
        tap((profile) => {
          this.ezGymStore.updateAccountActive((account) => ({
            ...account,
            profile: profile,
          }));
        }),
        switchMap((profile) => {
          if (this.state.ui?.avatarChanged) {
            return this.accountService
              .changeAvatar({
                accountId: this.data.accountProfile.id,
                avatar: this.editProfileForm.get('avatar')?.value,
              })
              .pipe(
                tap((avatar) => {
                  this.setState((state) => ({
                    ...state,
                    ui: {
                      avatarChanged: false,
                    },
                  }));

                  this.ezGymStore.updateAccountActive((account) => ({
                    ...account,
                    avatarUrl: avatar.avatarUrl,
                  }));
                })
              );
          }
          return of(profile);
        }),
        finalize(() => {
          this.setLoading(false);
        })
      )
      .subscribe();
  }

  setLoading(loading: boolean) {
    this.setState((state) => ({
      ...state,
      ui: {
        ...state.ui!,
        loading: loading,
      },
    }));
  }
}
