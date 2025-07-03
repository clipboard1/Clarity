import type {TagModel} from "../tags/TagModel.ts";

export interface TaskModel {
  id: string;
  title: string;
  description: string;
  tags: TagModel[];
  status: number;
}