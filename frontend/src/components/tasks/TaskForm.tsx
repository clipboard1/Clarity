import type { TaskFormProps } from "../../models/tasks/TaskFormProps.ts"

const TaskForm = ({isEditing, task, onInputChange, onSubmit}: TaskFormProps) => {
  return (<form onSubmit={onSubmit}>
    <div className="flex flex-col gap-4 mb-4 text-zinc-300">
      <div>
        <label htmlFor="title"
               className="block mb-2 text-sm font-medium ">Title</label>
        <input onChange={onInputChange}
               type="text" name="title" id="title"
               className="border border-zinc-500 text-sm rounded-xl
                  focus:ring-emerald-400/50 focus:border-emerald-400
                    block w-full p-2.5 placeholder:text-zinc-500"
               placeholder="Type task title"
               value={task?.title || ""}/>
      </div>
      <div>
        <label htmlFor="description"
               className="block mb-2 text-sm font-medium">Description</label>
        <textarea id="description" name="description"
                  className="block p-2.5 w-full text-sm rounded-lg border
                    border-zinc-500 focus:ring-emerald-400/50
                    focus:border-emerald-400 placeholder:text-zinc-500"
                  onChange={onInputChange}
                  placeholder="Write note here"
                  value={task?.description || ""}/>
      </div>
    </div>
    <button type="submit"
            className=" inline-flex items-center text-sm px-5 py-2.5 text-center
                bg-emerald-600 hover:bg-emerald-500 text-zinc-100
                border-transparent font-medium rounded-lg shadow-md
                transition duration-200">
      {isEditing ? "Update" : "Create"}
    </button>
  </form>)
}

export default TaskForm;