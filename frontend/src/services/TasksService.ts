import {RequestService} from "./RequestService.ts";
import type {TaskModel} from "../contracts/apptasks/TaskModel.ts";
import type {TaskCreateRequest} from "../contracts/apptasks/TaskCreateRequest.ts";
import type {TaskUpdateRequest} from "../contracts/apptasks/TaskUpdateRequest.ts";
import type {
  TaskChangeStatusRequest
} from "../contracts/apptasks/TaskChangeStatusRequest.ts";

export class TasksService extends RequestService {
  constructor() {
    super(import.meta.env.VITE_API_URL + "apptasks");
  }

  public async getTasks(): Promise<Array<TaskModel>> {
      return await this.handleFetch<Array<TaskModel>>(this.host);
  }

  public async createTask(createRequest: TaskCreateRequest): Promise<string> {
    return this.handleFetch<string>(
      this.host, 'POST',
      true,
      this.defaultHeaders,
      JSON.stringify(createRequest))
  }

  public async updateTask(updateRequest: TaskUpdateRequest): Promise<null> {
    return this.handleFetch(
      this.host, 'PATCH',
      false,
      this.defaultHeaders,
      JSON.stringify(updateRequest))
  }

  public async changeStatus(changeStatusRequest: TaskChangeStatusRequest)
  : Promise<TaskChangeStatusRequest> {
    const url = `${this.host}/change-status`
    return this.handleFetch(
      url, 'PUT',
      false,
      this.defaultHeaders,
      JSON.stringify(changeStatusRequest))
  }

  public async deleteTask(id: string): Promise<boolean> {
    const url = `${this.host}/${id}`
    return this.handleFetch<boolean>(
      url, 'DELETE',
      false,
      this.defaultHeaders)
  }
}