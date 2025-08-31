import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const ResetPassword: React.FC = () => {
  const [email, setEmail] = useState('');
  const [message, setMessage] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();
 
  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

   
    if (!/\S+@\S+\.\S+/.test(email)) {
      setError('Неверный формат электронной почты');
      return;
    }

    
    setTimeout(() => { 
      setMessage(`Пароль для ${email} успешно восстановлен. Ваш новый пароль: 'testTEST'`);
    }, 1000);
  };

  return (
    <div className="max-w-md mx-auto mt-10 p-6 bg-white shadow-md rounded-lg">
    
      <h2 className="text-2xl font-bold mb-6">Восстановление пароля</h2>

      <form onSubmit={handleSubmit}>
        <div className="mb-4">
            {/* Метка для поля email */}
          <label htmlFor="email" className="block text-sm font-medium text-gray-700">
            Электронная почта
          </label>
          <input
            id="email"
            name="email"
            type="email"
            value={email} // Привязка значения поля email к состоянию email
            onChange={(e) => setEmail(e.target.value)}
            className="mt-1 block w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
          {error && <span className="text-red-500 text-xs">{error}</span>}
        </div>

        <button type="submit" className="w-full py-2 bg-blue-500 text-white rounded-lg">
          Отправить запрос
        </button>
      </form>

      {message && (
        <div className="mt-4 text-green-600">
          {message}
        </div>
      )}

      <div className="mt-4 text-center">
        <button
          onClick={() => navigate('/sign-in')}
          className="text-sm text-blue-500 hover:underline"
        >
          Уже помнишь пароль? Войти
        </button>
      </div>
    </div>
  );
};

export default ResetPassword;
