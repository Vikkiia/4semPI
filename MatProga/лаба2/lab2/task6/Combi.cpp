#include "Combi.h"
namespace combi
{
    accomodation::accomodation(short n, short m)
    {
        this->n = n;
        this->m = m;
        this->cgen = new sochetan::xcombination(n, m);
        this->pgen = new perestan::permutation(m);
        this->sset = new short[m];//массив для хранения
        this->reset();
    }
    void  accomodation::reset()
    {
        this->na = 0;
        this->cgen->reset();
        this->pgen->reset();
        this->cgen->getfirst();
    };
    short accomodation::getfirst()///x
    {
        short rc = (this->n >= this->m) ? this->m : -1;//Если n < m, то размещений быть не может, возвращаем -1 (
        if (rc > 0)
        {
            for (int i = 0; i <= this->m; i++)
                this->sset[i] = this->cgen->sset[this->pgen->ntx(i)];
        };
        return rc;
    };
    short accomodation::getnext()
    {
        short rc;
        this->na++;
        if ((this->pgen->getnext()) > 0) rc = this->getfirst();// Новая перестановка
        else  if ((rc = this->cgen->getnext()) > 0)
        {
            this->pgen->reset();  rc = this->getfirst();
        };
        return rc;
    };
    short accomodation::ntx(short i)
    {
        return this->sset[i]; // Возвращаем `i`-й элемент текущего размещения
    };

    unsigned __int64 fact(unsigned __int64 x) { return (x == 0) ? 1 : (x * fact(x - 1)); };

    unsigned __int64  accomodation::count() const
    {

        return (this->n >= this->m) ?
            fact(this->n) / fact(this->n - this->m) : 0;
    };
}