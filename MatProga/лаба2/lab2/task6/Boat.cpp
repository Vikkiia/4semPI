#include "Boat.h"
#include "Perestanovka.h"
#include "Sochetania.h"
namespace boatfnc
{
    int calcv(sochetan::xcombination s, const int v[])  //считает суммарный вес текущего набора контейнеров
    {
        int rc = 0;
        for (int i = 0; i < s.m; i++) rc += v[s.ntx(i)];
        return rc;
    };

    int calcc(sochetan::xcombination s, const int c[]) //читает суммарный доход.
    {
        int rc = 0;
        for (int i = 0; i < s.m; i++) rc += c[s.ntx(i)];
        return rc;
    };

    void   copycomb(short m, short* r1, const short* r2)    // копирует найденное лучшее сочетание контейнеров.
    {
        for (int i = 0; i < m; i++)  r1[i] = r2[i];
    };

}
int  boat(
    int V,         
    short m,         
    short n,       
    const int v[], 
    const int c[],      
    short r[]      
)
{
    sochetan::xcombination xc(n, m);//генератор
    int rc = 0, i = xc.getfirst(), cc = 0;//rc максим дохож cc текущий доход
    while (i > 0)
    { // Если доход больше текущего максимального, обновляем лучший вариант
        if (boatfnc::calcv(xc, v) <= V)
            if ((cc = boatfnc::calcc(xc, c)) > rc)
            {
                rc = cc; boatfnc::copycomb(m, r, xc.sset);
            }
        i = xc.getnext();
    };
    return rc;
};