import { Router } from '@angular/router';
import { UserStore } from './../../core/authentication/user.store';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PreLoaderStore } from 'src/app/shared/components/pre-loader/pre-loader.store';
import { ModalStore } from 'src/app/shared/components/modal/modal.store';
import { catchError, finalize, of, tap } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup

  constructor(
    private userStore: UserStore,
    private modalStore: ModalStore,
    private preLoaderStore: PreLoaderStore,
    private formBuilder: FormBuilder,
    private router: Router
  ) {

    this.loginForm = this.formBuilder.group({
      userName: [],
      password: []
    })


  }

  ngOnInit(): void {
    this.preLoaderStore.show();
    this
      .userStore
      .refreshToken()
      .pipe(
        tap(user => {
          if (user?.authenticated)
            this.router.navigate(['/', user.userInfo?.userName])
        }),
        finalize(() => {
          this.preLoaderStore.close();
        })
      ).subscribe()
  }

  login() {
    this.preLoaderStore.show();
    this.userStore.authenticate({
      ...this.loginForm.value
    }).pipe(
      tap((response) => {
        const userName = this.loginForm.get('userName')?.value
        this.router.navigate(['/', userName])
      }),
      catchError(() => {
        this.modalStore.accessDenied({
          title: "Algo deu errado !",
          description: "Usuario ou senha invalidos, por favor tente novamente"
        })
        return of([])
      }),
      finalize(() => {
        this.preLoaderStore.close();
      })
    ).subscribe()

    return false;
  }
}
