import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { catchError, debounceTime, filter, finalize, pipe, switchMap, tap } from 'rxjs';
import { UserStore } from 'src/app/core/authentication/user.store';
import { AccountService } from 'src/app/core/ezgym/account.service';
import { ModalStore } from 'src/app/shared/components/modal/modal.store';
import { PreLoaderStore } from 'src/app/shared/components/pre-loader/pre-loader.store';

@Component({
  selector: 'app-create-user',
  templateUrl: './create-user.component.html',
  styleUrls: ['./create-user.component.scss']
})
export class CreateUserComponent {

  createUserForm: FormGroup
  ui = {
    userStatusClassName: 'bg-primary'
  }

  constructor(
    private store: UserStore,
    private accountService: AccountService,
    private preLoaderStore: PreLoaderStore,
    private fb: FormBuilder,
    private modalStore: ModalStore,
    private router: Router
  ) {

    this.createUserForm = this.fb.group({
      userName: this.fb.control('', [Validators.pattern(`^[a-z0-9_-]{2,15}$`), Validators.required]),
      name: [''],
      email: [''],
      password: [''],
      confirmPassword: ['']
    })

    this.createUserForm.get('userName')
      ?.valueChanges
      .pipe(
        filter(value => {
          const userName = value?.trim()
          if (userName.length)
            return true;

          this.ui = {
            ...this.ui,
            userStatusClassName: userName == '' ? 'bg-primary' : 'bg-danger'
          }

          return false;
        }),
        debounceTime(500),
        switchMap(userName => {
          return this
            .accountService
            .verifyAccount(userName)
            .pipe(
              tap(response => {
                this.ui = {
                  ...this.ui,
                  userStatusClassName: response.exists ? 'bg-danger' : 'bg-success'
                }
              })
            )
        }))
      .subscribe()
  }

  register() {
    this.preLoaderStore.show();
    this.store.createUser({
      ...this.createUserForm.value
    }).pipe(
      tap(() => {
        this.router.navigate(["/ezidentity/login"])
        this.modalStore.success({
          title: 'Seja bem vindo',
          description: 'Agora você faz parte da familia EZGym'
        })
      }),
      catchError((error) => {
        this.modalStore.error()
        return error;
      }),
      finalize(() => {
        this.preLoaderStore.close();
      })
    ).subscribe()
  }
}
