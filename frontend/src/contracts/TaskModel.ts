import type {TagModel} from "./TagModel.ts";

export interface TaskModel {
  id: string;
  title: string;
  description: string;
  tags: Array<TagModel>;
  status: number;
}