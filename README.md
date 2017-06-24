
#include<stdio.h>
#include<graphics.h>
#include<conio.h>
#include<time.h>
#include<math.h>

#pragma comment(lib, "winmm.lib")

#define ROWS 25
#define COLS 30
int map[ROWS][COLS];

#define X 0
#define Y 80

typedef struct SNAKE
{
	int left;
	int top;
	int right;
	int bottom;
	struct SNAKE *next = NULL;
}snake;

snake *head, *tail;

#define UP 1
#define DOWN 2
#define LEFT 3
#define RIGHT 4
int direction = RIGHT;
snake * food;
int manyfood = 0;
//bool IsExistFood = FALSE;
int score = 0;
int endgamestatus = 0;
int new_tail_l, new_tail_r, new_tail_t, new_tail_b;
void InitMap();
void playmusic();
void initsnake();
void movesnake();
void judge();
int eatefood();
int biteself();
int cantcrosswall();
void pause();
void endgame();
void GameControl();
void saveresults();
int * insertnum(int *arr);
void ranking();
void deletefood(snake*p);
snake* createfood();
void main(int argc, char*argv[])
{
	food = (snake*)malloc(sizeof(snake));
	food->next = NULL;
	initgraph(800, 600);
	InitMap();
	ranking();
	playmusic();
	initsnake();

	GameControl();
	_getch();
	closegraph();
}
void InitMap()
{
	setbkcolor(GREEN);
	cleardevice();


	int i, j;
	for (i = 0; i < ROWS; i++)
		for (j = 0; j < COLS; j++)
		{
			if (i == 0 || i == (ROWS - 1) || j == 0 || j == (COLS - 1))                   //边上为1中间为0
				map[i][j] = 1;
			else
				map[i][j] = 0;

		}
	IMAGE imgtop, imgleft, imgright, imgbottom, imgtu;
	loadimage(&imgtop, _T("res\\grasstop.jpg"));
	loadimage(&imgleft, _T("res\\grassleft.jpg"));
	loadimage(&imgright, _T("res\\grassright.jpg"));
	loadimage(&imgbottom, _T("res\\grassbottom.jpg"));
	loadimage(&imgtu, _T("res\\pic.jpg"));
	putimage(0, 0, &imgtu);

	RECT r = { 0, 0, 600, 100 };
	drawtext(_T("Huppert Super Snake"), &r, DT_CENTER | DT_VCENTER | DT_SINGLELINE);



	int left = X, top = Y, right = left + 20, bottom = top + 20;

	for (i = 0; i < ROWS; i++)
	{
		left = X;
		right = left + 20;
		for (j = 0; j < COLS; j++)
		{
			if (map[i][j] == 1 && i == 0)
				putimage(left, top, &imgtop);
			if (map[i][j] == 1 && i == (ROWS - 1))
				putimage(left, top, &imgbottom);
			if (map[i][j] == 1 && j == 0)
				putimage(left, top, &imgleft);
			if (map[i][j] == 1 && j == (COLS - 1))
				putimage(left, top, &imgright);
			if (map[i][j] == 0)
			{
				setfillcolor(GREEN);
				fillrectangle(left, top, right, bottom);
			}
			left += 20; right += 20;
		}
		top += 20; bottom += 20;

	}
}
void playmusic()
{
	mciSendString(_T("open res\\backmusic.wma alias BackMusic"), NULL, 0, NULL);
	mciSendString(_T("setaudio BackMusic volume to 300"), NULL, 0, NULL);
	mciSendString(_T("play BackMusic repeat"), NULL, 0, NULL);
}

void initsnake()
{
	head = (snake*)malloc(sizeof(snake));
	snake*p;
	int startRow = 10, startCol = 5;
	head->left = startCol * 20;         //矩形框边长20
	head->top = startRow * 20;
	head->right = (startCol + 1) * 20;
	head->bottom = (startRow + 1) * 20;
	p = head;
	for (int i = 0; i < 3; i++)
	{
		tail = (snake*)malloc(sizeof(snake));
		tail->left = startCol * 20;
		tail->top = (startRow + i + 1) * 20;
		tail->right = (startCol + 1) * 20;
		tail->bottom = (startRow + i + 2) * 20;
		p->next = tail;
		tail->next = NULL;
		p = tail;
	}
	p = head;
	while (p != NULL)              //从head开始把每一个节点变成正方形，并在头节点中画眼睛
	{
		setfillcolor(RED);
		fillrectangle(p->left, p->top, p->right, p->bottom);
		if (p == head)
		{
			int x = (p->left + p->right) / 2;
			int y = (p->top + p->bottom) / 2;
			setfillcolor(YELLOW);
			fillcircle(x, y, 2);
		}
		p = p->next;
	}
}

