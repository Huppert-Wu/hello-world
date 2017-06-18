#include<graphics.h>
#include<conio.h>
#include<time.h>
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
	struct SNAKE *next;
}snake;

snake *head, *tail;

#define UP 1
#define DOWN 2
#define LEFT 3
#define RIGHT 4
int direction = RIGHT;

snake* food;
bool IsExistFood = FALSE;
int score = 0;
int endgamestatus = 0;
void InitMap()
{
	setbkcolor(GREEN);
	cleardevice();


	int i, j;
	for (i = 0; i < ROWS; i++)
		for (j = 0; j < COLS; j++)
		{
		if (i == 0 || i == (ROWS - 1) || j == 0 || j == (COLS - 1))map[i][j] = 1;
		else map[i][j] = 0;
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
		left = X; right = left + 20;
		for (j = 0; j < COLS; j++)
		{
			if (map[i][j] == 1 && i == 0)putimage(left, top, &imgtop);
			if (map[i][j] == 1 && i == (ROWS - 1))putimage(left, top, &imgbottom);
			if (map[i][j] == 1 && j == 0)putimage(left, top, &imgleft);
			if (map[i][j] == 1 && j == (COLS - 1))putimage(left, top, &imgright);
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
	while (p != NULL)
	{
		Sleep(100);
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
}
void createfood()
{
	snake *p = head;
	srand((unsigned)time(NULL));
	food = (snake*)malloc(sizeof(snake));
	int row = rand() % ROWS;              //在随机行列产生
	int col = rand() % COLS;
	if (row == 0)row = 1;
	if (row == (ROWS - 1)) row = ROWS - 2;
	if (col == 0)col = 1;                 //苹果不能在边框
	if (col == (COLS - 1)) col = COLS - 2;
	food->left = col * 20; food->right = food->left + 20;
	food->top = row * 20 + 80; food->bottom = food->top + 20;
	p = head;
	bool ISSame = false;
	bool IsEated = false;
	while (p != NULL)      //判断与蛇身每个节点是否重合
	{
		if (head->left== food->left&& head->top == food->top&&head->right  == food->right&&head->bottom == food->bottom)
		{
			ISSame = true;
			free(food);
			createfood();
		}
		p = p->next;
	}
	if (!ISSame)
	{
		IMAGE imgfood;
		loadimage(&imgfood, _T("res\\apple.jpg"));
		putimage(food->left + 1, food->top + 1, &imgfood);
		IsExistFood = true;
	}
}
int eatefood()
{

}


int biteself()
{

}

int cantcrosswall()
{

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
	char s[] = "you scores:";
	char GetScore[10];
	itoa(score, GetScore, 10);
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
		movesnake();
		if (!IsExistFood)
			createfood();
		eatefood();
		if (biteself() == 1 || cantcrosswall() == 1)
		{
			endgame();
		}

	}
}
void main(int argc, char*argv[])
{
	initgraph(640, 480);
	InitMap();
	playmusic();
	initsnake();
	GameControl();
	_getch();
	closegraph();
}


