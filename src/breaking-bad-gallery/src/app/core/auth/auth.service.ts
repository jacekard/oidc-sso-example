import { Injectable, Inject, OnDestroy } from '@angular/core';
import { AuthModule, ConfigResult, OidcConfigService, OidcSecurityService, OpenIdConfiguration, AuthWellKnownEndpoints, AuthorizationResult, AuthorizationState } from 'angular-auth-oidc-client';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Subscription, Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService implements OnDestroy {

  isAuthorized = false;
  private isAuthorizedSubscription: Subscription = new Subscription;
  userDataSubscription: Subscription;
  userInfo: UserInfo;

  constructor(
    private oidcSecurityService: OidcSecurityService,
    private http: HttpClient,
    private router: Router,
    @Inject('BASE_URL') private originUrl: string,
    @Inject('AUTH_URL') private authUrl: string,
  ) { }

  ngOnDestroy(): void {
    if (this.isAuthorizedSubscription) {
      this.isAuthorizedSubscription.unsubscribe();
      this.userDataSubscription.unsubscribe();
    }
  }

  public initAuth() {
    console.log(this.originUrl + 'callback');
    const openIdConfig: OpenIdConfiguration = {
      stsServer: this.authUrl,
      redirect_url: this.originUrl + 'callback',
      client_id: 'breakingbadgallery',
      response_type: 'code',
      scope: 'openid profile address roles',
      post_logout_redirect_uri: this.originUrl,
      forbidden_route: '/forbidden',
      unauthorized_route: '/unauthorized',
      silent_renew: true,
      silent_renew_url: this.originUrl + 'assets/silent-renewal.html',
      history_cleanup_off: true,
      auto_userinfo: true,
      log_console_warning_active: true,
      log_console_debug_active: true,
      max_id_token_iat_offset_allowed_in_seconds: 10,
    }

    const authWellKnownEndpoints: AuthWellKnownEndpoints = {
      issuer: this.authUrl,
      jwks_uri: this.authUrl + '/.well-known/openid-configuration/jwks',
      authorization_endpoint: this.authUrl + '/connect/authorize',
      token_endpoint: this.authUrl + '/connect/token',
      userinfo_endpoint: this.authUrl + '/connect/userinfo',
      end_session_endpoint: this.authUrl + '/connect/endsession',
      check_session_iframe: this.authUrl + '/connect/checksession',
      revocation_endpoint: this.authUrl + '/connect/revocation',
      introspection_endpoint: this.authUrl + '/connect/introspect',
    }

    this.oidcSecurityService.setupModule(openIdConfig, authWellKnownEndpoints);

    if (this.oidcSecurityService.moduleSetup) {
      this.doCallbackLogicIfRequired();
    } else {
      this.oidcSecurityService.onModuleSetup.subscribe(() => {
        this.doCallbackLogicIfRequired();
      });
    }

    this.isAuthorizedSubscription = this.oidcSecurityService.getIsAuthorized()
      .subscribe(isAuthorized => {
        this.isAuthorized = this.isAuthorized;
      });

    this.oidcSecurityService.onAuthorizationResult.subscribe(
      (authorizationResult: AuthorizationResult) => {
        this.onAuthorizationResultComplete(authorizationResult);
      });
  }

  private onAuthorizationResultComplete(authorizationResult: AuthorizationResult) {
    console.log('Auth result reveiced, auth state:'
      + authorizationResult.authorizationState
      + 'validation result:' + authorizationResult.validationResult);

    if (authorizationResult.authorizationState === AuthorizationState.unauthorized) {
      if (window.parent) {
        // sent from the child iframe, for example the silent renew
        this.router.navigate(['/unauthorized']);
      } else {
        window.location.href = '/unauthorized';
      }
    }
  }

  private doCallbackLogicIfRequired() {
    this.oidcSecurityService.authorizedCallbackWithCode(window.location.toString());
  }

  getIsAuthorized(): Observable<boolean> {
    return this.oidcSecurityService.getIsAuthorized();
  }

  login() {
    console.log('start login');
    this.oidcSecurityService.authorize();
  }

  logout() {
    console.log('start logoff');
    this.oidcSecurityService.logoff();
  }

  getUserInfo(): Observable<UserInfo> {
    return this.oidcSecurityService.getUserData<UserInfo>();
  }
}

export interface UserInfo {
  address: string,
  family_name: string,
  given_name: string,
  role: UserRole
  sub: string
}

export enum UserRole {
  PayingUser = "PayingUser",
  FreeUser = "FreeUser"
}