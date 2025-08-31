export interface Todo {
    id: number;
    text: string;
    completed: boolean;
}

export interface AddTodoAction {
    type: 'ADD_TODO';
    payload: string;//текст задачи
}

export interface ToggleTodoAction {//переключить состояние выполнения
    type: 'TOGGLE_TODO';
    payload: number;
}

export interface EditTodoAction {
    type: 'EDIT_TODO';
    payload: { id: number; text: string };
}

export interface DeleteTodoAction {
    type: 'DELETE_TODO';
    payload: number;
}

export type TodoAction =
    | AddTodoAction
    | ToggleTodoAction
    | EditTodoAction
    | DeleteTodoAction;


