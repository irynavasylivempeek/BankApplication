import User from './user.model';

export default class Response {
  token: string;
  success: boolean;
  errorMessage: string;
  user: User;
}
