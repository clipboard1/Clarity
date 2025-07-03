import type {ChangeEvent, FormEvent} from "react";

export interface AuthenticationProps {
  onLoginInputChange: (e: ChangeEvent<HTMLInputElement>) => void;
  onLogin: (e: FormEvent) => void;
  onRegisterInputChange: (e: ChangeEvent<HTMLInputElement>) => void;
  onRegister: (e: FormEvent) => void;
}