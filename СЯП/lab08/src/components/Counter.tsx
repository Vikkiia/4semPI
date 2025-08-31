import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { increment, decrement, reset } from '../redux/actoins';

const Counter: React.FC = () => {
    const dispatch = useDispatch();
    const count = useSelector((state: { count: number }) => state.count);

    return (
        <div className="flex flex-col items-center space-y-4">
            <h1 className="text-2xl font-bold">Счётчик: {count}</h1>
            <div className="flex space-x-4">
                <button
                    onClick={() => dispatch(increment())}
                    className="px-4 py-2 bg-blue-500 text-white rounded"
                >
                    Увеличить (+)
                </button>
                <button
                    onClick={() => dispatch(decrement())}
                    className="px-4 py-2 bg-red-500 text-white rounded"
                >
                    Уменьшить (–)
                </button>
                <button
                    onClick={() => dispatch(reset())}
                    className="px-4 py-2 bg-gray-500 text-white rounded"
                >
                    Сбросить
                </button>
            </div>
        </div>
    );
};

export default Counter;
