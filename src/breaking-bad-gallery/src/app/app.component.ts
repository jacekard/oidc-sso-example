import { Component, OnInit, OnDestroy, Type } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, Subscription } from 'rxjs';
import { NgxSpinnerService } from "ngx-spinner";
import { AuthService, UserInfo, UserRole } from './core/auth/auth.service';

const apiUrl = "https://www.breakingbadapi.com/api/characters?limit=";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
  characters: Character[] = [];
  limit = 1;
  isAuthorized = false;
  isAuthorizedSubscription: Subscription;
  userInfo: UserInfo;
  UserRole = UserRole;

  constructor(
    private http: HttpClient,
    private spinner: NgxSpinnerService,
    private authService: AuthService) {
    this.isAuthorizedSubscription = this.authService.getIsAuthorized().subscribe(
      (isAuthorized: boolean) => {
        this.isAuthorized = isAuthorized;
        if(isAuthorized) {
          this.authService.getUserInfo().subscribe(userInfo => {
            this.userInfo = userInfo;
            console.log(this.userInfo);
          });
        }
      });
  }



  ngOnInit(): void {
    this.authService.initAuth();
    this.fetchCharacters();
  }

  ngOnDestroy(): void {
    this.isAuthorizedSubscription.unsubscribe();
    this.authService.ngOnDestroy();
  }

  public login() {
    this.authService.login();
  }

  public logout() {
    this.authService.logout();
  }

  fetchCharacters(): void {
    this.spinner.show();
    this.http.get<Character[]>(apiUrl + this.limit).subscribe(c => {
      this.characters = c;
      this.spinner.hide();
    });
  }
}

export interface Character {
  char_id: number,
  name: string,
  img: string,
  nickname: string
  portrayed: string,
  occupation: string,
  birthday: string,
  appearance: number[]
}