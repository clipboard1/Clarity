import {RequestService} from "./RequestService.ts";
import type {TagCreateRequest} from "../contracts/tags/TagCreateRequest.ts";

export class TagsService extends RequestService {
  constructor() {
    super(import.meta.env.VITE_API_URL + "tags");
  }

  public async createTag(createRequest: TagCreateRequest): Promise<number> {
    return this.handleFetch<number>(
      this.host, 'POST',
      true,
      this.defaultHeaders,
      JSON.stringify(createRequest))
  }

  public async deleteTag(id: number): Promise<boolean> {
    const url = `${this.host}/${id}`
    return this.handleFetch<boolean>(
      url, 'DELETE',
      false,
      this.defaultHeaders)
  }
}