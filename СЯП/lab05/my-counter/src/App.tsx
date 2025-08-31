import React from 'react';
import Counter from './components/Counter';

const App: React.FC = () => {
    return (
        <div className="min-h-screen flex items-center justify-center bg-gray-800">
            <Counter />
        </div>
    );
};

export default App;
