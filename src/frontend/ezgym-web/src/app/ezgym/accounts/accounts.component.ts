import { Component, OnInit, ViewChild } from '@angular/core';
import { Store } from 'src/app/core/state/store';
import { switchMap, tap } from 'rxjs';
import { AccountModel } from '../core/models/accout.model';
import { PlayerModel } from '../core/models/player.model';
import { AccountService } from '../core/services/account.service';
import { EzGymStore } from '../ezgym.store';
import { BottomModalComponent } from 'src/app/shared/components/bottom-modal/bottom-modal.component';

interface ComponentState {
  ui: {
    weekDays: { day: number; name: string }[];
    levelBarPercent: string;
    completeCheckinMode: boolean;
  };
  today?: number;
  activeAccount?: AccountModel;
  player?: PlayerModel;
}

@Component({
  selector: 'account-management',
  templateUrl: 'accounts.component.html',
  styleUrls: ['accounts.component.scss'],
})
export class AccountManagementComponent
  extends Store<ComponentState>
  implements OnInit
{
  @ViewChild(BottomModalComponent, { static: false })
  checkInModal?: BottomModalComponent;

  constructor(
    private accountService: AccountService,
    private ezGymStore: EzGymStore
  ) {
    super({
      ui: {
        weekDays: [
          { day: 0, name: 'Dom' },
          { day: 1, name: 'Seg' },
          { day: 2, name: 'Ter' },
          { day: 3, name: 'Qua' },
          { day: 4, name: 'Qui' },
          { day: 5, name: 'Sex' },
          { day: 6, name: 'Sab' },
        ],
        levelBarPercent: '0%',
        completeCheckinMode: false,
      },
      today: new Date().getDay(),
    });
  }

  ngOnInit() {
    this.ezGymStore.active$
      .pipe(
        tap((activeAccount) => {
          this.setState((state) => ({
            ...state,
            activeAccount: activeAccount,
          }));
        }),
        switchMap((activeAccount) =>
          this.accountService.getPlayer(activeAccount.id)
        ),
        tap((player) => {
          this.setState((state) => ({ ...state, player: player }));
        })
      )
      .subscribe();
  }

  switchDay(day: number) {
    this.setState((state) => ({
      ...state,
      today: day,
    }));
  }

  openCheckIn() {
    this.setState((state) => ({
      ...state,
      ui: {
        ...state.ui!,
        completeCheckinMode: true,
      },
    }));
    this.checkInModal?.open();
  }

  checkinModeClosed() {
    this.setState((state) => ({
      ...state,
      ui: {
        ...state.ui,
        completeCheckinMode: false,
      },
    }));
  }
}
