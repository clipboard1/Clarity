import {RequestService} from "./RequestService.ts";
import type {TagCreateRequest} from "../contracts/tags/TagCreateRequest.ts";

export class TagsService extends RequestService {
  constructor() {
    super();
  }

  public async createTag(createRequest: TagCreateRequest): Promise<number> {
    return this.handleFetch<number>(
      "tags", 'POST',
      true,
      this.defaultHeaders,
      JSON.stringify(createRequest))
  }

  public async deleteTag(id: number): Promise<boolean> {
    return this.handleFetch<boolean>(
      `tags/${id}`, 'DELETE',
      false,
      this.defaultHeaders)
  }
}