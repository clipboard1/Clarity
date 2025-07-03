import type {TaskModel} from "../../contracts/apptasks/TaskModel.ts";
import type {FormProps} from "../modal/FormProps.ts";

export interface TaskFormProps extends FormProps{
  isEditing: boolean;
  task?: TaskModel;
}