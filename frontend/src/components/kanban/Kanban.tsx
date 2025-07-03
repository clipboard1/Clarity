import Column from "./Column.tsx";
import Task from "../tasks/Task.tsx"
import Tag from "../tags/Tag.tsx"
import Modal from "../modal/Modal.tsx";
import TaskForm from "../tasks/TaskForm.tsx"
import {type FormEvent, useEffect} from "react";
import {ModalMode} from "../../models/modal/ModalMode.ts";
import ConfirmDialog from "../modal/ConfirmDialog.tsx";
import TagForm from "../tags/TagForm.tsx";
import {useTasks} from "../../hooks/useTasks.ts";
import {useTags} from "../../hooks/useTags.ts";
import {useModal} from "../../hooks/useModal.ts";
import type {KanbanProps} from "../../models/kanban/KanbanProps.ts";

const Kanban =
  ({setError}: KanbanProps) => {

  const {
    modalMode,
    setModalMode,
    resetModalStates,
    renderModalHeader
  } = useModal();

  const isEmpty = (str: string) => (!str?.length)

  const {
    tasks,
    setTasks,
    selectedTask,
    resetSelectedTask,
    onTaskInputChange,
    onOpenTaskEditModal,
    onTaskEdit,
    onTaskOpenCreateModal,
    onTaskCreate,
    onTaskDelete,
    onTaskDeleteConfirm,
    onDragStart,
    onDragOver,
    onDrop
  } = useTasks(setModalMode, setError, isEmpty)

  const {
    selectedTag,
    onTagInputChange,
    onTagOpenCreateModal,
    onTagCreate,
    onTagDelete,
    onTagDeleteConfirm
  } = useTags(tasks, setTasks, setModalMode, setError);



  useEffect(() => {
    resetModalStates();
    resetSelectedTask();
  }, [tasks])

  const getTasksForColumn = (status: number) => (
    tasks
      .filter((task) => task.status === status)
      .map((task) => (
        <Task
          key={task.id}
          title={task.title}
          onDelete={() => onTaskDelete(task.id)}
          onDragStart={() => onDragStart(task)}
          onEdit={(e) => {
            e.preventDefault();
            onOpenTaskEditModal(task.id);
          }}
          onTagCreate={() => onTagOpenCreateModal(task.id)}
        >
          {task.tags.map((tag) => (
            <Tag
              key={tag.id}
              name={tag.name}
              onDelete={() => onTagDelete(task.id, tag.id)}
            />
          ))}
        </Task>
      ))
  );

  const renderModalContent = () => {
    switch (modalMode) {
      case ModalMode.DELETE_TASK:
        return (
          <ConfirmDialog
            message="Are you sure want to delete this task ?"
            title={selectedTask?.title}
            onConfirm={() => onTaskDeleteConfirm(selectedTask.id)}
            onCancel={resetModalStates}
          />
        );
      case ModalMode.EDIT_TASK:
      case ModalMode.TASK_CREATE:
        return (
          <TaskForm
            isEditing={modalMode === ModalMode.EDIT_TASK}
            onSubmit={modalMode === ModalMode.EDIT_TASK ? onTaskEdit : onTaskCreate}
            onInputChange={onTaskInputChange}
            task={selectedTask}
          />
        );
      case ModalMode.TAG_CREATE :
        return (
          <TagForm
            onSubmit={(e: FormEvent) => onTagCreate(e)}
            onInputChange={onTagInputChange}
          />
        )
      case ModalMode.DELETE_TAG:
        return (
          <ConfirmDialog
            message="Are you sure want to delete "
            title={selectedTask?.tags.find((tag) => tag.id == selectedTag.id)?.name}
            onConfirm={() =>  onTagDeleteConfirm(selectedTag.id)}
            onCancel={resetModalStates}
          />
        );
      default:
        return null;
    }
  }

  return (
    <>
      <div
        className="flex flex-col lg:flex-row items-start gap-6
        overflow-x-auto pb-6"
      >
        <Column
          name="ToDo"
          onDragOver={(e) => onDragOver(e)}
          onDrop={(e) => onDrop(e, 0)}
          onOpenCreate={() => onTaskOpenCreateModal(0)}
        >
          {getTasksForColumn(0)}
        </Column>
        <Column
          name="In progress"
          onDragOver={(e) => onDragOver(e)}
          onDrop={(e) => onDrop(e, 1)}
          onOpenCreate={() => onTaskOpenCreateModal(1)}
        >
          {getTasksForColumn(1)}
        </Column>
        <Column
          name="Done"
          onDragOver={(e) => onDragOver(e)}
          onDrop={(e) => onDrop(e, 2)}
          onOpenCreate={() => onTaskOpenCreateModal(2)}
        >
          {getTasksForColumn(2)}
        </Column>
      </div>
      <Modal
        key={`modal-${selectedTask.id}-`}
        isOpen={modalMode !== ModalMode.NONE}
        onClose={resetModalStates}
        header={renderModalHeader()}
      >
        {renderModalContent()}
      </Modal>
    </>
  )
}

export default Kanban;