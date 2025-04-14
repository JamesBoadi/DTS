import { Task } from '../interface/interface';

export async function createTask(task: string): Promise<Task> {
    const response = await fetch(`http://localhost:5282/createTask`, {
        body: task,
        method: "POST"
    });

    var newTask = response.json();
    return newTask;
}

export async function getAllTasks(): Promise<Task[]> {
    const response = await fetch(`http://localhost:5282/tasks`);
    const tasks = await response.json();
    return tasks;
}

export async function getTask(id: string): Promise<Task> {
    const response = await fetch(`http://localhost:5282/task/${id}`);
    const task = await response.json();
    return task;
}

export async function updateTask(id: string, oldTask: string): Promise<Task> {
    const response = await fetch(`http://localhost:5282/task/${id}`, {
        body: oldTask,
        method: "POST"
    });
    
    const updatedTask = await response.json();
    return updatedTask;
}

export async function deleteTask(id: string): Promise<number> {
    const response = await fetch(`http://localhost:5282/task/${id}`);
    const status = await response.status;
    return status;
}



