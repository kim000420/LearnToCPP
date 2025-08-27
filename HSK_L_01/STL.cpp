/*##################################
STL(자료구조 수업용)
파일명: STL_empty.cpp
작성자 : 김홍규(downkhg@gmail.com)
마지막수정날짜 : 2022.03.09
버전 : 1.05
###################################*/
#include <iostream>
#include <vector>
#include <list>
#include <deque>
#include <queue>
#include <stack>
#include <map>
#include <set>
#include <string>
#include <unordered_map>//hash_map -> unordered_map: vs2019부터 변경
using namespace std;
//벡터: 동적배열
//0.배열은 데이터가 저장될공간이 미리 확보되어있다.
//1.인덱스로 원소접근이 가능하다.
//2.각 자료는 포인터연산(인덱스)을 통한 순차/랜덤접근이 가능하다.
//3.배열의 크기를 런타임중에 변경가능하다.
void VectorMain()
{
	cout << "[VectorMain Start]" << endl;
	vector<int> container(1);//컨테이너생성시 크기를 지정가능하다.
	container[0] = 10;
	cout << "Print:";
	for (int i = 0; i < container.size(); i++)
		cout << "[" << i << "]" << container[i] << ",";
	cout << endl;
	container.resize(3); //배열의 크기를 지정한다.
	cout << "Print:";
	for (int i = 0; i < container.size(); i++)
		cout << "[" << i << "]" << container[i] << ",";
	cout << endl;
	//1.추가 2.삽입 3.삭제 4.모두삭제
	vector<int>::iterator it;
	cout << "PrintPtr:";
	for (it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
	container.clear(); //모두삭제
	cout << "Clear:";
	for (it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;

	cout << "[VectorMain End]" << endl << endl;
}
//연결리스트
//1.데이터는 순차접근만 가능하다.(랜덤x)
//2.연결리스트에 추가,삽입,삭제은 O(1)이다.
//3.연결리스트의 종류: 단일, 환형, 이중 stl의 리스트는 어디에 해당되는가?
void ListMain()
{
	cout << "[VectorMain Start]" << endl;
	list<int> container(1);//컨테이너생성시 크기를 지정가능하다.
	container[0] = 10;
	cout << "Print:";
	for (int i = 0; i < container.size(); i++)
		cout << "[" << i << "]" << container[i] << ",";
	cout << endl;
	container.resize(3); //배열의 크기를 지정한다.
	cout << "Print:";
	for (int i = 0; i < container.size(); i++)
		cout << "[" << i << "]" << container[i] << ",";
	cout << endl;
	//1.추가 2.삽입 3.삭제 4.모두삭제
	list<int>::iterator it;
	cout << "PrintPtr:";
	for (it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;
	container.clear(); //모두삭제
	cout << "Clear:";
	for (it = container.begin(); it != container.end(); it++)
		cout << "[" << &*it << "]" << *it << ",";
	cout << endl;

	cout << "[VectorMain End]" << endl << endl;
}
//데크: 앞뒤로 자료를 추가/삭제가능, 랜덤접근가능.
void DequeMain()
{
	cout << "[DequeMain Start]" << endl;
	deque<int> dq(4);

	cout << "Print:";
	for (int i = 0; i < dq.size(); i++)
		cout << "[" << i << "]" << dq[i] << ",";
	cout << endl;

	//뒤에 자료 추가
	dq.push_back(10);

		cout << "PushBack:";
	for (int i = 0; i < dq.size(); i++)
		cout << "[" << i << "]" << dq[i] << ",";
	cout << endl;
	//앞에 자료 추가
	dq.push_front(10);

	cout << "PushFront:";
	for (int i = 0; i < dq.size(); i++)
		cout << "[" << i << "]" << dq[i] << ",";
	cout << endl;
	//뒤의 자료 삭제
	dq.pop_back();

	cout << "PushFront:";
	for (int i = 0; i < dq.size(); i++)
		cout << "[" << i << "]" << dq[i] << ",";
	cout << endl;
	//앞의 자료 삭제
	dq.pop_front();

	cout << "PushFront:";
	for (int i = 0; i < dq.size(); i++)
		cout << "[" << i << "]" << dq[i] << ",";
	cout << endl;
	//랜덤 접근

	dq.push_front(9);
	dq.push_front(8);
	dq.push_front(7);
	dq.push_front(6);
	dq.push_front(5);
	dq.push_front(4);
	dq.push_front(3);
	dq.push_front(2);
	dq.push_front(1);
	dq.push_front(0);

	cout << "RandomAccess:";
	cout << "[" << 3 << "]" << dq[3];
	cout << endl;

	cout << "[Deque End]" << endl << endl;
}
//스택: 뒤에서 추가되고 뒤에서 꺼냄.
//재귀함수에서 이전 함수를 호출할때마다 스택에 쌓임.
//문자열뒤집기 -> 문자배열 -> apple -> elppa
void StackMain()
{
	cout << "[Stack Start]" << endl;
	stack<int> stack;

	stack.push(10);
	cout << "Print Top: " << stack.top() << endl;
	cout << "Print Size: " << stack.size() << endl;
	stack.push(20);
	cout << "Print Top: " << stack.top() << endl;
	cout << "Print Size: " << stack.size() << endl;
	stack.push(30);
	cout << "Print Top: " << stack.top() << endl;
	cout << "Print Size: " << stack.size() << endl;
	stack.push(40);
	cout << "Print Top: " << stack.top() << endl;
	cout << "Print Size: " << stack.size() << endl << endl;

	stack.pop();
	cout << "Print Top: " << stack.top() << endl;
	cout << "Print Size: " << stack.size() << endl;
	stack.pop();
	cout << "Print Top: " << stack.top()<< endl;
	cout << "Print Size: " << stack.size() << endl;
	stack.pop();
	cout << "Print Top: " << stack.top() << endl;
	cout << "Print Size: " << stack.size() << endl;
	stack.pop();
	if (stack.empty() == 1) cout << "stack is NULL" << endl;

	cout << "[Stack End]" << endl << endl;
}
//큐: 뒤에서 추가하고 앞에서 꺼냄.
//메세지큐: 이벤트가 발생한 순서대로 저장하는 공간.
//입력된 순서대로 명령어 처리하기
void QueueMain()
{
	cout << "[Queue Start]" << endl;
	queue<int> queue;
	queue.push(10);
	cout << "queue Front: " << queue.front() << endl;
	cout << "queue Size : " << queue.size() << endl;
	queue.push(20);
	cout << "queue Front: " << queue.front() << endl;
	cout << "queue Size : " << queue.size() << endl;
	queue.push(30);
	cout << "queue Front: " << queue.front() << endl;
	cout << "queue Size : " << queue.size() << endl;
	queue.push(40);
	cout << "queue Front: " << queue.front() << endl;
	cout << "queue Size : " << queue.size() << endl << endl;

	queue.pop();
	cout << "queue Front: " << queue.front() << endl;
	cout << "queue Size : " << queue.size() << endl;
	queue.pop();
	cout << "queue Front: " << queue.front() << endl;
	cout << "queue Size : " << queue.size() << endl;
	queue.pop();
	cout << "queue Front: " << queue.front() << endl;
	cout << "queue Size : " << queue.size() << endl;
	queue.pop();
	if(queue.empty() == 1) cout << "queue is NULL"<< endl;


	cout << "[Queue End]" << endl << endl;
}
//우선순위큐: 우선순위가 높은 원소가 먼저나감(힙)
//무작위로 데이터를 넣었을때 어떤 순서대로 데이터가 나오는가? 큰값부터 나온다.
void PriorytyQueueMain()
{
	priority_queue<int> priQueue;
	priQueue.push(40);
	priQueue.push(20);
	priQueue.push(10);
	priQueue.push(50);
	priQueue.push(70);
	priQueue.push(30);
	priQueue.push(90);
	cout << "priQueue Front: " << priQueue.top() << endl;
	cout << "priQueue Size : " << priQueue.size() << endl;
	priQueue.pop();
	cout << "priQueue Front: " << priQueue.top() << endl;
	cout << "priQueue Size : " << priQueue.size() << endl;
	priQueue.pop();
	cout << "priQueue Front: " << priQueue.top() << endl;
	cout << "priQueue Size : " << priQueue.size() << endl;
	priQueue.pop();
	cout << "priQueue Front: " << priQueue.top() << endl;
	cout << "priQueue Size : " << priQueue.size() << endl;
	priQueue.pop();
	cout << "priQueue Front: " << priQueue.top() << endl;
	cout << "priQueue Size : " << priQueue.size() << endl;
	priQueue.pop();
}
//맵: 사전식으로 데이터를 찾을수있다.
//해당영어단어를 넣으면 한국어 결과가 나온다.
void MapMain()
{
	map<string, string> mapDic;

	mapDic["test"] = "시험";
	mapDic["pratice"] = "연습";
	mapDic["try"] = "도전";
	mapDic["note"] = "기록";

	cout << mapDic["try"] << endl;
	cout << mapDic["note"] << endl;
}
//셋: 순서없이 데이터를 넣는다. 데이터는 순서와 상관없이 데이터를 찾는다.
void SetMain()
{
	set<int> setData;

	setData.insert(10);
	setData.insert(20);
	setData.insert(30);
	setData.insert(40);

	set<int>::iterator it = setData.find(10);

	if (it != setData.end()) it;
	for (it = setData.begin(); it != setData.end(); it++)
		cout << *it << ",";
	cout << endl;
}
//해시맵: 해시테이블
void HashMapMain()
{
	unordered_map<string, string> mapDic;

	mapDic["test"] = "시험";
	mapDic["pratice"] = "연습";
	mapDic["try"] = "도전";
	mapDic["note"] = "기록";

	cout << mapDic["try"] << endl;
	cout << mapDic["note"] << endl;
}
void main()
{
	VectorMain();
	ListMain();
	DequeMain();
	StackMain();
	QueueMain();
	PriorytyQueueMain();
	MapMain();
	SetMain();
}