const LoginForm = ({ onSubmit, onInputChange, onSwitchToRegister }) => {
  return (
    <form
      onSubmit={onSubmit}
      className="bg-black rounded-2xl shadow-xl w-full max-w-lg mx-auto px-6 py-6"
    >
      <h2 className="text-zinc-100 text-2xl font-semibold mb-4 text-center">
        Welcome back
      </h2>

      <div className="flex flex-col gap-4 mb-6 text-zinc-300">
        <div className="flex flex-col">
          <label htmlFor="email" className="mb-1 text-sm font-medium">
            Email
          </label>
          <input
            onChange={onInputChange}
            type="email"
            name="email"
            id="email"
            placeholder="you@example.com"
            className="border border-zinc-500 bg-black text-sm rounded-xl
                     focus:ring-emerald-400/50 focus:border-emerald-400 block w-full px-4 py-2.5
                     placeholder:text-zinc-500 text-zinc-300"
            value="admin@example.com"
          />
        </div>

        <div className="flex flex-col">
          <label htmlFor="password" className="mb-1 text-sm font-medium">
            Password
          </label>
          <input
            onChange={onInputChange}
            type="password"
            name="password"
            id="password"
            placeholder="••••••••"
            className="border border-zinc-500 bg-black text-sm rounded-xl
                     focus:ring-emerald-400/50 focus:border-emerald-400 block w-full px-4 py-2.5
                     placeholder:text-zinc-500 text-zinc-300"
            value="string"
          />
        </div>
      </div>

      <button
        type="submit"
        className="w-full inline-flex justify-center items-center text-sm px-5 py-2.5
                 bg-emerald-600 hover:bg-emerald-500 text-zinc-100
                 font-medium rounded-lg shadow-md transition duration-200 mb-4"
      >
        Login
      </button>

      <p className="text-sm text-zinc-400 text-center">
        Don't have an account?{" "}
        <button
          type="button"
          onClick={onSwitchToRegister}
          className="text-emerald-500 hover:underline hover:text-emerald-400 transition"
        >
          Register
        </button>
      </p>
    </form>
  );
};

export default LoginForm;