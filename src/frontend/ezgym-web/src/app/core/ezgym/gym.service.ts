import { GymCreatedEvent, GymMemberShipCreatedEvent, PlanCreatedEvent } from './events/gym.events';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { GymPlanModel } from './models/gym.model';

@Injectable({
    providedIn: 'root'
})
export class GymService {
    constructor(private http: HttpClient) { }

    createPlan(command: any): Observable<PlanCreatedEvent> {
        return this.http
            .post<PlanCreatedEvent>(`gyms/${command.gymId}/plans`, command)
    }

    registerMemberShip(command: any): Observable<GymMemberShipCreatedEvent> {
        return this.http
            .post<GymMemberShipCreatedEvent>(`gyms/${command.gymId}/memberships`, command)
    }

    loadPlans(gymId: string) {
        return this.http.get<GymPlanModel[]>(`gyms/${gymId}/plans`)
    }
}