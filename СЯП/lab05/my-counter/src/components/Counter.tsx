import React, { FC, useState } from 'react';
import Button from './Button';

const Counter: FC = () => {
    const [count, setCount] = useState<number>(0);

    const handleIncrease = (): void => {
        setCount(prevCount => prevCount + 1);
    };

    const handleReset = (): void => {
        setCount(0);
    };

    const isIncreaseDisabled: boolean = count >= 5;
    const isResetDisabled: boolean = count === 0;

    return (
        <div className="bg-gray-700 p-6 rounded shadow text-center">
            <h1 className="text-4xl text-white mb-4">{count}</h1>
            <div className="flex justify-center">
                <Button title="increase" onClick={handleIncrease} disabled={isIncreaseDisabled} />
                <Button title="reset" onClick={handleReset} disabled={isResetDisabled} />
            </div>
        </div>
    );
};

export default Counter;
