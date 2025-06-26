import type {FormEvent} from "react";

export interface TaskProps {
  title: string;
  children: React.ReactNode;
  onDelete: () => void;
  onDragStart: () => void;
  onEdit: (e) => void;
  onTagCreate: () => void;
}