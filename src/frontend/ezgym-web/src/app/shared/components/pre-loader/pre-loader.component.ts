import { PreLoaderStore } from './pre-loader.store';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-pre-loader',
  templateUrl: './pre-loader.component.html',
  styleUrls: ['./pre-loader.component.scss']
})
export class PreLoaderComponent implements OnInit {

  show: boolean = false;

  constructor(private store: PreLoaderStore) { }

  ngOnInit(): void {
    this.store.showed$.subscribe(state => {
      this.show = state
    })
  }

}
