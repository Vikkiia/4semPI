import React from 'react';
import { useDispatch } from 'react-redux';
import { toggleTodo, deleteTodo } from '../redux/actions';
import { Todo } from '../redux/types';

interface Props {
    todo: Todo;
    onEdit: (todo: Todo) => void;
}

export const TodoItem: React.FC<Props> = ({ todo, onEdit }) => {
    const dispatch = useDispatch();

    return (
        <li className="flex items-center gap-2">
            <input type="checkbox" checked={todo.completed} onChange={() => dispatch(toggleTodo(todo.id))} />
            <span className={todo.completed ? 'line-through' : ''}>{todo.text}</span>
            <button onClick={() => onEdit(todo)}>✏️</button>
            <button onClick={() => dispatch(deleteTodo(todo.id))}>🗑️</button>
        </li>
    );
};
