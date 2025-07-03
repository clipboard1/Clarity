import {RequestService} from "./RequestService.ts";
import type {LoginUserRequest} from "../contracts/users/LoginUserRequest.ts";
import type {RegisterUserRequest} from "../contracts/users/RegisterUserRequest.ts";

export class UsersService extends RequestService {
  constructor() {
    super(import.meta.env.VITE_API_URL + "users");
  }

  public async check() : Promise<null> {
    const url = this.host + "/check"
    return this.handleFetch(
      url, 'GET',
      false,
      this.defaultHeaders,
    )
  }

  public async login(loginRequest: LoginUserRequest)
    : Promise<LoginUserRequest> {
    const url = this.host + "/login"
    return this.handleFetch<LoginUserRequest>(
      url, 'POST',
      true,
      this.defaultHeaders,
      JSON.stringify(loginRequest)
    )
  }

  public async register(registerRequest: RegisterUserRequest)
  : Promise<RegisterUserRequest> {
    const url = this.host + "/register"
    return this.handleFetch<RegisterUserRequest>(
      url, 'POST',
      false,
      this.defaultHeaders,
      JSON.stringify(registerRequest)
    )
  }

  public async logout()
  : Promise<boolean> {
    const url = this.host + "/logout"
    return this.handleFetch<boolean>(
      url, 'POST',
      false,
      this.defaultHeaders
    )
  }
}