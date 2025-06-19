export interface TaskProps {
  name: string;
  children: React.ReactNode;
  onDelete: () => void;
  onDragStart: () => void;
}