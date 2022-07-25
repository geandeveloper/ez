import { ModalStore } from 'src/app/shared/components/modal/modal.store';
import { ModalComponent } from './../../../shared/components/modal/modal.component';
import { PreLoaderStore } from 'src/app/shared/components/pre-loader/pre-loader.store';
import { AddressModel } from './../../../core/ezgym/address/address.model';
import { FormGroup, FormBuilder } from '@angular/forms';
import { finalize, tap, filter, catchError } from 'rxjs/operators';
import { Component } from '@angular/core';
import { debounceTime, of } from 'rxjs';
import { SearchByCepStore } from 'src/app/core/integrations/services/search-address/search-by-cep/search-by-cep.service';
import { Store } from 'src/app/core/state/store';

interface CreateGymComponentState {
    fantasyName: string,
    cnpj: string,
    address: AddressModel
}


@Component({
    selector: 'create-gym',
    templateUrl: 'create-gym.component.html'
})
export class CreateGymComponent extends Store<CreateGymComponentState> {

    creaGymFormGroup: FormGroup
    state?: CreateGymComponentState
    ui = {
        address1: 'Aguardando um cep valido',
        address2: '',
        address3: '',
        address4: '',
    }

    constructor(
        private searchByCepStore: SearchByCepStore,
        private fb: FormBuilder,
        private preloader: PreLoaderStore,
        private modal: ModalStore
    ) {
        super()

        this.creaGymFormGroup = fb.group({
            gym: this.fb.group({
                fantasyName: '',
                cnpj: ''
            }),
            address: this.fb.group({
                cep: '',
                number: this.fb.control({ value: '', disabled: false }),
                extraInformation: this.fb.control({ value: '', disabled: false }),
            })
        });

        this.store$.pipe(
            filter(state => Boolean(state?.address)),
            tap((state) => this.updateAddressDescription(state.address)),
        ).subscribe()
    }

    searchAddressByCep(cep: string) {
        this.preloader.show()
        this.searchByCepStore
            .searchAddressByCep(cep)
            .pipe(
                debounceTime(100),
                tap((response) => {
                    this.setState(state => ({
                        ...state,
                        address: {
                            number: this.creaGymFormGroup.get('address')?.get('number')?.value,
                            extraInformation: this.creaGymFormGroup.get('address')?.get('extraInformation')?.value,
                            cep: response.cep,
                            city: response.localidade,
                            neighborhood: response.bairro,
                            state: response.uf,
                            street: response.logradouro,
                        }
                    }))
                }),
                catchError(() => {
                    this.modal.error({
                        title: 'CEP invalido',
                        description: 'Por favor verificar cep e tentar novamente'
                    })
                    this.ui = {
                        address1: 'Endereco nao encontrado',
                        address2: '',
                        address3: '',
                        address4: '',
                    }
                    return of(false)
                }),
                finalize(() => {
                    this.preloader.close()
                })
            ).subscribe()
    }

    addressChanged() {
        this.setState(state => ({
            ...state,
            address: {
                ...state.address,
                number: this.creaGymFormGroup.get('address')?.get('number')?.value,
                extraInformation: this.creaGymFormGroup.get('address')?.get('extraInformation')?.value,
            }
        }))
    }

    private updateAddressDescription(address: AddressModel) {
        return this.ui = {
            address1: `${address.street}, ${address.number} - ${address.neighborhood}`,
            address2: `${address.city} - ${address.state}`,
            address3: `CEP: ${address.cep}`,
            address4: `${address.extraInformation}`
        }
    }

    createGym() {
        debugger

    }
}