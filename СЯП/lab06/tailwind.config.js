/** @type {import('tailwindcss').Config} */
module.exports = {
  // Важно: указываем, что dark-режим активируется через класс "dark"
  darkMode: 'class',
  content: [
    './src/**/*.{js,jsx,ts,tsx}',
    // Если у вас есть другие пути, добавьте их
  ],
  theme: {
    extend: {},
  },
  plugins: [],
};


