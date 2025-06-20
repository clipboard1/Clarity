export interface ColumnProps {
  name: string;
  children: Array<React.ReactNode>;
  onDragOver: (e: DragEvent) => void;
  onDrop: (e: DragEvent, newStatus: number) => void;
}