import User from './user.model';

export class Response {
  success: boolean;
  errorMessage: string;
  user: User;
}
