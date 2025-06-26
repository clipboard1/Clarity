import type {TaskModel} from "../contracts/TaskModel.ts";
import type {FormProps} from "./FormProps.ts";

export interface TaskFormProps extends FormProps{
  isEditing: boolean;
  task?: TaskModel;
}