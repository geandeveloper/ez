import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { catchError, tap } from 'rxjs';
import { ModalStore } from 'src/app/shared/components/modal/modal.store';
import { PreLoaderStore } from 'src/app/shared/components/pre-loader/pre-loader.store';
import { CreateUserStore } from './create-user.store';

@Component({
  selector: 'app-create-user',
  templateUrl: './create-user.component.html',
  styleUrls: ['./create-user.component.scss'],
  providers: [
    CreateUserStore
  ]
})
export class CreateUserComponent {

  createUserForm: FormGroup

  constructor(
    private store: CreateUserStore,
    private preLoaderStore: PreLoaderStore,
    private formBuilder: FormBuilder,
    private modalStore: ModalStore
  ) {

    this.createUserForm = this.formBuilder.group({
      name: [''],
      email: [''],
      password: [''],
      confirmPassword: ['']
    })
  }

  register() {
    this.preLoaderStore.show();
    this.store.register({
      ...this.createUserForm.value
    }).pipe(
      tap(() => {
        this.preLoaderStore.close();
        this.modalStore.success({
          title: 'Seja bem vindo',
          description: 'Agora você faz parte da familia EZGym'
        })
      }),
      catchError((error) =>  {
        this.modalStore.error()
        this.preLoaderStore.close();
        return error;
      })
    ).subscribe()
  }
}
