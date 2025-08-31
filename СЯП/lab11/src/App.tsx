import React from 'react';
import { Posts } from './features/posts/posts';
import { Provider } from 'react-redux';
import { store } from './app/store';

const App: React.FC = () => {
    return (
        <Provider store={store}>
            <Posts />;
        </Provider>
    )
};

export default App;