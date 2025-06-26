import {useState} from "react";
import {ModalMode} from "../models/ModalMode.ts";

export const useModal = () => {
  const [modalMode, setModalMode] = useState<ModalMode>(ModalMode.NONE);

  const resetModalStates = () => {
    setModalMode(ModalMode.NONE);
  };

  const renderModalHeader = () => {
    if (modalMode === ModalMode.NONE) return "";
    return modalMode;
  }

  return {
    modalMode,
    setModalMode,
    resetModalStates,
    renderModalHeader,
  }
}