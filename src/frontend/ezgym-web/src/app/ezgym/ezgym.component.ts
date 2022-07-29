import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { tap, catchError, finalize } from 'rxjs';
import { UserInfoState, UserState } from '../core/authentication/user.state';
import { UserStore } from '../core/authentication/user.store';
import { ModalStore } from '../shared/components/modal/modal.store';
import { PreLoaderStore } from '../shared/components/pre-loader/pre-loader.store';


@Component({
  selector: 'ezgym',
  templateUrl: './ezgym.component.html',
  styleUrls: ['./ezgym.component.scss']
})
export class EzGymComponent implements OnInit {

  userState = {} as UserState 

  constructor(
    private userStore: UserStore,
    private modalStore: ModalStore,
    private preLoaderStore: PreLoaderStore,
    private router: Router,
    private activeRoute: ActivatedRoute
  ) {

    this.activeRoute.paramMap.subscribe((params: ParamMap) => {
    });
  }

  ngOnInit(): void {
    this.userStore.store$.subscribe(userState => {
      this.userState = userState
    })
    import("src/assets/templates/skydash/skydash")
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
