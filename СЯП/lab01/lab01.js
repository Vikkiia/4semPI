/*1.	Напишите функцию, которая принимает массив из 10 целых чисел (от 0 до 9) и возвращает строку этих чисел в виде номера
телефона.  Формат номера телефона должен соответствовать "(xxx) xxx-xxxx".

Пример:
 createPhoneNumber([1, 2, 3, 4, 5, 6, 7, 8, 9, 0])
// => returns "(123) 456-7890"
 */
function createPhoneNumber(numbers) {
    if (numbers.length !== 10) {
        throw new Error("Массив должен содержать ровно 10 чисел.");
    }
    var part1 = numbers.slice(0, 3).join("");
    var part2 = numbers.slice(3, 6).join("");
    var part3 = numbers.slice(6).join("");
    return "(".concat(part1, ") ").concat(part2, "-").concat(part3);
}
var phone = createPhoneNumber([1, 2, 3, 4, 5, 6, 7, 8, 9, 0]);
console.log(phone); // "(123) 456-7890"
/*2.	Если перечислить все натуральные числа до 10, кратные 3 или 5, то получим 3, 5, 6 и 9. Сумма этих чисел равна 23.
Завершите метод так, чтобы он возвращал сумму всех чисел, кратных 3 или 5, меньше переданного числа. Кроме того, если число отрицательное, верните 0.

Примечание. Если число кратно и 3, и 5, считайте его только один раз.

 class Challenge {
 static solution(number: number) {
    //...
   }
 }

 */
var Challenge = /** @class */ (function () {
    function Challenge() {
    }
    Challenge.solution = function (number) {
        if (number <= 0) {
            return 0;
        }
        var sum = 0;
        for (var i = 1; i < number; i++) {
            if (i % 3 == 0 || i % 5 == 0) {
                sum += i;
            }
        }
        return sum;
    };
    return Challenge;
}());
/*let res2: number = Challenge.solution(10);
console.log(res2);
 */
console.log(Challenge.solution(10)); //  23 (3 + 5 + 6 + 9)
console.log(Challenge.solution(20)); //  78 (3 + 5 + 6 + 9 + 10 + 12 + 15 + 18)
console.log(Challenge.solution(-5));
console.log(Challenge.solution(0));
/*3.	Дан целочисленный массив nums, поверните массив вправо на k шагов, где k неотрицательно.

 Пример:
 Ввод: nums = [1,2,3,4,5,6,7], k = 3
 Вывод: [5,6,7,1,2,3,4]
 Объяснение:
 повернуть на 1 шаг вправо: [7,1,2,3,4,5,6]
 повернуть на 2 шага вправо: [6,7,1,2,3,4,5]
 повернуть на 3 шага вправо: [5,6,7,1,2,3,4]

 */
function rotateArray(numbers, k) {
    for (var i = 0; i < k; i++) {
        var temp = numbers[numbers.length - 1]; //Берём последний элемент массива.
        numbers.splice(numbers.length - 1, 1); //splice(индекс, количество) удаляет 1 элемент с конца.
        numbers.splice(0, 0, temp); //splice(0, 0, temp) вставляет temp в начало массива.
    }
}
var array = [1, 2, 3, 4, 5, 6, 7];
rotateArray(array, 3);
console.log(array);
/*4. Есть два отсортированных массива nums1 и nums2 размера m и n соответственно, вернуть медиану двух отсортированных массивов.
 Медиана число (два числа) находящееся в середине массива.

 Пример 1:
 Ввод: nums1 = [1,3], nums2 = [2]
 Вывод: 2.00000
 Объяснение:
 объединение массивов = [1,2,3], медиана - 2.

 Пример 2:
 Ввод: nums1 = [1,2], nums2 = [3,4]
 Вывод: 2.50000
 Объяснение: объединение массивов = [1,2,3,4], медиана (2 + 3) / 2 = 2.5.
 */
function task4(arr1, arr2) {
    arr1 = arr1.concat(arr2);
    arr1.sort(function (a, b) { return a - b; });
    return getArrayMedian(arr1);
}
function getArrayMedian(arr) {
    var n = arr.length;
    if (n % 2 === 0) { //Если длина чётная
        return (arr[n / 2] + arr[n / 2 - 1]) / 2;
    }
    else {
        return arr[Math.floor(n / 2)]; //Math.floor(n / 2) округляет вниз.
    }
}
console.log(task4([1, 3], [2])); // 2.00000 
console.log(task4([1, 2], [3, 4])); //  2.50000 
console.log(task4([0, 0], [0, 0])); //  0.00000 
console.log(task4([], [1])); //  1.00000 
console.log(task4([2], [])); // 2.00000 
