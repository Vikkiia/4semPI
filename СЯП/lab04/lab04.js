/*2.	Создать промис myPromise, который через 3 секунды генерирует случайное число. Результат выполнения промиса 
(сгенерированное число) вывести в консоль. */

let myPromise = new Promise(function(resolve, reject){
    setTimeout(() => resolve(Math.floor(Math.random()*100)), 300)
})

myPromise.then(result => console.log(result))



/*3.	Создать функцию, которая принимает параметр delay и возвращает промис myPromise (промис из предыдущей задачи).
 Сгенерируйте 3 числа (т.е. необходимо вызвать функцию 3 раза) и только после того, как все промисы выполняться успешно,
  вывести числа в консоль. Использовать Promise.all. */

function myFunction(delay){
    return new Promise(function(resolve, reject){
        let randomNumber = Math.floor(Math.random() * 10) + 1;
    setTimeout(() => resolve(randomNumber), delay);


    });
}

Promise.all([myFunction(2000), myFunction(3000), myFunction(4000)])
    .then(values => console.log(values));
    




    /*4.	Что будет выведено в консоль и почему? Что возвращают методы then и catch? */
    let pr = new Promise ((res,rej) =>{
    rej('ku')
            } )
          

pr .then(() => console.log(1))
.catch(() => console.log(2))
.catch(() => console.log(3))
.then(() => console.log(4))
.then(() => console.log(5))


/*5.	    Создайте промис, который выполнился успешно, результат выполнения промиса число 21. Вызовите цепочку методов then.
 Первый вызов метода then выводит в консоль результат выполнения предыдущего промиса. Второй вызов метода then выводит в консоль 
 результат выполнения предыдущего промиса умноженного на 2. В результате в консоль последовательно должны выводиться числа 21 и 42. */
 let prom = new Promise(function(res,rej){
    setTimeout(() => res(21),4000)
   
})

prom.then(function(ress){
    console.log(ress)
    return ress
})
.then(function(ress){
    console.log(ress *2)
})

/*6.	Предыдущую задачу реализуйте при помои синтаксиса async/await */


setTimeout(async() =>{
    let ress = await prom
    /*Ждёт, пока prom завершится (fulfilled).
Присваивает результат промиса переменной ress. */
    console.log(ress)
    console.log(ress * 2)
})

//7

// let promise = new Promise((res, rej) => {
//     res('Resolved promise - 1')
// })
// promise
// .then((res) => {
//     console.log("Resolved promise -2")
//     return "OK"
// })
// .then((res) =>{
//     console.log(res)// OK     -2
// })



// //8
// let promise = new Promise((res, rej) =>{
//     res('Resolved promise - 1')
// })

// promise
// .then((res) =>{
//     console.log(res)
//     return "OK"
// })
// .then((res1) =>{
//     console.log(res1)// - 1  OK
// })



// //9
// let promise = new Promise((res, rej) =>{
//     res('Resolved promise - 1')
// })

// promise
// .then((res) =>{
//     console.log(res)
//     return res
// })
// .then((res1) =>{
//     console.log('Reslved promise - 2')// - 1 - 2
// })



// //10
// let promise = new Promise((res,rej) =>{
//     res('Resolved promise - 1')
// })

// promise
// .then((res) =>{//Получает res = 'Resolved promise - 1' (
//     console.log(res)
//     return res
// })
// .then((res1) =>{
//     console.log(res1)//-1 -1
// })



// //11
// let promise = new Promise((res, rej) =>{
//     res('Resolved promise - 1')
// })

// promise
// .then((res) =>{
//     console.log(res)
// })
// .then((res1) =>{
//     console.log(res1)//-1 undefind
// })

// //12
// let pr = new Promise((res, rej) =>{
//     rej('ku')
// })
// pr
// .then(() => console.log(1))
// .catch(() => console.log(2))//2 4 5
// .catch(() => console.log(3))
// .then(() => console.log(4))
// .then(() => console.log(5))