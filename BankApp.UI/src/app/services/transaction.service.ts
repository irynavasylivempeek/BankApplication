import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Transaction} from '../models/transaction.model';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {

  constructor(private http: HttpClient) { }
  makeTransaction(transaction: Transaction): Observable<any> {
    let controller;
    switch (transaction.type) {
      case 0: controller = 'deposit';
              break;
      case 1: controller = 'withdraw';
              break;
      case 2: controller = 'transfer';
              break;
    }

    return this.http.post<any>(`/api/account/${controller}`,
      {
        senderId: transaction.senderId,
        receiverId: transaction.receiverId,
        amount: transaction.amount
      }
      );
  }
}
