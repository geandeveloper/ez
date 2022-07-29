import { GymCreatedEvent } from './events/gym.model';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";

@Injectable({
    providedIn: 'root'
})
export class GymService {
    constructor(private http: HttpClient) { }

    createGym(command: any): Observable<GymCreatedEvent> {
        return this.http
            .post<GymCreatedEvent>("accounts", command)
    }
}