import {RequestService} from "./RequestService.ts";
import type {TaskModel} from "../contracts/apptasks/TaskModel.ts";
import type {TaskCreateRequest} from "../contracts/apptasks/TaskCreateRequest.ts";
import type {TaskUpdateRequest} from "../contracts/apptasks/TaskUpdateRequest.ts";
import type {
  TaskChangeStatusRequest
} from "../contracts/apptasks/TaskChangeStatusRequest.ts";

export class TasksService extends RequestService {
  constructor() {
    super();
  }

  public async getTasks(): Promise<Array<TaskModel>> {
      return await this.handleFetch<Array<TaskModel>>("apptasks");
  }

  public async createTask(createRequest: TaskCreateRequest): Promise<string> {
    return this.handleFetch<string>(
     "apptasks", 'POST',
      true,
      this.defaultHeaders,
      JSON.stringify(createRequest))
  }

  public async updateTask(updateRequest: TaskUpdateRequest): Promise<null> {
    return this.handleFetch(
     "apptasks", 'PATCH',
      false,
      this.defaultHeaders,
      JSON.stringify(updateRequest))
  }

  public async changeStatus(changeStatusRequest: TaskChangeStatusRequest)
  : Promise<TaskChangeStatusRequest> {
    return this.handleFetch(
      "apptasks/change-status", 'PUT',
      false,
      this.defaultHeaders,
      JSON.stringify(changeStatusRequest))
  }

  public async deleteTask(id: string): Promise<boolean> {
    return this.handleFetch<boolean>(
      `apptasks/${id}`, 'DELETE',
      false,
      this.defaultHeaders)
  }
}