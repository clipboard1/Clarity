import Kanban from "./components/Kanban.tsx";
import Authentication from "./components/Authentication.tsx";
import useAuthentication from "./hooks/useAuthentication.ts";
import {useState} from "react";
import type {NotificationProps} from "./models/NotificationProps.ts";
import Notification from "./components/Notification.tsx";
import type {ApiError} from "./models/ApiError.ts";

function App() {

  const [notification, setNotification] =
    useState<NotificationProps | null>(null);

  const setError = (e: ApiError) => {
    setNotification({
      isError: true,
      status: e.status,
      message: e.message})
  }

  const {
    hasAuthToken,
    onLoginInputChange,
    onRegisterInputChange,
    onLogin,
    onRegister
  } = useAuthentication(setError)

  return (
    <div className="flex items-center justify-center flex-col
      gap-3 w-full max-w-6xl"
    >
      <h1 className="text-6xl font-bold text-zinc-200">
        Clarity
      </h1>
      <h2 className="text-zinc-300">
        Task manager which you want to use...
      </h2>
      {hasAuthToken ?
        <Kanban
          setError={setError}
        />
      : <Authentication
          onLoginInputChange={onLoginInputChange}
          onRegisterInputChange={onRegisterInputChange}
          onLogin={onLogin}
          onRegister={onRegister}
        />}
      {notification &&
        <Notification
          isError={notification.isError}
          status={notification.status}
          message={notification.message}
          onClose={() => (setNotification(null))}
        />}
    </div>
  )
}

export default App
