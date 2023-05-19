import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  registerMode = false;
  users: any;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getUsers();
  }

  registerToggle(){
    this.registerMode = !this.registerMode;
  }

  getUsers() {
    this.http.get('https://localhost/api/users').subscribe({
      next: (response: any) => this.users = response,
      error: (err: Error) => console.log(err),
      complete: () => console.log('Request has completed')
    });
  }

  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }
}
