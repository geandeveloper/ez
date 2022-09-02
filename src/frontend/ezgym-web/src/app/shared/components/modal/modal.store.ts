import { ModalComponent } from './modal.component';
import { MatDialog } from '@angular/material/dialog';
import { ModalState } from './modal.state';
import { Store } from 'src/app/core/state/store';
import { filter, tap } from 'rxjs/operators';
import { Injectable } from '@angular/core';

@Injectable()
export class ModalStore extends Store<ModalState> {

  public opened$ = this.store$
    .pipe(
      filter(state => state.opened == true)
    )

  public closed$ = this.store$
    .pipe(
      filter(state => state.opened == false)
    )


  constructor(private dialog: MatDialog) {
    super({
      confirmButtonLabel: 'Confirmar',
      opened: false,
      fixed: false,
      title: '',
      description: '',
      onConfirm: () => { dialog.closeAll() }
    })

    this.opened$.subscribe(state => {
      this.dialog.open(ModalComponent, { data: state, hasBackdrop: state.fixed, disableClose: true })
    })

    this.dialog.afterAllClosed.subscribe(() => this.close())
  }

  error(config?: { title: string, description: string }) {
    this.setState(state => ({
      ...state,
      opened: true,
      title: config?.title || "Opsss",
      description: config?.description || "Um erro não esperado ocorreu, por favor tente novamente !",
      iconSrc: 'assets/img/error.svg',
    }))
  }

  accessDenied(config?: { title: string, description: string }) {
    this.setState(state => ({
      ...state,
      opened: true,
      title: config?.title || 'Falha durante autentição',
      description: config?.description || 'Houve uma falha durante a verificação do usuario e senha, por favor tente novamente !',
      iconSrc: 'assets/img/access-denied.svg',
    }))
  }

  success(config?: { title: string, description: string, onConfirm?: () => void, confirmButtonLabel?: string }) {

    this.setState(state => ({
      ...state,
      opened: true,
      title: config?.title || 'Show de bola !',
      description: config?.description || 'Operação realizada com sucesso',
      iconSrc: 'assets/img/success.svg',
      confirmButtonLabel: config?.confirmButtonLabel || state.confirmButtonLabel,
      onConfirm: config?.onConfirm || this.state.onConfirm
    }))
  }


  close() {
    this.setState(state => ({
      ...state,
      opened: false
    }))
  }
}
