#pragma once
#include "Graph.h"
#include <vector>

struct DFS   // depth-first search ����� � ������� 
{
	const static int NIL = -1;
	enum Color { WHITE, GRAY, BLACK };       // 
	const graph::AList* al;                // �������� ����
	Color* c;                              // ���� �������  
	int* d;                              // ����� ����������� 
	int* f;                              // ����� ���������� ���������   
	int* p;                              // �������������� ������� 
	int   t;                               // ������� ����� 
	DFS(const graph::AList& al);
	DFS(const graph::AMatrix& am);
	std::vector <int> topological_sort; //��������� �������������� ����������
	void visit(int v);
	void init(const graph::AList& al);
	int get(int i);                        // �������� ������� 
};