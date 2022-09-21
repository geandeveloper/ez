import { Component, Inject, OnInit } from '@angular/core';
import { waitForAsync } from '@angular/core/testing';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { filter, forkJoin, switchMap, tap } from 'rxjs';
import { Store } from 'src/app/core/state/store';
import {
  WalletModel,
  WalletReceiptModel,
} from '../../core/models/wallet.model';
import { WalletService } from '../../core/services/wallet.service';
import { GymManagementStore } from '../../gyms/gyms.store';

interface ComponentState {
  wallet: WalletModel;
  receipts?: WalletReceiptModel[];
}

@Component({
  selector: 'wallet-statement',
  templateUrl: 'wallet-statement.component.html',
  styleUrls: ['wallet-statement.component.scss'],
})
export class WalletStatementComponet
  extends Store<ComponentState>
  implements OnInit
{
  constructor(
    private modal: MatDialogRef<WalletStatementComponet>,
    private gymManagementStore: GymManagementStore,
    private walletService: WalletService,
    @Inject(MAT_DIALOG_DATA) public data: { wallet: WalletModel; ui: any }
  ) {
    super();

    this.gymManagementStore.store$
      .pipe(filter((state) => state.wallet != null))
      .subscribe((ezGymState) => {
        this.setState((state) => ({ ...state, wallet: ezGymState.wallet }));
      });
  }

  ngOnInit(): void {
    this.walletService
      .getReceipts(this.state.wallet.id)
      .subscribe((receipts) => {
        this.setState((state) => ({ ...state, receipts: receipts }));
      });
  }

  close() {
    this.modal.close();
  }

  saveChanges() {
    this.modal.close();
  }
}

