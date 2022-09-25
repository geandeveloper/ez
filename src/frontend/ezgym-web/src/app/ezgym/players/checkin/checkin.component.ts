import { AccountService } from 'src/app/ezgym/core/services/account.service';
import { AccountMemberShipModel } from 'src/app/ezgym/core/models/accout.model';
import { Store } from 'src/app/core/state/store';
import { Component, OnInit } from '@angular/core';
import { EzGymStore } from '../../ezgym.store';
import { switchMap, tap } from 'rxjs';

interface ComponentState {
  memberShips?: AccountMemberShipModel[];
}

@Component({
  selector: 'checkin',
  templateUrl: './checkin.component.html',
  styleUrls: ['./checkin.component.scss'],
})
export class CheckinComponent extends Store<ComponentState> implements OnInit {
  constructor(
    private accountService: AccountService,
    private ezGymStore: EzGymStore
  ) {
    super();
  }

  ngOnInit(): void {
    this.ezGymStore.active$
      .pipe(
        switchMap((account) => this.accountService.getMemberShips(account.id)),
        tap((memberShips) => {
          this.setState((state) => ({ ...state, memberShips: memberShips }));
        })
      )
      .subscribe();
  }
}
