import React from "react";

interface HistoryProps {
  history: string[];
}

const History: React.FC<HistoryProps> = ({ history }) => {
  return (
    <div className="mt-4 p-4 bg-gray-200 dark:bg-gray-700 rounded-lg">
      <h3 className="text-lg font-bold">История</h3>
      <ul className="text-gray-600 dark:text-gray-300">
        {history.map((item, index) => (
          <li key={index}>{item}</li>
        ))}
      </ul>
    </div>
  );
};

export default History;
