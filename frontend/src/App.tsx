import Kanban from "./components/Kanban.tsx";

function App() {

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
      <Kanban/>
    </div>
  )
}

export default App
