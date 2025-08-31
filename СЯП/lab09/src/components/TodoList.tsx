import React, { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { addTodo, editTodo } from '../redux/actions';
import { TodoItem } from './TodoItem';
import { Todo } from '../redux/types';

export const TodoList: React.FC = () => {
    const [input, setInput] = useState('');
    const [editId, setEditId] = useState<number | null>(null);
    const dispatch = useDispatch();
    const todos = useSelector((state: Todo[]) => state);

    const handleAddOrEdit = () => {
        if (!input.trim()) return;

        if (editId !== null) {
            dispatch(editTodo(editId, input));
            setEditId(null);
        } else {
            dispatch(addTodo(input));
        }
        setInput('');
    };

    const handleEdit = (todo: Todo) => {
        setEditId(todo.id);
        setInput(todo.text);
    };

    return (
        <div className="p-4 max-w-md mx-auto">
            <h1 className="text-xl font-bold mb-4">Список дел</h1>
            <div className="flex gap-2 mb-4">
                <input
                    className="border rounded px-2 py-1 flex-grow"
                    value={input}
                    onChange={e => setInput(e.target.value)}
                    placeholder="Новая задача"
                />
                <button onClick={handleAddOrEdit} className="bg-blue-500 text-white px-4 py-1 rounded">
                    {editId !== null ? 'Изменить' : 'Добавить'}
                </button>
            </div>
            <ul className="space-y-2">
                {todos.map(todo => <TodoItem key={todo.id} todo={todo} onEdit={handleEdit} />)}
            </ul>
        </div>
    );
};
