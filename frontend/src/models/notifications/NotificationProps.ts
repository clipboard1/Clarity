export interface NotificationProps {
  status: number;
  message: string;
  isError: boolean;
  onClose?: () => void;
}