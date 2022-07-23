import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { JwtRequestInterceptor } from './core/interceptors/jwt-request.interceptor';
import { BaseUrlInterceptor } from './core/interceptors/base-url.interceptor';

import { AppComponent } from './app.component';

import { EzGymModule } from './ezgym/ezgym.module';
import { EzIdentityModule } from './ezidentity/ezidentity.module';
import { ModalModule } from './shared/components/modal/modal.module';
import { PreLoaderModule } from './shared/components/pre-loader/pre-loader.module';
import { AppRoutingModule } from './app-routing.module';


@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    HttpClientModule,
    RouterModule,
    BrowserModule,
    BrowserAnimationsModule,
    ModalModule,
    PreLoaderModule,
    EzGymModule,
    EzIdentityModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: BaseUrlInterceptor,
      multi: true
    },
    { provide: "BASE_API_URL", useValue: "/api" },
    { provide: HTTP_INTERCEPTORS, useClass: JwtRequestInterceptor, multi: true },
    // { provide: LocationStrategy, useClass: HashLocationStrategy }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
