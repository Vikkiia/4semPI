import React from "react";

interface DisplayProps {
  input: string;
  result: string;
}

const Display: React.FC<DisplayProps> = ({ input, result }) => {
  return (
    <div className="p-4 bg-gray-200 dark:bg-gray-700 text-right text-2xl">
      <div className="text-gray-500">{input}</div>
      <div className="font-bold">{result}</div>
    </div>
  );
};

export default Display;
