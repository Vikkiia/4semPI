import React, { FC } from 'react';

interface ButtonProps {
    title: string;
    onClick: () => void;
    disabled?: boolean;
}

const Button: FC<ButtonProps> = ({ title, onClick, disabled }) => {
    return (
        <button
            className="bg-blue-500 text-white font-bold py-2 px-4 rounded mx-2 hover:bg-blue-600 disabled:bg-gray-500"
            onClick={onClick}
            disabled={disabled}
        >
            {title}
        </button>
    );
};

export default Button;