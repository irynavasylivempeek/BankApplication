import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { UserService } from '../../services/user.service';
import { TransactionService } from '../../services/transaction.service';
import { AuthType, TransactionTypes } from '../../app.constants';

import User from '../../models/user.model';
import Transaction from '../../models/transaction.model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})

export class DashboardComponent implements OnInit {

  user: User;
  transferReceivers: User[];
  transaction: Transaction;
  transactionTypes: any;

  constructor(private userService: UserService, private transactionService: TransactionService, private router: Router) {
    this.transaction = new Transaction();

    this.userService.getUserInfo().subscribe(r => {
      this.user = r;
    });

    this.userService.getAllUsers().subscribe(r => {
      this.transferReceivers = r;
    });

    this.transactionTypes = TransactionTypes;
  }

  ngOnInit() { }

  makeTransaction() {
    this.transactionService.makeTransaction(this.transaction).subscribe(r => {
      if (r.success) {
        this.user = r.user;
      }
    });
  }
}

