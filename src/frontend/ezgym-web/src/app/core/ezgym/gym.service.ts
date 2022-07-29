import { GymCreatedEvent } from './events/gym.events';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";

@Injectable({
    providedIn: 'root'
})
export class GymService {
    constructor(private http: HttpClient) { }

}