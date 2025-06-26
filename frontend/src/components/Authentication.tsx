import Modal from "./Modal.tsx";
import LoginForm from "./LoginForm.tsx";
import {type ChangeEvent, type FormEvent, useState} from "react";
import type {LoginUserRequest} from "../contracts/LoginUserRequest.ts";
import RegisterForm from "./RegisterForm.tsx";

const Authentication = ({fallbackFunc}) => {
  const [authData, setAuthData] =
    useState<LoginUserRequest>({email: "", password: ""});

  const [mode, setMode] = useState<'login' | 'register'>('login');

  const onInputChange =
    (event:ChangeEvent<HTMLInputElement>) => {
      const { name, value } = event.target;
      setAuthData((prev) => ({ ...prev, [name]: value }));
    };

  const onLogin = (e : FormEvent)=> {
    e.preventDefault()
    fallbackFunc();
  }

  return (
    <Modal
      key={`modal-login`}
      isOpen={true}
      onClose={() => {}}
      header={mode === "login"
      ? "LoginForm"
      : "Register"}
    >
      {mode === "login"
      ? <LoginForm
          onInputChange={onInputChange}
          onSubmit={onLogin}
          onSwitchToRegister={() => setMode("register")}
        />
      : <RegisterForm
          onSubmit={() => {}}
          onInputChange={() => {}}
          onSwitchToLogin={() => setMode("login")}
        />}

    </Modal>)
}

export default Authentication;