import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { ImageCroppedEvent, LoadedImage } from 'ngx-image-cropper';

@Component({
    selector: 'edit-profile',
    templateUrl: './edit-profile.component.html',
    styleUrls: ['edit-profile.component.scss']
})

export class EditProfileComponent implements OnInit {

    editProfileForm: FormGroup;
    showPreview = false;
    imageChangedEvent: any = '';
    croppedImage: any = '';

    constructor(
        private fb: FormBuilder,
        private modal: MatDialogRef<EditProfileComponent>) {

        this.editProfileForm = this.fb.group({
            avatar: [null],
            name: ['']
        })
    }

    ngOnInit() { }


    close() {
        this.modal.close()
    }


    fileChangeEvent(event: any): void {
        this.imageChangedEvent = event;
        this.showPreview = true;
    }
    imageCropped(event: ImageCroppedEvent) {
        this.croppedImage = event.base64;
    }
    imageLoaded() {
        // show cropper
    }
    cropperReady() {
        // cropper ready
    }
    loadImageFailed() {
        // show message
    }
}

