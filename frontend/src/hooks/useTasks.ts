import {type ChangeEvent, type FormEvent, useEffect, useState} from "react";
import type {TaskModel} from "../contracts/apptasks/TaskModel.ts";
import {ModalMode} from "../models/modal/ModalMode.ts";
import {TasksService} from "../services/TasksService.ts";
import type {
  TaskChangeStatusRequest
} from "../contracts/apptasks/TaskChangeStatusRequest.ts";
import type {ApiError} from "../models/notifications/ApiError.ts";

export const useTasks =
  (setModalMode: (mode: ModalMode) => void,
   setError: (e: ApiError) => void,
   isEmpty: (str: string) => boolean) => {

  const taskService = new TasksService();

  const [tasks, setTasks] = useState<TaskModel[]>([])

  const [selectedTask, setSelectedTask] =
    useState<TaskModel>(
      {id: "", title: "", description: "", tags: [], status: 0});

  useEffect(() => {
    taskService.getTasks()
      .then((result) =>
        setTasks(result))
  }, [])

  const resetSelectedTask = () => {
    setSelectedTask({
      id: "",
      title: "",
      description: "",
      tags: [],
      status: 0
    });
  }

  const onInputChange =
    (event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
      const {name, value} = event.target;
      setSelectedTask((prev) => ({...prev, [name]: value}));
    };

  const onOpenEditModal = (id?: string) => {
    const task = tasks.find((task) => task?.id === id);
    if (task) {
      setSelectedTask(task);
      setModalMode(ModalMode.EDIT_TASK);
    }
  }

  const onEdit = (e: FormEvent) => {
    e.preventDefault();
    if (isEmpty(selectedTask?.title))
      return;
    taskService.updateTask(selectedTask)
      .then(() =>
        setTasks(prevItems =>
          prevItems.map(item =>
            item.id === selectedTask?.id ? selectedTask : item))
      )
      .catch(e => {
        setError(e)
      });
  }

  const onOpenCreateModal = (status: number) => {
    resetSelectedTask();
    const updatedTask = { ...selectedTask, status: status };
    setSelectedTask(updatedTask);
    setModalMode(ModalMode.TASK_CREATE);
  }

  const onCreate = (e: FormEvent) => {
    e.preventDefault();
    taskService.createTask(selectedTask)
      .then((result) => {
        const updatedTask = { ...selectedTask, id: result };
        setSelectedTask(updatedTask);
        console.log(updatedTask);
        setTasks((prev) => [...prev, updatedTask]);
      })
      .catch(e => {
        setError(e)
      });
  };

  const onDelete = (id?: string) => {
    const task = tasks.find((task) => task?.id === id);
    if (task) {
      setSelectedTask(task);
      setModalMode(ModalMode.DELETE_TASK);
    }
  };

  const onDeleteConfirm = (id: string) => {
    taskService.deleteTask(id)
      .then(() =>
        setTasks((prev) =>
          (prev.filter((task) => task.id !== id)))
      )
      .catch(e => {
        setError(e)
      });
  }

  const onDragStart = (task: TaskModel) => {
    setSelectedTask(task)
  }

  const onDragOver = (e: React.DragEvent<HTMLDivElement>) => {
  e.preventDefault();
};

const onDrop = (
  e: React.DragEvent<HTMLDivElement>,
  newStatus: number
) => {
  e.preventDefault();

  if (!selectedTask) return;
  if (selectedTask.status === newStatus) return;

  taskService
    .changeStatus({
      appTaskId: selectedTask.id,
      newStatus,
    } as TaskChangeStatusRequest)
    .then(() => {
      setTasks(tasks =>
        tasks.map(task =>
          task.id === selectedTask.id
            ? { ...task, status: newStatus }
            : task
        )
      );
      resetSelectedTask();
    })
    .catch(e => setError(e));
};

  return {
    tasks,
    setTasks,
    selectedTask,
    resetSelectedTask,
    onTaskInputChange: onInputChange,
    onOpenTaskEditModal: onOpenEditModal,
    onTaskEdit: onEdit,
    onTaskOpenCreateModal: onOpenCreateModal,
    onTaskCreate: onCreate,
    onTaskDelete: onDelete,
    onTaskDeleteConfirm: onDeleteConfirm,
    onDragStart,
    onDragOver,
    onDrop
  }
}