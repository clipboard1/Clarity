import type {ApiError} from "./ApiError.ts";

export interface KanbanProps {
  setError: (e: ApiError) => void
}