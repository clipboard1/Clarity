import { useEffect, useState } from "react";
import type { NotificationProps } from "../models/NotificationProps";

const Notification = ({isError, status, message, onClose } : NotificationProps) => {
  const [visible, setVisible] = useState(false);

  useEffect(() => {
    setVisible(true);
    const hideTimeout = setTimeout(() => setVisible(false), 3000);

    const removeTimeout = setTimeout(() => {
      onClose?.()
    }, 3500);

    return () => {
      clearTimeout(hideTimeout);
      clearTimeout(removeTimeout);
    }
  }, [])

  const headerColor = isError
    ? "text-rose-700"
    : "text-green-800"

  return  (
    <div className="fixed bottom-6 right-6 z-50">
      <div className={`
            rounded-xl min-w-[260px] max-w-sm w-fit flex gap-x-5 p-3 
            bg-black backdrop-blur-sm shadow-xl
            transition-all duration-500 ease-in-out
            ${visible ? "opacity-100 translate-y-0" : "opacity-0 translate-y-5"}`
      }>
        {isError
          ? <svg className="h-10 self-start mt-2" version="1.1" id="Layer_1"
                 xmlns="http://www.w3.org/2000/svg"
                 x="0px" y="0px" viewBox="0 0 122.879 122.879"
                 enable-background="new 0 0 122.879 122.879"><g>
            <path fill-rule="evenodd" clip-rule="evenodd" fill="#FF4141"
                  d="M61.44,0c33.933,0,61.439,27.507,61.439,61.439 s-27.506,61.439-61.439,61.439C27.507,
                          122.879,0,95.372,0,61.439S27.507,0,61.44,0L61.44,0z M73.451,39.151 c2.75-2.793,
                          7.221-2.805,9.986-0.027c2.764,2.776,2.775,7.292,0.027,10.083L71.4,61.445l12.076,
                          12.249 c2.729,2.77,2.689,7.257-0.08,10.022c-2.773,2.765-7.23,2.758-9.955-0.013L61.446,
                          71.54L49.428,83.728 c-2.75,2.793-7.22,2.805-9.986,
                          0.027c-2.763-2.776-2.776-7.293-0.027-10.084L51.48,
                          61.434L39.403,49.185 c-2.728-2.769-2.689-7.256,0.082-10.022c2.772-2.765,
                          7.229-2.758,9.953,0.013l11.997,12.165L73.451,39.151L73.451,39.151z"/></g>
          </svg>
          : <svg className="h-10 self-start mt-2" xmlns="http://www.w3.org/2000/svg" x="0px" y="0px" viewBox="0 0 48 48">
            <path fill="#c8e6c9" d="M44,24c0,11.045-8.955,20-20,20S4,35.045,4,
                        24S12.955,4,24,4S44,12.955,44,24z"></path><path fill="#4caf50"
                                                                        d="M34.586,14.586l-13.57,13.586l-5.602-5.586l-2.828,2.828l8.434,
                        8.414l16.395-16.414L34.586,14.586z"></path>
          </svg>}
        <div>
          <p className={`text-xl ${headerColor} font-medium`}>
            {isError
              ? `Error ${status}`
              : "Successful"}
          </p>
          <p className="mb-3 text-zinc-200 opacity-80">{message}</p>
        </div>
        <button
          onClick={onClose}
          type="button"
          className="ml-auto inline-flex items-center self-start
                            text-zinc-200 bg-transparent hover:bg-white/40
                             hover:text-rose-500 rounded-full text-sm">
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
    </div>
  )
}

export default Notification;