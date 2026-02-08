export interface ColumnProps {
  name: string;
  children: Array<React.ReactNode>;
  onOpenCreate: () => void;
  onDragOver: (e: React.DragEvent<HTMLDivElement>) => void;
  onDrop: (e: React.DragEvent<HTMLDivElement>, newStatus: number) => void;
}