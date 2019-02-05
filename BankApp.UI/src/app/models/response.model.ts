import User from './user.model';

export class LoginResult {
  success: boolean;
  errorMessage: string;
  user: User;
}
