/// <reference lib="es7" />

abstract class BaseUser {
    constructor(protected id: number, protected name: string) { }

    abstract getPermissions(): string[];

    getRole(): string {
        return (this.constructor as any).name;
    }
}

class Guest extends BaseUser {
    getPermissions(): string[] {
        return ["Просмотр контента"];
    }
}

class User extends BaseUser {
    getPermissions(): string[] {
        return ["Просмотр контента", "Добавление комментариев"];
    }
}


class Admin extends BaseUser {
    getPermissions(): string[] {
        return [
            "Просмотр контента",
            "Добавление комментариев",
            "Удаление комментариев",
            "Управление пользователями",
        ];
    }
}


const guest = new Guest(1, "Аноним");
console.log(guest.getPermissions()); // ["Просмотр контента"]
console.log(guest.getRole()); // "Guest"

const user = new User(2, "Иван");
console.log(user.getPermissions()); // ["Просмотр контента", "Добавление комментариев"]
console.log(user.getRole()); // "User"

const admin = new Admin(3, "Мария");
console.log(admin.getPermissions());
// ["Просмотр контента", "Добавление комментариев", "Удаление комментариев", "Управление пользователями"]
console.log(admin.getRole()); // "Admin"

//2

interface IReport {
    generate(): string | object;
}


abstract class Reeport implements IReport {
    constructor(protected title: string, protected content: string) { }

    abstract generate(): string | object;
}

class HTMLReport extends Reeport {
    constructor(title: string, content: string) {
        super(title, content);
    }

    generate(): string {
        return `<h1>${this.title}</h1><p>${this.content}</p>`;
    }
}

class JSONReport extends Reeport {
    constructor(title: string, content: string) {
        super(title, content);
    }

    generate(): object {
        return {
            title: this.title,
            content: this.content
        };
    }
}

const report1 = new HTMLReport("Отчет 1", "Содержание отчета");
console.log(report1.generate());
// "<h1>Отчет 1</h1><p>Содержание отчета</p>"

const report2 = new JSONReport("Отчет 2", "Содержание отчета");
console.log(report2.generate());
// { title: "Отчет 2", content: "Содержание отчета" }


//3

class Caache<T> {
    private storage: Map<string, { value: T; expiresAt: number }>;

    constructor() {
        this.storage = new Map();
    }
    //добавляет элемент с временем жизни ttl (в мс).
    add(key: string, value: T, ttl: number): void {
        const expiresAt = Date.now() + ttl;//– вычисляет время истечения записи.
        this.storage.set(key, { value, expiresAt });

        setTimeout(() => this.clearExpired(), ttl + 100);

    }


    get(key: string): T | null {
        const item = this.storage.get(key);
        if (!item) return null;

        if (Date.now() > item.expiresAt) {
            this.storage.delete(key);
            return null;
        }
        return item.value;
    }


    clearExpired(): void {
        const now = Date.now();
        this.storage.forEach((item, key) => {
            if (item.expiresAt <= now) {
                this.storage.delete(key);
            }
        });
    }
}


const cachee = new Caache<number>();

cachee.add("price", 100, 5000); // Добавляем "price" со сроком 5 секунд

console.log(cachee.get("price")); // Выведет: 100

setTimeout(() => console.log(cachee.get("price")), 6000); // Выведет: null (данные устарели)

//4


class Product {
    constructor(public name: string, public price: number) { }
}


function createInstance<T>(cls: new (...args: any[]) => T, ...args: any[]): T {
    return new cls(...args);
}


const p = createInstance(Product, "Телефон", 50000);
console.log(p); // Product { name: "Телефон", price: 50000 }

//5

enum LogLevel {//Перечисление (enum)
    INFO = "INFO",
    WARNING = "WARNING",
    ERROR = "ERROR"
}


type LogEntry = [Date, LogLevel, string];


function logEvent(event: LogEntry): void {
    const [timestamp, level, message] = event;
    console.log(`[${timestamp.toISOString()}] [${level}]: ${message}`);
}


logEvent([new Date(), LogLevel.INFO, "Система запущена"]);
// Вывод: [2025-02-13T10:00:00.000Z] [INFO]: Система запущена


//6

enum HttpStatus {
    OK = 200,
    BAD_REQUEST = 400,
    UNAUTHORIZED = 401,
    NOT_FOUND = 404,
    INTERNAL_SERVER_ERROR = 500
}


type ApiResponse<T> = [HttpStatus, T | null, string?];
/*
string? – опциональное сообщение об ошибке. */


function success<T>(data: T): ApiResponse<T> {
    return [HttpStatus.OK, data];
}


function error(message: string, status: HttpStatus): ApiResponse<null> {
    return [status, null, message];
}


const res1 = success({ user: "Андрей" });
console.log(res1); // [200, { user: "Андрей" }]

const res2 = error("Не найдено", HttpStatus.NOT_FOUND);
console.log(res2); // [404, null, "Не найдено"]


