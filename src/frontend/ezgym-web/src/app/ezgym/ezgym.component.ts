import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { tap, catchError, finalize, mergeMap, map } from 'rxjs';
import { UserStore } from '../core/authentication/user.store';
import { Store } from '../core/state/store';
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
    private preLoaderStore: PreLoaderStore,
    private ezGymStore: EzGymStore,
    private router: Router,
    private activeRoute: ActivatedRoute
  ) {
    super()

  }

  ngOnInit(): void {
    this.ezGymStore.active$.subscribe(activeAccount => {
      this.setState(state => ({
        ...state,
        activeAccount: activeAccount
      }))
    })

    this.ezGymStore
      .loadAccounts()
      .pipe(
        mergeMap(accounts => this.activeRoute.params
          .pipe(
            tap(params => {
              var accountName = params['accountName']
              if (accounts.some(a => a.accountName == accountName) && !this.ezGymStore.state.accountActive)
                this.ezGymStore.setActiveAccount(accountName)
            }),
            map(() => accounts)
          ))
      )
      .subscribe(accounts => {
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

   
    import("src/assets/templates/skydash/skydash")

  }

  switchAccount(accountName: string): void {
    this.router.navigate([`/${accountName}`])
    this.ezGymStore.setActiveAccount(accountName)
  }

  logout() {
    this.preLoaderStore.show();
    this.userStore.revokeToken()
      .pipe(
        tap(() => {
          this.router.navigate(['/ezidentity/login'])
        }),
        catchError((error) => {
          this.router.navigate(['/ezidentity/login'])
          return error
        }),
        finalize(() => {
          this.preLoaderStore.close();
        })
      ).subscribe()

    return false;
  }
}
