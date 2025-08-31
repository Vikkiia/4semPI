import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const Login: React.FC = () => {
  const [formData, setFormData] = useState({
    email: '',
    password: '',
  });
  const [errors, setErrors] = useState({
    email: '',
    password: '',
  });
  const [successMessage, setSuccessMessage] = useState('');
  const navigate = useNavigate();

  
  const validateForm = () => {
    const newErrors: any = {};
    const { email, password } = formData; 

    
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    if (!email) {
      newErrors.email = 'Электронная почта обязательна'; 
    } else if (!emailRegex.test(email)) {
      newErrors.email = 'Неверный формат почты';
    }

    
    if (!password) {
      newErrors.password = 'Пароль обязателен';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;  
  };

  
  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    if (validateForm()) {
      
      if (formData.email === 'test@test.com' && formData.password === 'test') {
        setSuccessMessage('Авторизация успешна!');
        setTimeout(() => {
          navigate('/'); 
        }, 2000);
      } else {
        setErrors({ email: 'Неверный логин или пароль', password: '' });
        setSuccessMessage('');
      }
    }
  };


  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
   
    const { name, value } = e.target;
  
    setFormData({ ...formData, [name]: value });
    
  };

  return ( 
    <div className="max-w-md mx-auto mt-10 p-6 bg-white shadow-md rounded-lg">
      <h2 className="text-2xl font-bold mb-6">Авторизация</h2>

    
      {successMessage && <div className="mb-4 text-green-600">{successMessage}</div>}

      <form onSubmit={handleSubmit}>
        
        <div className="mb-4">
       
          <label htmlFor="email" className="block text-sm font-medium text-gray-700">
            Электронная почта
          </label>
          <input
            id="email"
            name="email"
            type="email"
            value={formData.email}
            onChange={handleInputChange}
            className="mt-1 block w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />  
          {errors.email && <span className="text-red-500 text-xs">{errors.email}</span>}
        </div>

        <div className="mb-4">
         
          <label htmlFor="password" className="block text-sm font-medium text-gray-700">
            Пароль
          </label>
         
          <input
            id="password"
            name="password"
            type="password"
            value={formData.password}
            onChange={handleInputChange}
            className="mt-1 block w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
          {errors.password && <span className="text-red-500 text-xs">{errors.password}</span>}
        </div>
{/* Кнопка для отправки формы */}
        <button type="submit" className="w-full py-2 bg-blue-500 text-white rounded-lg">
          Войти
        </button>
      </form>
 {/* Ссылка на страницу регистрации */}
      <div className="mt-4 text-center">
        <button
          onClick={() => navigate('/sign-up')}
          className="text-sm text-blue-500 hover:underline"
        >
          Нет аккаунта? Зарегистрироваться
        </button>
      </div>
   {/* Ссылка на страницу восстановления пароля */}
      <div className="mt-4 text-center">
        <button
          onClick={() => navigate('/reset-password')}// При клике перенаправляет на страницу восстановления пароля
          className="text-sm text-blue-500 hover:underline"
        >
          Забыли пароль?
        </button>
      </div>
    </div>
  );
};

export default Login;
