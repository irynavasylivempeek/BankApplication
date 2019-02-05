import User from './user.model';

export default class TransactionDetails {
  sender: User;
  receiver: User;
  typeDescription: string;
  amount: number;
  income: boolean;
}
