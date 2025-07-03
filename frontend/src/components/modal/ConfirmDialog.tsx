import type { ConfirmDialogProps } from "../../models/modal/ConfirmDialogProps.ts"

const ConfirmDialog = ({message, title, onConfirm, onCancel}: ConfirmDialogProps) => {
  return(
    <div >
      <div className="mb-3">
        <p className="block mb-2 text-zinc-300 text-sm font-medium ">{`${message} ${title || ""} ?`}</p>
      </div>
      <div className="flex gap-x-3">
        <button type="submit"
                onClick={onConfirm}
                className="inline-flex items-center font-medium text-sm px-5 py-2 text-center
                        bg-emerald-600 hover:bg-emerald-500 text-zinc-100 rounded-lg shadow-sm">
          Confirm
        </button>
        <button type="button"
                className="inline-flex items-center font-medium text-sm px-5 py-2 text-center
                        bg-rose-800 hover:bg-rose-700 text-zinc-100 rounded-lg shadow-sm"
                onClick={onCancel}>
          Cancel
        </button>
      </div>

    </div>
  )
}

export default ConfirmDialog;