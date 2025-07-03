import {type ChangeEvent, type FormEvent, useEffect, useState} from "react";
import {UsersService} from "../services/UsersService.ts";
import type {LoginUserRequest} from "../contracts/users/LoginUserRequest.ts";
import type {RegisterUserRequest} from "../contracts/users/RegisterUserRequest.ts";
import type {ApiError} from "../models/ApiError.ts";

const useAuthentication =
  (setError: (e: ApiError) => void) => {

  const authService = new UsersService();

  const [hasAuthToken, setHasAuthToken] = useState(false);

  useEffect(() => {
    isAuthorized()
  }, []);

  const isAuthorized = () => {
    authService.check()
      .then(() => setHasAuthToken(true))
      .catch(e => {
        setHasAuthToken(false)
        setError(e)
      });
  }

  const [loginData, setLoginData] =
    useState<LoginUserRequest>({
      email: "admin@example.com",
      password: "string"
    })

  const [registerData, setRegisterData] =
    useState<RegisterUserRequest>({
      username: "testUser",
      email: "testUser@example.com",
      password: "string"
    })

  const onInputChange =
    (event:ChangeEvent<HTMLInputElement>, setter) => {
      const { name, value } = event.target;
      setter((prev) => ({ ...prev, [name]: value }));
    };

  const onLogin = (e : FormEvent)=> {
    e.preventDefault()
    authService.login(loginData)
      .then(() => setHasAuthToken(true))
      .catch(e => {
        setHasAuthToken(false)
        setError(e)
      });
  }

  const onRegister = (e : FormEvent)=> {
    e.preventDefault()
    authService.register(registerData)
      .then(()  => location.reload())
      .catch(e => {
        setHasAuthToken(false)
        setError(e)
      });
  }

  return {
    hasAuthToken,
    onLoginInputChange:
      (e: ChangeEvent<HTMLInputElement>)=> onInputChange(e, setLoginData),
    onRegisterInputChange:
      (e: ChangeEvent<HTMLInputElement> )=> onInputChange(e, setRegisterData),
    onLogin,
    onRegister,
  }
}

export default useAuthentication;