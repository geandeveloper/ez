import { Component, OnInit } from '@angular/core';
import { Store } from 'src/app/core/state/store';


interface ComponentState {
    ui: {
        weekDays: { day: number, name: string }[]
    },
    today?: number
}

@Component({
    selector: 'account-management',
    templateUrl: 'account-management.component.html',
    styleUrls: ['account-management.component.scss']
})

export class AccountManagementComponent extends Store<ComponentState> implements OnInit {
    constructor() {
        super({
            ui: {
                weekDays: [
                    { day: 0, name: "Dom" },
                    { day: 1, name: "Seg" },
                    { day: 2, name: "Ter" },
                    { day: 3, name: "Qua" },
                    { day: 4, name: "Qui" },
                    { day: 5, name: "Sex" },
                    { day: 6, name: "Sab" }
                ]
            },
            today: new Date().getDay()
        })
    }

    ngOnInit() { }

    switchDay(day: number) {
        this.setState(state => ({
            ...state,
            today: day
        }))
    }
}