void movesnake()
{
	int left, top, right, bottom;
	int newleft, newtop, newright, newbottom;

	snake *p;

	setcolor(WHITE);
	setfillcolor(GREEN);
	fillrectangle(tail->left, tail->top, tail->right, tail->bottom);
	p = head;

	static double i = 1, j=0;
	static int k = 0;
	j = pow(1.5, -(i++));
	k = (int)((1 - j)*20)/10;              //控制蛇速度
	while (p != NULL)
	{
		if (k <= 90)
			Sleep(100 - k);
		else
			Sleep(10);

		if (p == head)
		{
			newleft = p->left; newtop = p->top; newright = p->right; newbottom = p->bottom;
			//保留上一个节点坐标
			if (direction == RIGHT)
			{
				p->left += 20; p->right += 20;
			}
			if (direction == LEFT)
			{
				p->left -= 20; p->right -= 20;
			}
			if (direction == UP)
			{
				p->top -= 20; p->bottom -= 20;
			}
			if (direction == DOWN)
			{
				p->top += 20; p->bottom += 20;
			}
		}
		else
		{
			left = p->left;
			top = p->top;
			right = p->right;
			bottom = p->bottom;
			//保留现节点坐标
			p->left = newleft;
			p->right = newright;       //将上一节点坐标赋值给此节点
			p->top = newtop;
			p->bottom = newbottom;
			//更新上一节点坐标
			newleft = left; newtop = top; newright = right; newbottom = bottom;
		}
		setcolor(WHITE); setfillcolor(RED);
		fillrectangle(p->left, p->top, p->right, p->bottom);
		//给蛇头画眼睛
		if (p == head)
		{
			fillrectangle(newleft, newtop, newright, newbottom);
			int x = (p->left + p->right) / 2;
			int y = (p->top + p->bottom) / 2;
			setfillcolor(YELLOW);
			fillcircle(x, y, 2);
		}
		p = p->next;
	}
	new_tail_l = newleft; new_tail_r = newright; new_tail_t = newtop; new_tail_b = bottom;
}
void judge()
{

	snake *newfood, *next_apple;
	snake *p = head;
	newfood = createfood();
	p = head;              //循环判断苹果不与之前节点重合
//	bool ISSame = false;
//	bool IsEated = false;
	while (p != NULL)      //判断与蛇身每个节点是否重合
	{
		if (head->left == newfood->left&& head->top == newfood->top&&head->right == newfood->right&&head->bottom == newfood->bottom)
		{
			//			ISSame = true;
			free(newfood);
			newfood = createfood();
			p = head;
			continue;
			//判断与之前的点是否重合
		}
		p = p->next;
	}
	if (food->next == NULL)
	{
		food->next = newfood;
		IMAGE imgfood;
		loadimage(&imgfood, _T("res\\apple.jpg"));
		putimage(newfood->left + 1, newfood->top + 1, &imgfood);
	}
	else
	{
		next_apple = food->next;
		while (next_apple != NULL)    //与其他苹果是否重合
		{
			if (next_apple->left == newfood->left&& next_apple->top == newfood->top&&next_apple->right == newfood->right&&next_apple->bottom == newfood->bottom)
			{
				//				ISSame = true;
				free(newfood);
				newfood = createfood();
				next_apple = food->next;
				continue;
			}
			next_apple = next_apple->next;
		}
		IMAGE imgfood;
		loadimage(&imgfood, _T("res\\apple.jpg"));
		putimage(newfood->left + 1, newfood->top + 1, &imgfood);
		//		IsExistFood = true;

		next_apple = food;
		while (next_apple->next != NULL)//将新苹果添加为链表最后一项
		{
			next_apple = next_apple->next;
		}
		next_apple->next = newfood;
	}
	//	if (!ISSame)
	manyfood++;

}
void deletefood(snake*p)
{
	snake*next_apple = food;
	while (next_apple->next != p)
	{
		next_apple = next_apple->next;
	}
	next_apple->next = p->next;
	free(p);
	manyfood--;
	
}
int eatefood()
{
	snake *p = head;
	snake*findfood = food->next;
	while (findfood != NULL)
	{
		if ((p->left == findfood->left) && (p->top == findfood->top))
		{
			score += 10;
			deletefood(findfood);
//			IsExistFood = FALSE;
			return 1;
		}
		findfood = findfood->next;
	}
	return 0;
}
int biteself()
{
	snake *p = head->next;
	while (p != NULL)
	{
		if (p->left == head->left && p->top == head->top)
		{
			endgamestatus = 2;
			return 1;
		}p = p->next;
	}
	return 0;
}

