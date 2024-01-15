import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../core/services/account.service';
import { EzGymStore } from '../../ezgym.store';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-membership',
  templateUrl: './membership.component.html',
  styleUrls: ['./membership.component.scss']
})
export class MembershipComponent implements OnInit {

  constructor(
    private readonly accountService: AccountService,
    private readonly ezGynStore: EzGymStore,
    private modal: MatDialogRef<MembershipComponent>) { }

  ngOnInit(): void {
    this.accountService.getMemberShips(this.ezGynStore.state.accountActive.id).subscribe(response => {
      console.log(response)
    })
  }

  close() {
    this.modal.close();
  }

}
