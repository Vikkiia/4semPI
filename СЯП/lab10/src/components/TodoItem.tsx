import React from 'react';
import { useAppDispatch } from '../redux/hooks';
import { toggleTodo, deleteTodo } from '../redux/todoSlice';
import { Todo } from '../redux/types';

interface Props {
    todo: Todo;
    onEdit: (todo: Todo) => void;
}

export const TodoItem: React.FC<Props> = ({ todo, onEdit }) => {
    const dispatch = useAppDispatch();

    return (
        <li className="flex items-center gap-2">
            <input type="checkbox" checked={todo.completed} onChange={() => dispatch(toggleTodo(todo.id))} />
            <span className={todo.completed ? 'line-through' : ''}>{todo.text}</span>
            <button onClick={() => onEdit(todo)}>✏️</button>
            <button onClick={() => dispatch(deleteTodo(todo.id))}>🗑️</button>
        </li>
    );
};