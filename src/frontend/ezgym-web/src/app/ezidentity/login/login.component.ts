import { Router } from '@angular/router';
import { UserStore } from './../../core/authentication/user.store';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PreLoaderStore } from 'src/app/shared/components/pre-loader/pre-loader.store';
import { ModalStore } from 'src/app/shared/components/modal/modal.store';

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
      email: [],
      password: []
    })

  }

  login() {

    this.preLoaderStore.show();
    this.userStore.authenticate({
      ...this.loginForm.value
    }).subscribe(
      () => {
        this.router.navigate(['/'])
        this.preLoaderStore.close();
      },
      error => {
        console.log(error)
        this.preLoaderStore.close();
        this.modalStore.error({
          title: "Algo deu errado !",
          description: "Usuario ou senha invalidos, por favor tente novamente"
        })

      }
    )

    return false;
  }

  ngOnInit(): void {
  }
}
