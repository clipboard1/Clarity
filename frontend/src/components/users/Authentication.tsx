import Modal from "../modal/Modal.tsx";
import LoginForm from "./LoginForm.tsx";
import {type ChangeEvent, type FormEvent, useState} from "react";
import RegisterForm from "./RegisterForm.tsx";
import type {AuthenticationProps} from "../../models/users/AuthenticationProps.ts";

const Authentication = ({onLoginInputChange, onLogin, onRegisterInputChange, onRegister}
                        : AuthenticationProps) => {
  const [mode, setMode] = useState<'login' | 'register'>('login');

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
          onInputChange={(e :ChangeEvent<HTMLInputElement>) => onLoginInputChange(e)}
          onSubmit={(e: FormEvent) => onLogin(e)}
          onSwitchToRegister={() => setMode("register")}
        />
      : <RegisterForm
          onSubmit={(e: FormEvent) => onRegister(e)}
          onInputChange={(e :ChangeEvent<HTMLInputElement>) => onRegisterInputChange(e)}
          onSwitchToLogin={() => setMode("login")}
        />}

    </Modal>)
}

export default Authentication;