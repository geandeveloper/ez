import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../core/services/account.service';
import { EzGymStore } from '../../ezgym.store';
import { MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AccountMemberShipModel } from '../../core/models/accout.model';
import { debounceTime } from 'rxjs';

@Component({
  selector: 'app-membership',
  templateUrl: './membership.component.html',
  styleUrls: ['./membership.component.scss']
})

export class MembershipComponent implements OnInit {

  searchForm: FormGroup;
  memberships: AccountMemberShipModel[] = [];
  selectedMemberships: AccountMemberShipModel[] = [];
  status: boolean = true;

  constructor(
    private readonly accountService: AccountService,
    private readonly ezGynStore: EzGymStore,
    private readonly fb: FormBuilder,
    private modal: MatDialogRef<MembershipComponent>) {
    this.searchForm = this.fb.group({
      search: [''],
    })

    this.searchForm
      .get('search')
      ?.valueChanges
      .pipe(
        debounceTime(500)
      )
      .subscribe((query: string) => {
          this.selectedMemberships = query ? this.memberships.filter(membership => 
              membership.gymAccountName.toLocaleLowerCase().includes(query?.toLocaleLowerCase()) && 
              membership.active === this.status
          ) : this.memberships.filter(membership => membership.active === this.status);
      })
  }

  ngOnInit(): void {
    this.accountService.getMemberShips(this.ezGynStore.state.accountActive.id).subscribe(response => {
      this.memberships = response;
      this.selectedMemberships = response.filter(membership => membership.active);
    })
  }

  close() {
    this.modal.close();
  }

  tabChanged(tabIndex: number) {
    this.searchForm.get('search')?.reset();
    this.status = !tabIndex;
    this.selectedMemberships = this.memberships.filter(membership => membership.active === !tabIndex);
  }
}
