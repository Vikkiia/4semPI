#include "BFS.h" 


void BFS::init(const graph::AList& al, int s)
{
    // Устанавливаем указатель на переданный список смежности
    this->al = &al;

    
    this->c = new Color[this->al->n_vertex]; // Массив цветов вершин
    this->d = new int[this->al->n_vertex];   // Массив расстояний от начальной вершины
    this->p = new int[this->al->n_vertex];   // Массив предшествующих вершин

    
    for (int i = 0; i < this->al->n_vertex; i++)
    {
        this->c[i] = WHITE; // WHITE - не посещена
        this->d[i] = INF;   // INF - бесконечное расстояние (не достижима)
        this->p[i] = NIL;   // NIL - отсутствие предшествующей вершины
    }

    
    this->c[s] = GRAY; // GRAY - посещена, но не обработана
    this->q.push(s);  
}

// Конструктор для списка смежности
BFS::BFS(const graph::AList& al, int s)
{
    
    this->init(al, s);
}

// Конструктор для матрицы смежности
BFS::BFS(const graph::AMatrix& am, int s)
{
    // Создаем новый список смежности из матрицы и вызываем метод инициализации
    this->init(*(new graph::AList(am)), s);
}


int BFS::get()
{
    int rc = NIL, v = NIL; // Переменные для текущей вершины (rc) и смежной вершины (v)

    if (!this->q.empty()) 
    {
        rc = this->q.front(); // Берем первую вершину из очереди
        for (int j = 0; j < this->al->size(rc); j++) 
        {
            if (this->c[v = this->al->get(rc, j)] == WHITE) 
            {
                this->c[v] = GRAY;          // Помечаем ее как посещенную, но не обработанную
                this->d[v] = this->d[rc] + 1; // Устанавливаем расстояние до нее на 1 больше, чем до текущей
                this->p[v] = rc;             // Устанавливаем текущую вершину rc как предшествующую для v
                this->q.push(v);             // Добавляем v в очередь для дальнейшей обработки
            }
        }

        this->q.pop();       // Удаляем текущую вершину из очереди, так как ее обработка завершена
        this->c[rc] = BLACK; // Помечаем текущую вершину как обработанную (черная)
    }

    return rc; 
}