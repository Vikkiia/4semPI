import React, { useState, useEffect } from "react";
import Display from "./Display.tsx"; 
import Button from "./Button.tsx";   


const Calculator: React.FC = () => {
  
  const [input, setInput] = useState<string>(""); 
  const [result, setResult] = useState<string>(""); 
  const [history, setHistory] = useState<string[]>([]); 

  
  const operators = ["+", "-", "*", "/", "."];

  
  const handleClick = (value: string) => {
    
    if (value === "C") {
      setInput("");
      setResult("");
      return;
    }

    
    if (value === "⌫") {
      setInput((prev) => prev.slice(0, -1));
      return;
    }

    
    if (value === "=") {
      try {
        
        // Здесь строка input должна содержать математическое выражение (например, "3+5").
        const evalResult = eval(input);
      
        if (evalResult === Infinity || evalResult === -Infinity) {
          setResult("Ошибка: Деление на ноль");
        } else {
          const stringResult = evalResult.toString();
          setResult(stringResult);
        
          setHistory((prev) => [...prev, `${input} = ${stringResult}`]);
        }
      } catch {
        
        setResult("Ошибка");
      }
      return;
    }

    // Если нажата другая кнопка (число или оператор)
    if (operators.includes(value)) {
     
      if (input === "" && value !== "-") {
        return;
      }
      // Если последний символ в input уже является оператором, предотвращаем ввод двух подряд
      const lastChar = input.slice(-1);
     
      if (operators.includes(lastChar)) {
        return;
      }
    }

    // Добавляем значение кнопки к текущему вводу
    setInput((prev) => prev + value);
  };

  
  const handleKeyDown = (event: KeyboardEvent) => {
    let value = "";

    
    if (event.key === "Enter" || event.key === "=") {
      value = "=";
    }
    
    else if (event.key === "Backspace") {
      value = "⌫";
    }
    
    else if (event.key === "Escape" || event.key.toLowerCase() === "c") {
      value = "C";
    }
    
    else if (/^[0-9+\-*/.]$/.test(event.key)) {
      value = event.key;
    }

    
    if (value) {
      event.preventDefault();
      handleClick(value);
    }
  };

 
  useEffect(() => {
    window.addEventListener("keydown", handleKeyDown);

    return () => {
      window.removeEventListener("keydown", handleKeyDown);
    };
  }, [input]); // Зависимость [input] позволяет обновлять обработчик при изменении ввода

  return (
    
    <div className="max-w-md mx-auto p-5 bg-gray-100 dark:bg-gray-800 rounded-lg shadow-lg mt-5">
      
      {history.length > 0 && (
        <div className="mb-3 p-2 bg-gray-200 dark:bg-gray-700 rounded overflow-y-auto max-h-40">
          {history.map((item, index) => (
            <div key={index} className="text-xs sm:text-sm text-gray-700 dark:text-gray-200">
              {item}
            </div>
          ))}
        </div>
      )}

      
      <Display input={input} result={result} />

   
      <div className="grid grid-cols-4 gap-2 mt-4">
    
        {["7", "8", "9", "⌫"].map((item) => (
          <Button key={item} label={item} onClick={handleClick} />
        ))}
       
        {["4", "5", "6", "/"].map((item) => (
          <Button key={item} label={item} onClick={handleClick} />
        ))}
      
        {["1", "2", "3", "*"].map((item) => (
          <Button key={item} label={item} onClick={handleClick} />
        ))}
      
        {["+", "0", "=", "-"].map((item) => (
          <Button key={item} label={item} onClick={handleClick} />
        ))}
       
        {["C", "."].map((item) => (
          <Button key={item} label={item} className="col-span-2" onClick={handleClick} />
        ))}
      </div>
    </div>
  );
};

export default Calculator;
