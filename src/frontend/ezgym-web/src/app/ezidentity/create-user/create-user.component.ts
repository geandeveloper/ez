import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { catchError, finalize, tap } from 'rxjs';
import { UserStore } from 'src/app/core/authentication/user.store';
import { ModalStore } from 'src/app/shared/components/modal/modal.store';
import { PreLoaderStore } from 'src/app/shared/components/pre-loader/pre-loader.store';

@Component({
  selector: 'app-create-user',
  templateUrl: './create-user.component.html',
  styleUrls: ['./create-user.component.scss']
})
export class CreateUserComponent {

  createUserForm: FormGroup

  constructor(
    private store: UserStore,
    private preLoaderStore: PreLoaderStore,
    private formBuilder: FormBuilder,
    private modalStore: ModalStore,
  ) {

    this.createUserForm = this.formBuilder.group({
      userName: [''],
      name: [''],
      email: [''],
      password: [''],
      confirmPassword: ['']
    })
  }

  register() {
    this.preLoaderStore.show();
    this.store.createUser({
      ...this.createUserForm.value
    }).pipe(
      tap(() => {
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
