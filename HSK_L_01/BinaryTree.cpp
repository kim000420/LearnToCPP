/*##################################
이진트리(C언어 수업용)
파일명: BinaryTree.cpp
작성자 : 김홍규(downkhg@gmail.com)
마지막수정날짜 : 2022.03.04
버전 : 1.01
###################################*/
#include <stdio.h>
#include <queue>
#include <string>
using namespace std;

struct SNode {
	int nData;
	SNode* pLeft;
	SNode* pRight;
};

void PrintLevel(const SNode* root, const char* title);
bool CheckMinHeap(const SNode* root, const char* tag, bool verbose = true);
bool IsCompleteBinaryTree(const SNode* root, bool verbose = true);
int  CountNodes(const SNode* root);

SNode* CreateNode(int data)
{
	SNode* pTemp = new SNode;
	pTemp->nData = data;
	pTemp->pLeft = NULL;
	pTemp->pRight = NULL;
	return pTemp;
};
bool MakeLeft(SNode* pParent, SNode* pChilde)
{
	if (pParent == NULL)
		return false;
	pParent->pLeft = pChilde;
	return true;
};
bool MakeRight(SNode* pParent, SNode* pChilde)
{
	if (pParent == NULL)
		return false;
	pParent->pRight = pChilde;
	return true;
};

void Traverse(SNode* pNode)
{
	if (!pNode) return;
	//printf("%d\n", pNode->nData); //전위
	Traverse(pNode->pLeft);
	//printf("%d\n", pNode->nData); //중위
	Traverse(pNode->pRight);
	printf("%d\n", pNode->nData); //후위
}

// 레벨별로 출력 (검증용 덤프)
void PrintLevel(const SNode* root, const char* title)
{
	if (title) printf("\n[%s]\n", title);
	if (!root) { printf("(empty)\n"); return; }

	queue<const SNode*> q;
	q.push(root);
	while (!q.empty()) {
		int sz = (int)q.size();
		bool anyChild = false;
		while (sz--) {
			const SNode* cur = q.front(); q.pop();
			if (cur) {
				printf("%d ", cur->nData);
				q.push(cur->pLeft);
				q.push(cur->pRight);
				if (cur->pLeft || cur->pRight) anyChild = true;
			}
			else {
				printf(". ");
				q.push(nullptr);
				q.push(nullptr);
			}
		}
		printf("\n");
		if (!anyChild) break; // 더 내려가도 전부 nullptr
	}
}

// Min-Heap(부모 <= 자식) 속성 검사
static bool checkMinHeapRec(const SNode* node, const char* tag, bool verbose)
{
	if (!node) return true;

	bool ok = true;
	if (node->pLeft) {
		if (node->nData > node->pLeft->nData) {
			ok = false;
			if (verbose)
				printf("[HEAP-FAIL:%s] parent(%d) > left(%d)\n",
					tag, node->nData, node->pLeft->nData);
		}
	}
	if (node->pRight) {
		if (node->nData > node->pRight->nData) {
			ok = false;
			if (verbose)
				printf("[HEAP-FAIL:%s] parent(%d) > right(%d)\n",
					tag, node->nData, node->pRight->nData);
		}
	}
	return ok & checkMinHeapRec(node->pLeft, tag, verbose)
		& checkMinHeapRec(node->pRight, tag, verbose);
}

bool CheckMinHeap(const SNode* root, const char* tag, bool verbose)
{
	bool ok = checkMinHeapRec(root, tag, verbose);
	if (verbose)
		printf("[MinHeap %s] %s\n", tag, ok ? "OK" : "NG");
	return ok;
}

// 완전 이진트리(complete) 여부 검사 (힙의 형태 요건)
// BFS로 null을 처음 본 이후엔 이후 노드는 모두 null이어야 함
bool IsCompleteBinaryTree(const SNode* root, bool verbose)
{
	if (!root) { if (verbose) printf("[Complete] empty OK\n"); return true; }

	queue<const SNode*> q;
	q.push(root);
	bool seenNull = false;
	int level = 0;

	while (!q.empty()) {
		int sz = (int)q.size();
		while (sz--) {
			const SNode* cur = q.front(); q.pop();

			if (!cur) {
				seenNull = true;
				continue;
			}
			if (seenNull) {
				if (verbose)
					printf("[COMPLETE-FAIL] null after non-null gap at level=%d (node=%d)\n",
						level, cur->nData);
				return false;
			}
			q.push(cur->pLeft);
			q.push(cur->pRight);
		}
		level++;
	}
	if (verbose) printf("[Complete] OK\n");
	return true;
}

int CountNodes(const SNode* root)
{
	if (!root) return 0;
	return 1 + CountNodes(root->pLeft) + CountNodes(root->pRight);
}


void ChangeNode(SNode* pNode)
{
	SNode* pTemp = pNode;
	if (pNode->pLeft == NULL) return;

	if (pNode->pLeft->nData < pNode->nData)
	{
		printf("[Change Parent: %d <-> Childe: %d]\n", pNode->nData, pNode->pLeft->nData);

		int data = pNode->nData;
		pNode->nData = pNode->pLeft->nData;
		pNode->pLeft->nData = data;
	}
	else if (pNode->pRight->nData < pNode->nData)
	{
		printf("[Change Parent: %d <-> Childe: %d]\n", pNode->nData, pNode->pRight->nData);

		int data = pNode->nData;
		pNode->nData = pNode->pRight->nData;
		pNode->pRight->nData = data;
	}
	else return;
}

void SortNode(SNode* pNode)
{
	if (!pNode) return;
	//printf("%d\n", pNode->nData); //전위
	SortNode(pNode->pLeft);
	ChangeNode(pNode);
	//printf("%d\n", pNode->nData); //중위
	SortNode(pNode->pRight);
	ChangeNode(pNode);
}

void Print(SNode* pSeed)
{
	Traverse(pSeed);
}

int main()
{
	SNode* pSeed = NULL;

	SNode* pParent = CreateNode(10);
	SNode* pLeft = CreateNode(20);
	SNode* pRight = CreateNode(30);
	SNode* pD = CreateNode(40);
	SNode* pE = CreateNode(50);

	SNode* pF = CreateNode(25);
	SNode* pG = CreateNode(5);

	MakeLeft(pParent, pLeft);
	MakeRight(pParent, pRight);

	MakeLeft(pLeft, pD);
	MakeRight(pLeft, pE);

	MakeLeft(pRight, pF);
	MakeRight(pRight, pG);

	pSeed = pParent;
	
	// 기존 Print(pSeed);
	PrintLevel(pSeed, "Before");

	// 힙 속성/형태 사전 검증
	CheckMinHeap(pSeed, "Before");
	IsCompleteBinaryTree(pSeed);

	// 네가 만든 정렬(힙화) 수행
	SortNode(pSeed);

	// 결과 확인
	PrintLevel(pSeed, "After");
	CheckMinHeap(pSeed, "After");
	IsCompleteBinaryTree(pSeed);

	return 0;
}