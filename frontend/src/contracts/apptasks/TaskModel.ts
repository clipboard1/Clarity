import type {TagModel} from "../tags/TagModel.ts";

export interface TaskModel {
  id: string;
  title: string;
  description: string;
  tags: Array<TagModel>;
  status: number;
}