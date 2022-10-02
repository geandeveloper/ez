import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CheckInCreatedEvent } from '../events/player.events';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PlayerService {
  constructor(private http: HttpClient) {}

  createCheckIn(command: {
    playerId: string;
    gymAccountId: string;
    memberShipId: string;
  }): Observable<CheckInCreatedEvent> {
    return this.http.post<CheckInCreatedEvent>(
      `players/${command.playerId}/checkins`,
      command
    );
  }
}
