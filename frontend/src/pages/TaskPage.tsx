
import React, { FC, useEffect, useState } from 'react';
import { Task } from '../interface/interface';
import * as api from '../api/api';
import { v4 as uuidv4 } from 'uuid';


const TaskPage: React.FC = () => {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [editingId, setEditingId] = useState<string | null>(null);
  const [status, setStatus] = useState<boolean>(false);
  const [dueDate, setDueDate] = useState('');

  useEffect(() => {
    loadTasks();
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!title.trim()) return;

    if (editingId) { // Edit task
      setTasks(prev =>
        prev.map(task => task.id === editingId ? { ...task, title, description } : task)
      );

      setEditingId(null);
    } else {
      // Create new task
      const dataToSubmit = JSON.stringify({ title, description, status, dueDate })
      
      try {
        let task = await api.createTask(dataToSubmit);
        console.log('taskId: ' + task.id);
        setTasks([...tasks, { id: task.id, title, description, status, dueDate }]);
        
      } catch (error) {
        console.error("Failed to load tasks:", error);

        // For testing purposes only (if the server side fails)
        setTasks([...tasks, { id: uuidv4(), title, description, status, dueDate }]);
      }
    }

    setTitle('');
    setDescription('');
    setStatus(false);
    setDueDate('');
  };

  const loadTasks = async () => {
    try {
      const tasks = await api.getAllTasks();
      setTasks(tasks);
    } catch (error) {
      console.error("Failed to load tasks:", error);
    }
  };

  const handleEdit = async (task: Task) => {

    try {
      const updatedTask = await api.updateTask(task.id, JSON.stringify(task));
      console.log(updatedTask);
    } catch (error) {
      console.error("Failed to update task:", error);
    }

    setEditingId(task.id);
    setTitle(task.title);
    setDescription(task.description);
    setStatus(task.status);
    setDueDate(task.dueDate);
  };

  const handleDelete = async (id: string) => {

    try {
      const status = await api.deleteTask(id);
      console.log(status);
    } catch (error) {
      console.error("Failed to delete task:", error);
    }
    
    setTasks(prev => prev.filter(task => task.id !== id));
  };

  return (
    <div className="p-6 max-w-6xl mx-auto">
      <h1 className="text-2xl font-bold mb-4">Task Manager</h1>

      <form onSubmit={handleSubmit} className="mb-6 space-y-4 grid grid-cols-1 md:grid-cols-2 gap-4">
        <input
          type="text"
          className="border p-2 rounded"
          placeholder="Task title"
          value={title}
          onChange={e => setTitle(e.target.value)}
        />
        <input
          type="date"
          className="border p-2 rounded"
          value={dueDate}
          onChange={e => setDueDate(e.target.value)}
        />
        <textarea
          className="border p-2 rounded col-span-full"
          placeholder="Description"
          value={description}
          onChange={e => setDescription(e.target.value)}
        />

        <label>Completed?:</label>
        <input
          type="checkbox"
          className="border p-2 rounded"
          checked={status}
          onChange={e => setStatus(e.target.checked)}
        />

        <button type="submit" className="bg-blue-600 text-white px-4 py-2 rounded col-span-full w-full md:w-auto">
          {editingId ? 'Update Task' : 'Add Task'}
        </button>
      </form>

      {tasks.length > 0 ? (
        <table className="w-full table-auto border-collapse">
          <thead>
            <tr className="bg-gray-200 text-left">
              <th className="p-2 border">Title</th>
              <th className="p-2 border">Description</th>
              <th className="p-2 border">Due Date</th>
              <th className="p-2 border">Status</th>
              <th className="p-2 border">Priority</th>
              <th className="p-2 border">Actions</th>
            </tr>
          </thead>
          <tbody>
            {tasks.map(task => (
              <tr key={task.id} className="border-t">
                <td className="p-2 border">{task.title}</td>
                <td className="p-2 border">{task.description}</td>
                <td className="p-2 border">{task.dueDate}</td>
                <td className="p-2 border">{task.status ? 'Completed' : 'Not Completed'}</td>
                <td className="p-2 border space-x-2">
                  <button onClick={() => handleEdit(task)} className="text-blue-600">Edit</button>
                  <button onClick={() => handleDelete(task.id)} className="text-red-600">Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      ) : (
        <p className="text-gray-500">No tasks added yet.</p>
      )}
    </div>
  );
};

export default TaskPage;