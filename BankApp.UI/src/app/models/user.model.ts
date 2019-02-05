import Transaction from './transaction.model';
import TransactionDetails from './transaction-details.model';

export default class User {
  userId: number;
  userName: string;
  balance: number;
  transactions: Transaction | TransactionDetails;
}
