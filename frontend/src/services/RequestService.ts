import type {ApiError} from "../models/notifications/ApiError.ts";

const RAW_API_BASE_URL = import.meta.env.VITE_API_URL;

if (!RAW_API_BASE_URL) {
  throw new Error(
      "VITE_API_URL is not defined. Check .env.production or build args."
  )
}

const API_BASE_URL = RAW_API_BASE_URL.endsWith("/")
  ? RAW_API_BASE_URL 
  : `${RAW_API_BASE_URL}/`;

export class RequestService {
  protected readonly baseUrl: string;
  protected readonly defaultHeaders: HeadersInit = {
    "Content-Type": "application/json",
  }

  constructor(baseUrl: string = API_BASE_URL) {
    this.baseUrl = baseUrl;
  }
  
  protected buildUrl(path: string): string {
    return new URL(path, this.baseUrl).toString();
  }

  async handleFetch<T>(path:string, method: string = 'GET',
                       expectJson = true,headers?: HeadersInit,
                       body?: string): Promise<T>
  {
    const url = this.buildUrl(path);
    
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