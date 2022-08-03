import { Component, OnInit } from '@angular/core';
import { EzGymComponentStore } from '../../ezgym.component.store';

@Component({
    selector: 'search-account',
    templateUrl: './search-account.component.html'
})

export class SearchAccountComponent implements OnInit {
    constructor(private ezGymComponentStorage: EzGymComponentStore) {
        this.ezGymComponentStorage.showTopNavBar(false)
    }

    ngOnInit() { }
}