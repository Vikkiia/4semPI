var __spreadArray = (this && this.__spreadArray) || function (to, from, pack) {
    if (pack || arguments.length === 2) for (var i = 0, l = from.length, ar; i < l; i++) {
        if (ar || !(i in from)) {
            if (!ar) ar = Array.prototype.slice.call(from, 0, i);
            ar[i] = from[i];
        }
    }
    return to.concat(ar || Array.prototype.slice.call(from));
};
var array = [
    { id: 1, name: 'name1', group: 1 },
    { id: 2, name: 'name2', group: 12 },
    { id: 3, name: 'name3', group: 3 },
    { id: 4, name: 'name4', group: 4 },
    { id: 5, name: 'name5', group: 5 },
];
// interface CarsType {
//     manufacturer?: string;
//     model?: string;
// }
var car = {}; //Создаёт переменную car типа CarsType
car.manufacturer = "manufacturer";
car.model = 'model';
//3
var car1 = {};
car1.manufacturer = "manufacturer";
car1.model = 'model';
var car2 = {};
car2.manufacturer = "manufacturer";
car2.model = 'model';
var arrayCars = [{
        cars: [car1, car2]
    }];
var student1 = {
    id: 1,
    name: "student 1",
    group: 7,
    marks: [
        { subject: "Java", mark: 10, passed: true },
        { subject: "JS", mark: 10, passed: true },
        { subject: "OOP", mark: 10, passed: true }
    ]
};
var student2 = {
    id: 2,
    name: "student 2",
    group: 4,
    marks: [
        { subject: "Java", mark: 10, passed: true },
        { subject: "JS", mark: 3, passed: false },
        { subject: "OOP", mark: 6, passed: true }
    ]
};
//  Создаём объект группы студентов
var Group = {
    students: new Array(student1, student2), // Массив студентов (student1, student2)
    studentsFilter: function (group) {
        return Group.students.filter(function (stud) { return stud.group === group; });
    },
    marksFilter: function (markk) {
        return Group.students.filter(function (stud) {
            return stud.marks.some(function (mark) { return mark.mark === markk; });
        });
    },
    deleteStudent: function (id) {
        Group.students = __spreadArray([], Group.students.filter(function (stud) { return stud.id !== id; }), true);
    },
    mark: 5,
    group: 12
};
console.log(Group.studentsFilter(4)); //   (student2)
console.log("----");
console.log(Group.marksFilter(10)); // (student1 и student2)
console.log("-----");
Group.deleteStudent(1);
console.log(Group.students); // (остался только student2)
console.log("----");
console.log(Group.marksFilter(10)); //  (остался только student2)
