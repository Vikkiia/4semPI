import React from 'react';
import { Provider } from 'react-redux';
import { store } from './redux/store';
import Counter from './components/Counter';

const App: React.FC = () => {
    return (
        <Provider store={store}>
            <div className="min-h-screen flex justify-center items-center">
                <Counter />
            </div>
        </Provider>
    );
};

export default App;
