#include <algorithm>
#include <iostream>
#include <ctime>
#include <iomanip>
#include <cstring>
#include "Levenshtein.h"
#include <Windows.h>

using namespace std;

char* GenerateRandomString(int size)
{
    char* str = (char*)malloc(sizeof(char) * (size + 1));
    for (int i = 0; i < size; i++) {
        str[i] = rand() % 26 + 'a'; 
    }
    str[size] = '\0';
    return str;
}

int main()
{
    SetConsoleCP(1251);
    SetConsoleOutputCP(1251);

    const int threeHundred = 300;
    const int twoHundred = 200;

    char* s1 = GenerateRandomString(threeHundred);
    cout << "S1: " << endl;
    for (int i = 0; i < threeHundred; i++) {
        if (i % 50 == 0)// Разбивает строку на строки по 50 символов
        {
            cout << "\n";
        }
        cout << s1[i];
    }
    cout << endl << endl;

    srand(time(NULL) + 1);
    char* s2 = GenerateRandomString(twoHundred);
    cout << "S2: " << endl;
    for (int i = 0; i < twoHundred; i++) {
        if (i % 50 == 0)
        {
            cout << "\n";
        }
        cout << s2[i];
    }
    cout << endl << endl;

    clock_t t1 = 0, t2 = 0, t3 = 0, t4 = 0;
    int lx = strlen(s1);
    int ly = strlen(s2);

    int s1_size[]{ threeHundred / 25, threeHundred / 20, threeHundred / 15, threeHundred / 10, threeHundred / 5, threeHundred / 2, threeHundred };
    int s2_size[]{ twoHundred / 25, twoHundred / 20, twoHundred / 15, twoHundred / 10, twoHundred / 5, twoHundred / 2, twoHundred };

    cout << "\n\n-- расстояние Левенштейна -----";
    cout << "\n\n--длина --- рекурсия -- дин.програм. ---\n";

    for (int i = 0; i < min(sizeof(s1_size) / sizeof(s1_size[0]), sizeof(s2_size) / sizeof(s2_size[0])); i++)
    {
        t1 = clock();
        levenshtein_r(s1_size[i], s1, s2_size[i], s2);// Рекурсивный метод
        t2 = clock();

        t3 = clock();
        levenshtein(s1_size[i], s1, s2_size[i], s2); // Динамическое программирование
        t4 = clock();
        cout << right << setw(2) << s1_size[i] << "/" << setw(2) << s2_size[i]
            << "        " << left << setw(10) << (t2 - t1)
            << "   " << setw(10) << (t4 - t3) << endl;
    }
    system("pause");
    return 0;
}
