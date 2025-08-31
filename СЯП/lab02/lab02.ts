


//1
// interface arrayElement {
//   id: number;
//   name: string;
//   group: number;
// }
type id = number;
type name = string;
type group = number;
type arrayElement = {id:id, name:name, group:group};
const array: arrayElement[] = [//Создаётся массив array с элементами типа arrayElement[]:
    {id: 1, name: 'name1', group: 1},
    {id: 2, name: 'name2', group: 12},
    {id: 3, name: 'name3', group: 3},
    {id: 4, name: 'name4', group: 4},
    {id: 5, name: 'name5', group: 5},
];

//2

type CarsType = {
    manufacturer?: string,
    model?: string
}
// interface CarsType {
//     manufacturer?: string;
//     model?: string;
// }
let car: CarsType = {};//Создаёт переменную car типа CarsType
car.manufacturer = "manufacturer";
car.model = 'model';

//3

let car1: CarsType = {};

car1.manufacturer = "manufacturer";
car1.model = 'model';


let car2: CarsType = {};
car2.manufacturer = "manufacturer";
car2.model = 'model';

type ArrayCarsType = {//
    cars: Array<CarsType>;// cars — это массив объектов типа CarsType
}

const arrayCars: Array<ArrayCarsType> = [{
    cars: [car1, car2]
}];


//4

type MarkFilterType = 1|2|3|4|5|6|7|8|9|10;
type PassedType = boolean;  
type GroupFilterType = MarkFilterType | 11 | 12;  

type MarkType = {
    subject: string;
    mark: MarkFilterType; // может принимать значения от 1 до 10
    passed: PassedType;  
};

type StudentType = {
    id: number;
    name: string;
    group: GroupFilterType; // может принимать значения от 1 до 12
    marks: Array<MarkType>;  // Массив оценок по предметам это массив объектов, где каждый объект соответствует MarkType
};

type studentsArray = Array<StudentType>;  

type GroupType = {
    students: studentsArray; // массив студентов типа StudentType

    
    studentsFilter: (group: number) => Array<StudentType>; 

    marksFilter: (mark: number) => Array<StudentType>; 


    deleteStudent: (id: number) => void; 
    mark: MarkFilterType; 
    group: GroupFilterType; 
};

let student1: StudentType = {
    id: 1,
    name: "student 1",
    group: 7,
    marks: [
        { subject: "Java", mark: 10, passed: true },
        { subject: "JS", mark: 10, passed: true },
        { subject: "OOP", mark: 10, passed: true }
    ]
};

let student2: StudentType = {
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
let Group: GroupType = {
    students: new Array<StudentType>(student1, student2),// Массив студентов (student1, student2)

    studentsFilter: (group: number) => {
        return Group.students.filter((stud: StudentType) => stud.group === group);
    },

    marksFilter: (markk: number) => {
        return Group.students.filter((stud: StudentType) =>
            stud.marks.some(mark => mark.mark === markk) 
        );
    },

    deleteStudent: (id: number) => {
        Group.students = [...Group.students.filter(stud => stud.id !== id)];
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
