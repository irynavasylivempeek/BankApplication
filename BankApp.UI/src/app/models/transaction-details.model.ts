import User from './user.model';

export default class TransactionDetails {
  sender: User;
  receiver: User;
  type: string;
  amount: number;
  income: boolean;
}
