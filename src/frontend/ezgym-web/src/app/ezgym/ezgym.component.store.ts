import { Injectable } from "@angular/core"
import { Store } from "../core/state/store"

export interface EzGymComponentState {
    ui: {
        showTopNavBar: boolean
    }
}

@Injectable({
    providedIn: 'root'
})
export class EzGymComponentStore extends Store<EzGymComponentState> {

    state: EzGymComponentState | undefined

    constructor() {
        super({
            ui: {
                showTopNavBar: true
            }
        })
    }

    showTopNavBar(show: boolean) {
        this.setState(state => ({
            ...state,
            ui: {
                ...state.ui,
                showTopNavBar: show
            }
        }))
    }

}