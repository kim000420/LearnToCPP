#include <stdio.h>
#include <vector>

using namespace std;

struct SNode
{
	char cData;
	vector<SNode*> pListAdj;
};


