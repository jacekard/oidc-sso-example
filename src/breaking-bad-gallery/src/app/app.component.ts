import { Component, OnInit } from '@angular/core';
import { HttpClient }    from '@angular/common/http';
import { Observable } from 'rxjs';
import { NgxSpinnerService } from "ngx-spinner";

const apiUrl = "https://www.breakingbadapi.com/api/characters?limit=";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  characters: Character[] = [];
  limit = 1;

  constructor(
    private http: HttpClient,
    private spinner: NgxSpinnerService) {}
  
  ngOnInit(): void {
    this.fetchCharacters();
  }

  fetchCharacters(): void {
    console.log(this.limit)
    this.spinner.show();
    this.http.get<Character[]>(apiUrl + this.limit).subscribe(c => {
      this.characters = c;
      this.spinner.hide();
    });
  }

  
  formatLabel(value: number) {
    if (value >= 1000) {
      return Math.round(value / 1000) + 'k';
    }

    return value;
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