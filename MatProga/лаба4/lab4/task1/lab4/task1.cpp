#include <iostream>
#include <windows.h>
#include <string>
#include <ctime>

using namespace std;

string generate(int countSymb) {
    string str;
    char symb;

    for (int i = 0; i < countSymb; i++) {
        
        symb = (rand() % 2 == 0) ? ('A' + rand() % 26) : ('a' + rand() % 26);


        str += symb;
    }

    return str;
}

int main() {
    SetConsoleCP(1251);
    SetConsoleOutputCP(1251);

    srand(time(NULL)); 

    string s1 = generate(300);
    string s2 = generate(200);

    cout << "S1(300): " << s1 << endl;
    cout << "S2(200): " << s2 << endl;

    return 0;
}
