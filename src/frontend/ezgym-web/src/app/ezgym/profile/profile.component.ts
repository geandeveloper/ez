import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';

@Component({
    selector: 'profile',
    templateUrl: 'profile.component.html'
})

export class ProfileComponent implements OnInit {
    userName: string = ""

    constructor(private activeRoute: ActivatedRoute) {
        debugger
        this.activeRoute.paramMap.subscribe((params: ParamMap) => {
            this.userName = params.get("id")!;
        });
    }

    ngOnInit() { }
}