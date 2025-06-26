export interface ModalProps {
  isOpen: boolean;
  onClose: () => void;
  header: string;
  children: React.ReactNode;
}