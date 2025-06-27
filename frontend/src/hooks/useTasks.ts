import {type ChangeEvent, type FormEvent, useState} from "react";
import type {TaskModel} from "../contracts/apptasks/TaskModel.ts";
import {ModalMode} from "../models/ModalMode.ts";

export const useTasks = (setModalMode: (mode: ModalMode) => void,
                         isEmpty: (str: string) => boolean) => {
  const [tasks, setTasks] = useState<TaskModel[]>([
    {
      id: "new-guid-1", title: "Marker research", description: "", tags: [
        {id: 1, name: "Work", taskId: "new-guid-1"},
        {id: 2, name: "Personal", taskId: "new-guid-1"}
      ],
      status: 0
    },
    {
      id: "new-guid-2", title: "Sigma research", description: "", tags: [
        {id: 1, name: "Work", taskId: "new-guid-2"},
        {id: 2, name: "Personal", taskId: "new-guid-2"}
      ],
      status: 0
    },
  ])

  const [selectedTask, setSelectedTask] =
    useState<TaskModel>(
      {id: "", title: "", description: "", tags: [], status: 0});

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
    const task = tasks.find((note) => note?.id === id);
    if (task) {
      setSelectedTask(task);
      setModalMode(ModalMode.EDIT_TASK);
    }
  }

  const onEdit = (e: FormEvent) => {
    e.preventDefault();
    if (isEmpty(selectedTask?.title))
      return;
    setTasks(prevItems =>
      prevItems.map(item =>
        item.id === selectedTask?.id ? selectedTask : item))
  }

  const onOpenCreateModal = () => {
    resetSelectedTask();
    setModalMode(ModalMode.TASK_CREATE);
  }

  const onCreate = (e: FormEvent) => {
    e.preventDefault();
    setTasks((prev) => [...prev, selectedTask]);
  }

  const onDelete = (id?: string) => {
    const task = tasks.find((task) => task?.id === id);
    if (task) {
      setSelectedTask(task);
      setModalMode(ModalMode.DELETE_TASK);
    }
  };

  const onDeleteConfirm = (id: string) => {
    setTasks((prev) =>
      (prev.filter((task) => task.id !== id)));
  }

  const onDragStart = (task: TaskModel) => {
    setSelectedTask(task)
  }

  const onDragOver = (e: DragEvent) => {
    e.preventDefault();
  }

  const onDrop = (e: DragEvent, newStatus: number) => {
    e.preventDefault();

    if (!selectedTask) return;

    if (selectedTask.status === newStatus) return;

    const updatedTasks = tasks.map((task) =>
      task.id === selectedTask.id
        ? {...task, status: newStatus}
        : task
    );

    setTasks(updatedTasks);
    resetSelectedTask();
  }

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