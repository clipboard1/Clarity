import type {ColumnProps} from "../models/ColumnProps.ts";

const Column = ({name, children, onDragOver, onDrop} : ColumnProps) => {
  return (
    <div key="name" className="flex-shrink-0 w-auto bg-black rounded-3xl
          min-w-85 shadow-xl"
         onDragOver={onDragOver}
         onDrop={onDrop}
    >
      <div className="mt-5 ml-6 text-zinc-200 font-bold text-xl
            rounded-t-md"
      >
        {name}
      </div>
      <div className="p-3 max-h-[80vh] overflow-y-auto">
          {children}
          <button
            className="
                  text-zinc-200
                  transition-all
                  duration-200 py-1 px-3
                  rounded-full
                  flex
                  hover:border hover:border-zinc-200"
          > + Add task
          </button>
      </div>
    </div>
  )
}

export default Column;