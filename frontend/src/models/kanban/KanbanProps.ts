import type {ApiError} from "../notifications/ApiError.ts";

export interface KanbanProps {
  setError: (e: ApiError) => void
}