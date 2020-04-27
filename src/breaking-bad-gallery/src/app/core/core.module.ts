import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from './auth/auth.service';

import { AuthModule, OidcSecurityService } from 'angular-auth-oidc-client';
import { UnauthorizedComponent } from './auth/unauthorized/unauthorized.component';

@NgModule({
  declarations: [UnauthorizedComponent],
  imports: [
    CommonModule,
    HttpClientModule,
    RouterModule,
    FormsModule,
    AuthModule.forRoot()
  ],
  providers: [
    AuthService,
    OidcSecurityService
  ]
})
export class CoreModule { }
