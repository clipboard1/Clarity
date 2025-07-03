import type {FormProps} from "../../models/modal/FormProps.ts";

const TaskForm = ({onInputChange, onSubmit}: FormProps) => {
  return (<form onSubmit={onSubmit}>
    <div className="flex flex-col gap-2 mb-4 text-zinc-300">
      <label
        htmlFor="name"
        className="block text-sm font-medium "
      >Name
      </label>
      <input
        onChange={onInputChange}
        type="text"
        name="name"
        id="name"
        className="border border-zinc-500 text-sm rounded-xl
                       focus:ring-emerald-400/50 focus:border-emerald-400 block w-full p-2.5
                       placeholder:text-zinc-500"
        placeholder="Type tag name"
      />
    </div>
    <button
      type="submit"
      className=" inline-flex items-center text-sm px-5 py-2.5 text-center
                bg-emerald-600 hover:bg-emerald-500 text-zinc-100
                 border-transparent font-medium rounded-lg shadow-md
                transition duration-200"
    >
      Create tag
    </button>
  </form>)
}

export default TaskForm;