import type {ModalProps} from "../../models/modal/ModalProps.ts";

const Modal  = ({ isOpen, onClose, header, children }: ModalProps) => {
  return (
    <div
      className={` fixed inset-0 z-50 flex items-center justify-center transition-opacity duration-10
            ${isOpen ? "opacity-100 pointer-events-auto" : "opacity-0 pointer-events-none"}`}
      onClick={onClose}
    >
      <div className={`
                absolute inset-0 bg-white/30 backdrop-blur-xl transition-all duration-10
                ${isOpen ? "opacity-100" : "opacity-0"}`} />

      <div className={`relative z-10 transform transition-all duration-200 
                    ${isOpen ? 'opacity-100 scale-100' : 'opacity-0 scale-95'}
                    w-full max-w-xl mx-4`}
           onClick={(e) => e.stopPropagation()}
      >
        <div key={header} className="p-6 w-full
                    bg-black rounded-lg shadow-xl">
          <div className="flex justify-between items-center mb-4 border-b
          border-zinc-300 pb-2">
            <h3 className="text-lg text-zinc-300 font-semibold">{header}</h3>
            <button
              onClick={onClose}
              type="button"
              className="p-1.5 ml-auto inline-flex items-center
                            text-zinc-300 bg-transparent hover:bg-white/40
                             hover:text-rose-500 rounded-lg text-sm ">
              <svg aria-hidden="true" className="w-5 h-5" fill="currentColor" viewBox="0 0 20 20">
                <path
                  fillRule="evenodd"
                  d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z"
                  clipRule="evenodd"
                ></path>
              </svg>
              <span className="sr-only">Close modal</span>
            </button>
          </div>
          {children}
        </div>
      </div>
    </div>
  )
}

export default Modal;