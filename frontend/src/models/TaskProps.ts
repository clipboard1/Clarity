export interface TaskProps {
  title: string;
  children: React.ReactNode;
  onDelete: () => void;
  onDragStart: () => void;
}