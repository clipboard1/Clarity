import Task from "./components/Task.tsx";
import Tag from "./components/Tag.tsx";

function App() {

  return (
    <>
    <Task
      name="sigma task"
      onDelete={() => console.log("Deleting task")}
      onDragStart={() => console.log("Dragging task")}
    >
      <Tag
        name="sigma tag"
        onDelete={() => console.log("Deleting tag")}
      />
    </Task>
    </>
  )
}

export default App
