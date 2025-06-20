import Column from "./Column.tsx";
import type {TaskModel} from "../contracts/TaskModel.ts";
import Task from "./Task.tsx"
import Tag from "./Tag.tsx"
import { useState } from "react";

const Kanban = () => {
  const [tasks, setTasks] = useState<TaskModel[]>([
    {id: "new-guid-1", title: "Marker research", description: "", tags: [
        {id: 1, name: "Work", taskId: "new-guid-1"},
        {id: 2, name: "Personal", taskId: "new-guid-1"}
      ],
      status: 0
    },
    {id: "new-guid-2", title: "Sigma research", description: "", tags: [
        {id: 1, name: "Work", taskId: "new-guid-2"},
        {id: 2, name: "Personal", taskId: "new-guid-2"}
      ],
      status: 0
    },
  ])

  const [draggedItem, setDraggedItem] = useState<TaskModel>(null);

  const getTasksForColumn = (status) => (
    tasks
      .filter((task) => task.status === status)
      .map((task) => (
          <Task
            title={task.title}
            onDelete={() => {}}
            onDragStart={() => onDragStart(task)}
          >
            {task.tags.map((tag) => (
              <Tag
                name={tag.name}
                onDelete={() => {}}
              />
            ))}
          </Task>
      ))
    );

  const onDragStart = (task) => {
    setDraggedItem(task)
  }

  const onDragOver = (e: DragEvent) => {
    e.preventDefault();
  }

  const onDrop = (e: DragEvent, newStatus) => {
    e.preventDefault();

    if (!draggedItem) return;

    if (draggedItem.status === newStatus) return;

    const updatedTasks = tasks.map((task) =>
      task.id === draggedItem.id
        ? {...task, status: newStatus}
        : task
    );

    setTasks(updatedTasks);
    setDraggedItem(null);
  }


  return (
    <div className="flex flex-col lg:flex-row items-start gap-6
        overflow-x-auto pb-6">
      <Column
        name="ToDo"
        onDragOver={(e) => onDragOver(e)}
        onDrop={(e) => onDrop(e, 0)}
      >
        {getTasksForColumn(0)}
      </Column>
      <Column
        name="In progress"
        onDragOver={(e) => onDragOver(e)}
        onDrop={(e) => onDrop(e, 1)}
      >
        {getTasksForColumn(1)}
      </Column>
      <Column
        name="Done"
        onDragOver={(e) => onDragOver(e)}
        onDrop={(e) => onDrop(e, 2)}
      >
        {getTasksForColumn(2)}
      </Column>
    </div>
  )
}

export default Kanban;