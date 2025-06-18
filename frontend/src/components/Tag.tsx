import type {TagProps} from "../models/TagProps.ts";

const Tag = ({name, onDelete} : TagProps) => {

  return (
    <div className="border rounded-xl px-2 py-1 text-sm
    bg-zinc-200 text-zinc-600 flex justify-between items-center">
      <span>{name}</span>
      <button
        onClick={onDelete}
        className="text-zinc-400
        hover:text-rose-400 transition-colors
        duration-200 w-3 h-3 flex items-center
        justify-center rounded-full
        hover:bg-zinc-300"
      >
        X
      </button>
    </div>
  )
}

export default Tag;