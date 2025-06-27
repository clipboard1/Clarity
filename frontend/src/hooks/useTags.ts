import {type ChangeEvent, type FormEvent, useState} from "react";
import type {TagModel} from "../contracts/tags/TagModel.ts";
import {ModalMode} from "../models/ModalMode.ts";
import type {TaskModel} from "../contracts/apptasks/TaskModel.ts";

export const useTags = (tasks: TaskModel[],
                        setTasks: (tasks: TaskModel[]) => void,
                        setModalMode: (mode: ModalMode) => void) => {
  const [selectedTag, setSelectedTag] = useState<TagModel>({
    id: 0, name: "", taskId: ""
  })

  const resetSelectedTag = () => {
    setSelectedTag({id: 0, name: "", taskId: ""})
  }

  const onInputChange =
    (event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
      const {name, value} = event.target;
      setSelectedTag((prev) => ({...prev, [name]: value}));
    };

  const onOpenCreateModal = (taskId: string) => {
    resetSelectedTag();
    setSelectedTag(prevTag => ({
      ...prevTag,
      taskId: taskId
    }));
    setModalMode(ModalMode.TAG_CREATE);
  }

  const onCreate = (e: FormEvent) => {
    e.preventDefault();
    setSelectedTag(prevTag => ({
      ...prevTag,
      id: Math.floor(Math.random())
    }));
    console.log(selectedTag)

    if (!selectedTag) return;

    setTasks(prevTasks =>
      prevTasks.map(task =>
        task.id === selectedTag.taskId
          ? {
            ...task,
            tags: [...task.tags, selectedTag]
          }
          : task
      )
    );
    console.log(tasks)
  }

  const onDelete = (taskId?: string, id?: number) => {
    const tag = tasks
      .find((task) => task?.id === taskId)
      ?.tags
      .find((tag) => tag?.id === id);
    if (tag) {
      setSelectedTag(tag);
      setModalMode(ModalMode.DELETE_TAG);
    }
  };

  const onDeleteConfirm = (id: number) => {
    setTasks(prevTasks =>
      prevTasks.map(task =>
        task.tags.some(tag => tag.id === id)
      ? {...task, tags: task.tags.filter(tag => tag.id !== id)}
        : task
      )
    )
  }

  return {
    selectedTag,
    onTagInputChange: onInputChange,
    onTagCreate: onCreate,
    onTagOpenCreateModal: onOpenCreateModal,
    onTagDelete: onDelete,
    onTagDeleteConfirm: onDeleteConfirm
  }
}