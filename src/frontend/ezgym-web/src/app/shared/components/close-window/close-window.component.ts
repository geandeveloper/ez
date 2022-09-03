import { Component, OnInit } from '@angular/core';


import { Browser } from '@capacitor/browser';

@Component({
    selector: 'success-account-setup',
    templateUrl: 'close-window.component.html',
    styleUrls: ['close-window.component.scss']
})

export class CloseWindowComponent implements OnInit {
    constructor() { }

    ngOnInit() { }

    close() {
        location.href = "http://exitme';"
    }
}