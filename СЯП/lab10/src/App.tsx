import React from 'react';
import { TodoList } from './components/TodoList';
import { Provider } from 'react-redux';
import { store } from './redux/store';


const App: React.FC = () => {
    return (
        <Provider store={store}>
            <TodoList />;
        </Provider>
    )
};

export default App;