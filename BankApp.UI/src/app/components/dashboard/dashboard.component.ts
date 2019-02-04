import { Component, OnInit } from '@angular/core';
import {User} from '../../models/user.model';
import {UserService} from '../../services/user.service';
import {Router} from '@angular/router';
import {Transaction} from '../../models/transaction.model';
import {TransactionService} from '../../services/transaction.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  user: User;
  transferReceivers: User[];
  transaction: Transaction;
  constructor(private userService: UserService, private transactionService: TransactionService, private router: Router) {
    this.transaction = {
      senderId: 0,
      receiverId: 0,
      amount: 0,
      type: 0
    };
    this.userService.getUserInfo().subscribe(r => {
      this.user = r;
    });
    this.userService.getAllUsers().subscribe(r => {
      this.transferReceivers = r;
    });
  }
  ngOnInit() {
  }
  makeTransaction() {
    console.log(this.transaction);
    this.transactionService.makeTransaction(this.transaction).subscribe(r => {
      if (r.success) {
        this.user = r.user;
      }
    });
  }
}

