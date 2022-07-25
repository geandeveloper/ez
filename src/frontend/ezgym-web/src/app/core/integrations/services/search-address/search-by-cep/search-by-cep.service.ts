import { SearchAddressResponse } from './../search-address.response';
import { tap } from 'rxjs/operators';
import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class SearchByCepStore {

  constructor(private http: HttpClient) { }

  searchAddressByCep(cep: string) {
    return this.http
      .get<SearchAddressResponse>(`integrations/services/search-address/${cep}`);
  }

}