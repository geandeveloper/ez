import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { tap, catchError, finalize } from 'rxjs';
import { UserStore } from '../core/authentication/user.store';
import { Store } from '../core/state/store';
import { ModalStore } from '../shared/components/modal/modal.store';
import { PreLoaderStore } from '../shared/components/pre-loader/pre-loader.store';
import { AccountModel } from './core/models/accout.model';
import { EzGymStore } from './ezgym.store';


interface ComponentState {
  activeAccount?: AccountModel,
  accounts: AccountModel[],
  ui: {
    showTopNavBar: boolean
  }
}

@Component({
  selector: 'ezgym',
  templateUrl: './ezgym.component.html',
  styleUrls: ['./ezgym.component.scss']
})
export class EzGymComponent extends Store<ComponentState> implements OnInit {


  constructor(
    private userStore: UserStore,
    private modalStore: ModalStore,
    private preLoaderStore: PreLoaderStore,
    private ezGymStore: EzGymStore,
    private router: Router) {
    super()

    this.ezGymStore.active$.subscribe(activeAccount => {
      this.setState(state => ({
        ...state,
        activeAccount: activeAccount
      }))
    })

    this.ezGymStore.loadAccounts().subscribe(accounts => {
      this.setState(state => ({
        ...state,
        accounts: accounts
      }))
    })

    this.setState(state => ({
      ...state,
      ui: {
        showTopNavBar: true
      }
    }))
  }

  ngOnInit(): void {
    import("src/assets/templates/skydash/skydash")

  }

  switchAccount(accountName: string): void {
    this.ezGymStore.setActiveAccount(accountName)
    this.router.navigate(['/', accountName])
  }

  logout() {
    this.preLoaderStore.show();
    this.userStore.revokeToken()
      .pipe(
        tap(() => {
          this.router.navigate(['/ezidentity/login'])
        }),
        catchError((error) => {
          this.modalStore.error({
            title: "Algo deu errado !",
            description: "Por favor tente novamente"
          })
          return error
        }),
        finalize(() => {
          this.preLoaderStore.close();
        })
      ).subscribe()

    return false;
  }
}
