import React from "react";
import Calculator from "./components/Calculator.tsx";
import ThemeSwitcher from "./components/ThemeSwitcher.tsx";

const App: React.FC = () => {
  return (
    <div className="min-h-screen bg-gray-100 dark:bg-gray-900">
      <header className="p-4 text-center bg-blue-600 text-white">
        <h1 className="text-3xl font-bold">Calculator App</h1>
      </header>
      <main className="flex flex-col items-center justify-center p-4">
        <ThemeSwitcher />
        <Calculator />
      </main>
    </div>
  );
};

export default App;
