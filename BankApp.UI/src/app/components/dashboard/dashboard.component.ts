import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { UserService } from '../../services/user.service';
import { TransactionService } from '../../services/transaction.service';
import { TransactionTypes } from '../../app.constants';

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
  error: string;

  constructor(private userService: UserService, private transactionService: TransactionService, private router: Router) {
    this.transaction = new Transaction();

    this.userService.getUserInfo().subscribe(result => {
      this.user = result;
    });

    this.userService.getAllUsers().subscribe(result => {
      this.transferReceivers = result;
    });

    this.transactionTypes = TransactionTypes;
  }

  makeTransaction() {
    this.transactionService.makeTransaction(this.transaction).subscribe(result => {
      if (result.success) {
        this.user = result.user;
      } else {
        this.error = result.errorMessage;
      }
    });
  }

  resetError() {
    this.error = '';
  }

  resetSubmit(form: any) {
    form.submitted = false;
  }

  ngOnInit() { }
}

