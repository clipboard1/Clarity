import type {ApiError} from "../models/notifications/ApiError.ts";

const API_BASE_URL = import.meta.env.VITE_API_URL ?? "";

export class RequestService {
  protected readonly host: string;
  protected readonly defaultHeaders: HeadersInit = {
    "Content-Type": "application/json",
  }

  constructor(host: string = API_BASE_URL) {
    this.host = host;
  }

  async handleFetch<T>(url:string, method: string = 'GET',
                       expectJson = true,headers?: HeadersInit,
                       body?: string): Promise<T>
  {
    try {
      const response = await fetch(url, {
        method,
        headers: { ...this.defaultHeaders, ...headers},
        body,
        credentials: "include"
      })

      if (!response.ok) {
        let message = response.statusText;

        try {
          const errorData = await response.json();
          message = errorData["errors"].join("\n") || message;
        } catch {
          throw {
            status: response.status,
            message: response.status === 401 ? "Unauthorized"
              : response.status === 403 ? "Forbidden" : message,
          } as ApiError
        }
      }

      if (!expectJson) {
        return true as T;
      }

      return response.json() as Promise<T>

    } catch (error) {
      throw {
        status: (error as ApiError).status || 500,
        message: (error as ApiError).message || "Network error",
      } as ApiError
    }
  }
}