import {RequestService} from "./RequestService.ts";
import type {LoginUserRequest} from "../contracts/users/LoginUserRequest.ts";
import type {RegisterUserRequest} from "../contracts/users/RegisterUserRequest.ts";

export class UsersService extends RequestService {
  constructor() {
    super();
  }

  public async check() : Promise<null> {
    return this.handleFetch(
      "users/check", 'GET',
      false,
      this.defaultHeaders,
    )
  }

  public async login(loginRequest: LoginUserRequest)
    : Promise<LoginUserRequest> {
    return this.handleFetch<LoginUserRequest>(
      "users/login", 'POST',
      true,
      this.defaultHeaders,
      JSON.stringify(loginRequest)
    )
  }

  public async register(registerRequest: RegisterUserRequest)
  : Promise<RegisterUserRequest> {
    return this.handleFetch<RegisterUserRequest>(
      "users/register", 'POST',
      false,
      this.defaultHeaders,
      JSON.stringify(registerRequest)
    )
  }

  public async logout()
  : Promise<boolean> {
    return this.handleFetch<boolean>(
      "user/logout", 'POST',
      false,
      this.defaultHeaders
    )
  }
}