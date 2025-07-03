import {type ChangeEvent, type FormEvent, useState} from "react";
import type {TagModel} from "../contracts/tags/TagModel.ts";
import {ModalMode} from "../models/modal/ModalMode.ts";
import type {TaskModel} from "../contracts/apptasks/TaskModel.ts";
import {TagsService} from "../services/TagsService.ts";
import type {TagCreateRequest} from "../contracts/tags/TagCreateRequest.ts";
import type {ApiError} from "../models/notifications/ApiError.ts";
import loginForm from "../components/users/LoginForm.tsx";

export const useTags =
  (tasks: TaskModel[],
   setTasks: (tasks: TaskModel[]) => void,
   setModalMode: (mode: ModalMode) => void,
   setError: (e: ApiError) => void) => {

  const tagsService = new TagsService();

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
    const tempId = Math.floor(Math.random() * 1000);

    const newTag = {
      ...selectedTag,
      id: tempId
    };

    setTasks(prevTasks =>
      prevTasks.map(task =>
        task.id === selectedTag.taskId
          ? {
            ...task,
            tags: [...task.tags, newTag]
          }
          : task
      )
    );

    if (!selectedTag) return;

    tagsService.createTag({
      name: selectedTag.name,
      apptaskId: selectedTag.taskId
    } as TagCreateRequest)
      .then((createdTagId) => {
        console.log(createdTagId)
        setTasks(prevTasks =>
          prevTasks.map(task =>
            task.id === selectedTag.taskId
              ? {
                ...task,
                tags: task.tags.map(tag =>
                  tag.id === tempId
                    ? { ...tag, ...createdTagId }
                    : tag
                )
              }
              : task
          )
        );
      })
      .catch(e => {
        setError(e)
      });
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
    tagsService.deleteTag(id)
      .then(() =>
        setTasks(prevTasks =>
          prevTasks.map(task =>
            task.tags.some(tag => tag.id === id)
              ? {...task, tags: task.tags.filter(tag => tag.id !== id)}
              : task
          ))
      )
      .catch(e => {
      setError(e)
    });

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