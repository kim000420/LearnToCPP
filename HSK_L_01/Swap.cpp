#include <stdio.h>

// 문제: 데이터 2개를 저장하고 
// 저장된 데이터를 각각 값/포인터/참조를 이용해 함수를 이용하여 두 값을 교환해보고 
// 변수의 값과 주소값을 출력한다

// 알고리즘
// 1. 변수 2개 선언 및 초기화
// 2. Swap함수 3개만들기(값/포인터/참조)
// 3. 각 변수의 데이터와 주소값 출력하기

int SwapData(int A, int B)
{
	int buf;

	buf = A;
	A = B;
	B = buf;

	printf("A = %d[%d], B = %d[%d]\n", A, &A, B, &B);
	return 0;
}

int SwapPointer(int* pA, int* pB)
{
	int buf;

	buf = *pA;
	*pA = *pB;
	*pB = buf;

	printf("numA = %d[%d], numB = %d[%d]\n", *pA, &pA, *pB, &pB);
	return 0;
}

int SwapReference(int &A, int &B)
{
	int buf;

	buf = A;
	A = B;
	B = buf;

	printf("numA = %d[%d], numB = %d[%d]\n", A, &A, B, &B);
	return 0;
}

int main()
{
	int numA = 10;
	int numB = 20;

	printf("numA = %d[%d], numB = %d[%d]\n", numA, &numA, numB, &numB);
	SwapData(numA,numB);
	printf("numA = %d[%d], numB = %d[%d]\n", numA, &numA, numB, &numB);
	SwapPointer(&numA,&numB);
	printf("numA = %d[%d], numB = %d[%d]\n", numA, &numA, numB, &numB);
	SwapReference(numA,numB);

}