int cantcrosswall()
{
	snake *p = head;
	if ((p->left == 0) || (p->right == 600) || (p->top == 80) || (p->bottom == 580))
	{
		endgamestatus = 1;
		return 1;
	}
	else
		return 0;
}
void pause()
{
	while (1)
	{
		Sleep(300);
		if (GetAsyncKeyState(VK_SPACE))
		{
			break;
		}
	}
}
void endgame()
{
	RECT r = { 0, 100, 600, 200 };
	RECT r2 = { 0, 200, 600, 300 };
	if (endgamestatus == 1)
	{
		drawtext(_T("撞墙了"), &r, DT_CENTER | DT_VCENTER | DT_SINGLELINE);
	}
	if (endgamestatus == 2)
	{
		drawtext(_T("咬住自己了"), &r, DT_CENTER | DT_VCENTER | DT_SINGLELINE);
	}
	if (endgamestatus == 3)
	{
		drawtext(_T("游戏结束"), &r, DT_CENTER | DT_VCENTER | DT_SINGLELINE);
	}
	char s[] = "you score:   ";
	char GetScore[10];
	_itoa_s(score, GetScore, 10);
	outtextxy(10, 20, s);
	outtextxy(80, 20, GetScore);
	pause();
	exit(0);
}
void GameControl()
{
	while (1)
	{
		if (GetAsyncKeyState(VK_UP))
		{
			direction = UP;
		}
		else if (GetAsyncKeyState(VK_DOWN))
		{
			direction = DOWN;
		}
		else if (GetAsyncKeyState(VK_LEFT))
		{
			direction = LEFT;
		}
		else if (GetAsyncKeyState(VK_RIGHT))
		{
			direction = RIGHT;
		}
		else if (GetAsyncKeyState(VK_ESCAPE))
		{
			break;
		}
		char s[] = "your score:   ";
		char GetScore[10];
		_itoa_s(score, GetScore, 10);
		outtextxy(10, 20, s);
		outtextxy(80, 20, GetScore);

		movesnake();

//		if (!IsExistFood)
		if(manyfood<=score/10*2)
		judge();
		if (eatefood() == 1)
		{
			//吃了苹果加长度
			snake * newtail = (snake*)malloc(sizeof(snake));
			newtail->left = new_tail_l;
			newtail->right = new_tail_r;
			newtail->top = new_tail_t;
			newtail->bottom = new_tail_b;
			newtail->next = NULL;
			tail->next = newtail;
			tail = newtail;
			setcolor(WHITE); setfillcolor(RED);
			fillrectangle(newtail->left, newtail->top, newtail->right, newtail->bottom);
		}


		//判断是否咬到自己和是否穿墙
		if (biteself() == 1 || cantcrosswall() == 1)
		{
			saveresults();
			endgame();
		}

	}
}
void saveresults()
{
	FILE * p;
	int score_arr[11] = { 0 };
	int *pointer = score_arr;
	if (p = fopen("scoreresult.txt", "a+"))
	{
		for (int i = 0; i < 10; ++i)
		{
			fscanf(p, "%d", &score_arr[i]);
		}
	}
	fclose(p);
	pointer = insertnum(score_arr);

	if(p=fopen("scoreresult.txt","w+"))
	for (int j = 0; j < 10; j++)
	{
		fprintf(p, "%d\n", pointer[j]);
	}
	fclose(p);
}
int * insertnum(int *arr)
{
	int count = 9;

	if (score > arr[count])        //凡score高于排行榜最后一位进入循环
	{
		arr[count + 1] = arr[count];
		arr[count] = score;
		while (score > arr[count-1]&&count>0)
		{
			count--;
			arr[count + 1] = arr[count];
			arr[count] = score;
		}
	}
	return arr;
}
void ranking()
{
	FILE *p;
	p = fopen("scoreresult.txt", "r");
	RECT r = { 600, 50, 780, 100 };
	drawtext(_T("Ranking"), &r, DT_CENTER | DT_VCENTER | DT_SINGLELINE);
	for (int i = 0; i < 10; i++)
	{
		int myint;
		char myarr[5];
		char * mystr;
		fscanf(p, "%d", &myint);
		mystr = _itoa(myint,myarr,10);
		RECT r = { 600, 100+i*40, 780, 140+i*40 };
		drawtext(_T(mystr), &r, DT_CENTER | DT_VCENTER | DT_SINGLELINE);
	}
	fclose(p);
}

snake *createfood()
{
	snake *newfood;
	srand((unsigned)time(NULL));
	newfood = (snake*)malloc(sizeof(snake));
	int row = rand() % ROWS;              //在随机行列产生
	int col = rand() % COLS;
	if (row == 0)row = 1;
	if (row == (ROWS - 1)) row = ROWS - 2;
	if (col == 0)col = 1;                 //苹果不能在边框
	if (col == (COLS - 1)) col = COLS - 2;
	newfood->left = col * 20; newfood->right = newfood->left + 20;
	newfood->top = row * 20 + 80; newfood->bottom = newfood->top + 20;
	newfood->next = NULL;
	return newfood;
}

