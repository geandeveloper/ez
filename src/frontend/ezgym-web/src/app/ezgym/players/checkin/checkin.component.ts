import { AccountService } from 'src/app/ezgym/core/services/account.service';
import { AccountMemberShipModel } from 'src/app/ezgym/core/models/accout.model';
import { Store } from 'src/app/core/state/store';
import { Component, Input, OnInit } from '@angular/core';
import { EzGymStore } from '../../ezgym.store';
import { switchMap, tap } from 'rxjs';
import { PlayerService } from '../../core/services/player.service';
import { PlayerModel } from '../../core/models/player.model';

interface ComponentState {
  ui?: {
    confirmMode: boolean;
  };
  memberShips?: AccountMemberShipModel[];
  selectedMemberShip: AccountMemberShipModel;
}

@Component({
  selector: 'checkin',
  templateUrl: './checkin.component.html',
  styleUrls: ['./checkin.component.scss'],
})
export class CheckinComponent extends Store<ComponentState> implements OnInit {
  constructor(
    private accountService: AccountService,
    private ezGymStore: EzGymStore,
    private playerService: PlayerService
  ) {
    super();
  }

  @Input()
  player?: PlayerModel;

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

  memberShipSelected(memberShipId: string) {
    this.setState((state) => ({
      ...state,
      ui: {
        confirmMode: true,
      },
      selectedMemberShip: state.memberShips?.find(
        (m) => m.id === memberShipId
      )!,
    }));
  }

  createCheckin() {
    debugger;
    this.playerService
      .createCheckIn({
        gymAccountId: this.state.selectedMemberShip.gymAccountId,
        memberShipId: this.state.selectedMemberShip.id,
        playerId: this.player?.id!,
      })
      .subscribe();
  }
}
