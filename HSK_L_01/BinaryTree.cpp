#include <stdio.h>

struct SNode 
{
	int nData;
	SNode* pLeft;
	SNode* pRight;
	
};

// 최소 힙트리 알고리즘
// 빈공간찾기
// 빈공간에 새 데이터 넣기
// 데이터의 부모 데이터와 비교해서 바꾸기
// 

void main()
{

}

void CreNode(SNode* pStart, int data)
{
	//빈공간 찾기
	// pStart에서 pLeft확인 플레그
	// pLeft가 있다면 pRight확인
	// pRight가 있다면 pLeft로 이동/ 없다면 pRight에 저장
	// pLeft가 없다면 데이터 넣기
	
	SNode* pTemp = NULL;

	pTemp = pStart;
	while (true)
	{
		if (pTemp->pLeft == NULL)
		{
			pTemp->nData = data;
			return;
		}
		else //pTemp의 Left가 있다면
		{
			if (pTemp->pRight == NULL)
			{
				pTemp->nData = data;
				return;
			}
			else //pTemp의 Left와 Right가 있다면
			{
				pTemp = pTemp->pLeft;
			}
		}
	}



}