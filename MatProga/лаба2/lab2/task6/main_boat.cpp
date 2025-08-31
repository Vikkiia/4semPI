////
//#include <iostream>
//#include <iomanip>
//#include "Boat.h"
//#define NN 25
//#define MM 5
//int main()
//{
//    setlocale(LC_ALL, "rus");
//    int V = 1500,
//        v[] = { 100,  200,   300,  400,  500,  600, 700, 800, 900, 150, 250, 350, 450, 550, 650, 750, 850, 120, 220, 320, 420, 520, 620, 720, 820 },
//        c[NN] = { 10,   15,    20,   25,   30,  35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95, 100, 105, 110, 115, 120, 125, 150 };
//    short  r[MM];
//    int cc = boat(
//        V,   // [in]  Ограничение по весу
//        MM,  // [in]  Количество мест для контейнеров     
//        NN,  // [in]  Число контейнеров  
//        v,   // [in]  Вес каждого контейнера  
//        c,   // [in]  Масса содержимого каждого контейнера    
//        r    // [out] Результат: номера выбранных контейнеров  
//    );
//    std::cout << std::endl << "- Задача о загрузке контейнеров на судне";
//    std::cout << std::endl << "- Число возможных контейнеров    : " << NN;
//    std::cout << std::endl << "- Количество мест для контейнеров: " << MM;
//    std::cout << std::endl << "- Ограничение по весу            : " << V;
//    std::cout << std::endl << "- Вес контейнеров                : ";
//    for (int i = 0; i < NN; i++) std::cout << std::setw(3) << v[i] << " ";
//    std::cout << std::endl << "- Масса содержимого              : ";
//    for (int i = 0; i < NN; i++) std::cout << std::setw(3) << c[i] << " ";
//    std::cout << std::endl << "- Выбранные контейнеры (0,1,...,m-1): ";
//    for (int i = 0; i < MM; i++) std::cout << r[i] << " ";
//    std::cout << std::endl << "- Масса содержимого выбранных контейнеров: " << cc;
//    std::cout << std::endl << "- Общий вес выбранных контейнеров : ";
//    int s = 0; for (int i = 0; i < MM; i++) s += v[r[i]]; std::cout << s;
//    std::cout << std::endl << std::endl;
//    system("pause");
//    return 0;
//}

/////////////////////////////////////////clock/////////////////////////////////////////////////////
#include <iostream>
#include <iomanip>
#include "Boat.h"
#include <time.h>
#define NN (sizeof(v)/sizeof(int))
#define MM 6
#define SPACE(n) std::setw(n)<<" "
int main()
{
    setlocale(LC_ALL, "rus");
    int V = 1000,
        v[] = { 250, 560, 670, 400, 200, 270, 370, 330, 330, 440, 530, 120,
               200, 270, 370, 330, 330, 440, 700, 120, 550, 540, 420, 170,
               600, 700, 120, 550, 540, 420, 430, 140, 300, 370, 310 };
    int c[NN] = { 15,26,  27,  43,  16,  26,  42,  22,  34,  12,  33,  30,
               42,22,  34,  43,  16,  26,  14,  12,  25,  41,  17,  28,
               12,45,  60,  41,  33,  11,  14,  12,  25,  41,  30 };
    short r[MM];
    int maxcc = 0;
    clock_t t1, t2;
    std::cout << std::endl << "-- Задача об оптимальной загрузке судна -- ";
    std::cout << std::endl << "-  ограничение по весу    : " << V;
    std::cout << std::endl << "-  количество мест        : " << MM;
    std::cout << std::endl << "-- количество ------ продолжительность -- ";
    std::cout << std::endl << "   контейнеров        вычисления  ";
    for (int i = 25; i <= NN; i++)
    {
        t1 = clock();
        int maxcc = boat(V, MM, i, v, c, r);
        t2 = clock();
        std::cout << std::endl << SPACE(7) << std::setw(2) << i
            << SPACE(15) << std::setw(5) << (t2 - t1);
    }
    std::cout << std::endl << std::endl;
    system("pause");
    return 0;
}
