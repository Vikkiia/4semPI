import React from "react";
import { ButtonProps } from "./Types";

const Button: React.FC<ButtonProps> = ({ label, onClick, className = "" }) => {
  return (
    <button
      className={`p-4 border rounded-md text-xl ${className}`}
      onClick={() => onClick(label)}
    >
      {label}
    </button>
  );
};

export default Button;
