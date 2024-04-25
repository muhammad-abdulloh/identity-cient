import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrl: './users.component.scss'
})
export class UsersComponent implements OnInit {
  iterator = 0;
  ngOnInit(): void {
    this.iterator += 1
    console.log(this.iterator);
  }

}
