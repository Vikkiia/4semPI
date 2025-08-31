// Calc.js

// Определяем функции
function Sum(a, b) {
    return a + b;
}

function Sub(a, b) {
    return a - b;
}

function Mul(a, b) {
    return a * b;
}

function Div(a, b) {
    // проверяем деление на ноль
    if (b === 0) {
        return '∞';
    }
    return a / b;
}

// Когда страница загрузится, заполним <span> результатами
document.addEventListener('DOMContentLoaded', () => {
    // Ищем элементы по ID
    const sumEl = document.getElementById('sum');
    const subEl = document.getElementById('sub');
    const mulEl = document.getElementById('mul');
    const divEl = document.getElementById('div');

    // Вызываем наши функции
    sumEl.textContent = Sum(7, 3);
    subEl.textContent = Sub(7, 3);
    mulEl.textContent = Mul(7, 3);
    divEl.textContent = Div(7, 3).toFixed(2); // до 2 знаков после запятой
});
