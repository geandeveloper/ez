import { CreateUserState } from './create-user.state';
import { Store } from 'src/app/core/state/store';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class CreateUserStore extends Store<CreateUserState>{

  constructor(private httpClient: HttpClient) {
    super()
  }

  register(user: CreateUserState) {
    return this.httpClient.post("user", user)
  }
}