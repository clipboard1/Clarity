import type {TaskProps} from "../models/TaskProps.ts";

const Task =
  ({title, children, onDelete, onDragStart, onEdit, onTagCreate}
   : TaskProps) => {
    return (
      <div
        className="p-4 mb-3 bg-black text-zinc-300
      rounded-3xl shadow-md cursor-move flex flex-col gap-y-2
      transform transition-all duration-200
      hover:border hover:border-zinc-200"
        draggable
        onDragStart={onDragStart}
      >
        <div className="flex items-center justify-between">
          <span>{title}</span>
          <div className="flex flex-row">
            <button
              onClick={onEdit}
              className="text-zinc-400
                              hover:text-amber-300 transition-colors
                              duration-200 w-6 h-6 flex items-center
                              justify-center rounded-full
                              hover:bg-zinc-600"
            >
              <svg className="w-4 h-4" xmlns="http://www.w3.org/2000/svg" version="1.1"
                   viewBox="0 0 16 16" fill="none" stroke="currentColor"
                   stroke-linecap="round" stroke-linejoin="round"
                   stroke-width="1.5">
                <polygon points="1.75 11.25,1.75 14.25,4.75 14.25,14.25
                4.75,11.25 1.75"/>
                <line x1="8.75" y1="4.75" x2="11.25" y2="7.25"/>
              </svg>
            </button>
            <button
              onClick={onDelete}
              className="text-zinc-400
                              hover:text-rose-500 transition-colors
                              duration-200 w-6 h-6 flex items-center
                              justify-center rounded-full
                              hover:bg-zinc-600"
            >
              <svg aria-hidden="true" className="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
                <path
                  fillRule="evenodd"
                  d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414
                  1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293
                  4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z"
                  clipRule="evenodd"
                ></path>
              </svg>
            </button>
          </div>
        </div>
        <div className="w-fit flex flex-row gap-2">
          {children}
          <div
            className="rounded-xl px-2 py-1 gap-x-2 text-sm
            flex justify-between items-center hover:border
            hover:border-zinc-200"
          >
            <button
              onClick={onTagCreate}
              className="
                  text-zinc-200
                  transition-all
                  duration-200 py-1 px-3
                  rounded-full
                  flex
                  "
            >
              + Add tag
            </button>
          </div>
        </div>
      </div>)
  }

export default Task;