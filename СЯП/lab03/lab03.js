/// <reference lib="es7" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __spreadArray = (this && this.__spreadArray) || function (to, from, pack) {
    if (pack || arguments.length === 2) for (var i = 0, l = from.length, ar; i < l; i++) {
        if (ar || !(i in from)) {
            if (!ar) ar = Array.prototype.slice.call(from, 0, i);
            ar[i] = from[i];
        }
    }
    return to.concat(ar || Array.prototype.slice.call(from));
};
var BaseUser = /** @class */ (function () {
    function BaseUser(id, name) {
        this.id = id;
        this.name = name;
    }
    BaseUser.prototype.getRole = function () {
        return this.constructor.name;
    };
    return BaseUser;
}());
var Guest = /** @class */ (function (_super) {
    __extends(Guest, _super);
    function Guest() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    Guest.prototype.getPermissions = function () {
        return ["Просмотр контента"];
    };
    return Guest;
}(BaseUser));
var User = /** @class */ (function (_super) {
    __extends(User, _super);
    function User() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    User.prototype.getPermissions = function () {
        return ["Просмотр контента", "Добавление комментариев"];
    };
    return User;
}(BaseUser));
var Admin = /** @class */ (function (_super) {
    __extends(Admin, _super);
    function Admin() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    Admin.prototype.getPermissions = function () {
        return [
            "Просмотр контента",
            "Добавление комментариев",
            "Удаление комментариев",
            "Управление пользователями",
        ];
    };
    return Admin;
}(BaseUser));
var guest = new Guest(1, "Аноним");
console.log(guest.getPermissions()); // ["Просмотр контента"]
console.log(guest.getRole()); // "Guest"
var user = new User(2, "Иван");
console.log(user.getPermissions()); // ["Просмотр контента", "Добавление комментариев"]
console.log(user.getRole()); // "User"
var admin = new Admin(3, "Мария");
console.log(admin.getPermissions());
// ["Просмотр контента", "Добавление комментариев", "Удаление комментариев", "Управление пользователями"]
console.log(admin.getRole()); // "Admin"
var Reeport = /** @class */ (function () {
    function Reeport(title, content) {
        this.title = title;
        this.content = content;
    }
    return Reeport;
}());
var HTMLReport = /** @class */ (function (_super) {
    __extends(HTMLReport, _super);
    function HTMLReport(title, content) {
        return _super.call(this, title, content) || this;
    }
    HTMLReport.prototype.generate = function () {
        return "<h1>".concat(this.title, "</h1><p>").concat(this.content, "</p>");
    };
    return HTMLReport;
}(Reeport));
var JSONReport = /** @class */ (function (_super) {
    __extends(JSONReport, _super);
    function JSONReport(title, content) {
        return _super.call(this, title, content) || this;
    }
    JSONReport.prototype.generate = function () {
        return {
            title: this.title,
            content: this.content
        };
    };
    return JSONReport;
}(Reeport));
var report1 = new HTMLReport("Отчет 1", "Содержание отчета");
console.log(report1.generate());
// "<h1>Отчет 1</h1><p>Содержание отчета</p>"
var report2 = new JSONReport("Отчет 2", "Содержание отчета");
console.log(report2.generate());
// { title: "Отчет 2", content: "Содержание отчета" }
//3
var Caache = /** @class */ (function () {
    function Caache() {
        this.storage = new Map();
    }
    Caache.prototype.add = function (key, value, ttl) {
        var _this = this;
        var expiresAt = Date.now() + ttl; //– вычисляет время истечения записи.
        this.storage.set(key, { value: value, expiresAt: expiresAt });
        setTimeout(function () { return _this.clearExpired(); }, ttl + 100);
    };
    Caache.prototype.get = function (key) {
        var item = this.storage.get(key);
        if (!item)
            return null;
        if (Date.now() > item.expiresAt) {
            this.storage.delete(key);
            return null;
        }
        return item.value;
    };
    Caache.prototype.clearExpired = function () {
        var _this = this;
        var now = Date.now();
        this.storage.forEach(function (item, key) {
            if (item.expiresAt <= now) {
                _this.storage.delete(key);
            }
        });
    };
    return Caache;
}());
var cachee = new Caache();
cachee.add("price", 100, 5000); // Добавляем "price" со сроком 5 секунд
console.log(cachee.get("price")); // Выведет: 100
setTimeout(function () { return console.log(cachee.get("price")); }, 6000); // Выведет: null (данные устарели)
//4
var Product = /** @class */ (function () {
    function Product(name, price) {
        this.name = name;
        this.price = price;
    }
    return Product;
}());
function createInstance(cls) {
    var args = [];
    for (var _i = 1; _i < arguments.length; _i++) {
        args[_i - 1] = arguments[_i];
    }
    return new (cls.bind.apply(cls, __spreadArray([void 0], args, false)))();
}
var p = createInstance(Product, "Телефон", 50000);
console.log(p); // Product { name: "Телефон", price: 50000 }
//5
var LogLevel;
(function (LogLevel) {
    LogLevel["INFO"] = "INFO";
    LogLevel["WARNING"] = "WARNING";
    LogLevel["ERROR"] = "ERROR";
})(LogLevel || (LogLevel = {}));
function logEvent(event) {
    var timestamp = event[0], level = event[1], message = event[2];
    console.log("[".concat(timestamp.toISOString(), "] [").concat(level, "]: ").concat(message));
}
logEvent([new Date(), LogLevel.INFO, "Система запущена"]);
// Вывод: [2025-02-13T10:00:00.000Z] [INFO]: Система запущена
//6
var HttpStatus;
(function (HttpStatus) {
    HttpStatus[HttpStatus["OK"] = 200] = "OK";
    HttpStatus[HttpStatus["BAD_REQUEST"] = 400] = "BAD_REQUEST";
    HttpStatus[HttpStatus["UNAUTHORIZED"] = 401] = "UNAUTHORIZED";
    HttpStatus[HttpStatus["NOT_FOUND"] = 404] = "NOT_FOUND";
    HttpStatus[HttpStatus["INTERNAL_SERVER_ERROR"] = 500] = "INTERNAL_SERVER_ERROR";
})(HttpStatus || (HttpStatus = {}));
/*
string? – опциональное сообщение об ошибке. */
function success(data) {
    return [HttpStatus.OK, data];
}
function error(message, status) {
    return [status, null, message];
}
var res1 = success({ user: "Андрей" });
console.log(res1); // [200, { user: "Андрей" }]
var res2 = error("Не найдено", HttpStatus.NOT_FOUND);
console.log(res2); // [404, null, "Не найдено"]
