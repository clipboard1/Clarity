import type {TaskProps} from "../models/TaskProps.ts";

const Task = ({name, children, onDelete, onDragStart} : TaskProps) => {
  return (
    <div className="p-3 max-h-[80vh] overflow-y-auto">
      <div className="p-4 mb-3 bg-black text-zinc-300
      rounded-3xl shadow-md cursor-move flex flex-col gap-y-2
      ransform transition-all duration-200
      hover:border hover:border-zinc-200"
        draggable
        onDragStart={onDragStart}
      >
        <div className="flex items-center justify-between">
          <span>{name}</span>
          <button
            onClick={onDelete}
            className="text-zinc-400
                              hover:text-red-400 transition-colors
                              duration-200 w-6 h-6 flex items-center
                              justify-center rounded-full
                              hover:bg-zinc-600"
          >X
          </button>
        </div>
        <div className="w-fit flex flex-row flex-wrap gap-2">
          {children}
        </div>
      </div>
    </div>)
}

export default Task;