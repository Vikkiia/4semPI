import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const Register: React.FC = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    password: '',
    confirmPassword: '',
  });
  const [errors, setErrors] = useState({
    name: '',
    email: '',
    password: '',
    confirmPassword: '',
  });
  const [successMessage, setSuccessMessage] = useState('');

  
  const validateForm = () => {
    const newErrors: any = {};
    const { name, email, password, confirmPassword } = formData;

    
    if (!name) {
      newErrors.name = 'Имя обязательно';
    } else if (!/^[a-zA-Zа-яА-ЯёЁ\s]+$/.test(name)) {
      newErrors.name = 'Имя может содержать только буквы и пробелы';
    } else if (name.length < 2) {
      newErrors.name = 'Имя должно содержать хотя бы 2 символа';
    } else if (name.length > 50) {
      newErrors.name = 'Имя не может быть длиннее 50 символов';
    }

   
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    if (!email) {
      newErrors.email = 'Электронная почта обязательна';
    } else if (!emailRegex.test(email)) {
      newErrors.email = 'Неверный формат почты';
    } else if (email.includes(' ')) {
      newErrors.email = 'Электронная почта не может содержать пробелы';
    }


    if (!password) {
      newErrors.password = 'Пароль обязателен';
    } else if (password.length < 8) {
      newErrors.password = 'Пароль должен содержать хотя бы 8 символов';
    } else if (!/[A-Z]/.test(password) || !/[a-z]/.test(password) || !/\d/.test(password)) {
      newErrors.password = 'Пароль должен содержать хотя бы одну заглавную букву, одну строчную букву и одну цифру';
    } else if (password.includes(' ')) {
      newErrors.password = 'Пароль не должен содержать пробелы';
    }

 
    if (!confirmPassword) {
      newErrors.confirmPassword = 'Подтверждение пароля обязательно';
    } else if (confirmPassword !== password) {
      newErrors.confirmPassword = 'Пароли не совпадают';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  
  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    if (validateForm()) {
     
      setSuccessMessage('Регистрация успешна!');
      setTimeout(() => {
        navigate('/sign-in'); 
      }, 2000);
    }
  };

  // Обработчик изменения данных в форме
  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  return (
    <div className="max-w-md mx-auto mt-10 p-6 bg-white shadow-md rounded-lg">
      <h2 className="text-2xl font-bold mb-6">Регистрация</h2>

      {successMessage && <div className="mb-4 text-green-600">{successMessage}</div>}

      <form onSubmit={handleSubmit}>
        <div className="mb-4">
          <label htmlFor="name" className="block text-sm font-medium text-gray-700">
            Имя
          </label>
          <input
            id="name"
            name="name"
            type="text"
            value={formData.name}
            onChange={handleInputChange}
            className="mt-1 block w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
          {errors.name && <span className="text-red-500 text-xs">{errors.name}</span>}
        </div>

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

        <div className="mb-4">
          <label htmlFor="confirmPassword" className="block text-sm font-medium text-gray-700">
            Подтверждение пароля
          </label>
          <input
            id="confirmPassword"
            name="confirmPassword"
            type="password"
            value={formData.confirmPassword}
            onChange={handleInputChange}
            className="mt-1 block w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
          {errors.confirmPassword && <span className="text-red-500 text-xs">{errors.confirmPassword}</span>}
        </div>

        <button type="submit" className="w-full py-2 bg-blue-500 text-white rounded-lg">
          Зарегистрироваться
        </button>
      </form>

      <div className="mt-4 text-center">
        <button
          onClick={() => navigate('/sign-in')}
          className="text-sm text-blue-500 hover:underline"
        >
          Уже зарегистрированы? Войти
        </button>
      </div>
    </div>
  );
};

export default Register;
