import type {TagProps} from "../models/TagProps.ts";

const Tag = ({name, onDelete} : TagProps) => {

  return (
    <div className="border rounded-xl px-2 py-1 gap-x-2 text-sm
    bg-zinc-200 text-zinc-600 flex justify-between items-center
      max-w-[120px] overflow-hidden"
    >
      <span className="truncate block whitespace-nowrap overflow-hidden
      text-ellipsis"
      >
        {name}
      </span>
      <button
        onClick={onDelete}
        className="ml-2 text-zinc-400
        hover:text-rose-400 transition-colors
        duration-200 w-3 h-3 flex items-center
        justify-center rounded-full
        hover:bg-zinc-300 flex-shrink-0"
      >
        <svg aria-hidden="true" className="w-3 h-3" fill="currentColor" viewBox="0 0 20 20">
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
  )
}

export default Tag;