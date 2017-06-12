#include<graphics.h>
#include<conio.h>

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
int direction= RIGHT;

void InitMap()
{
	
}

void playmusic()
{
	
}

void initsnake()
{

}

void movesnake()
{
	int left,top,right,bottom;
	int newleft,newtop,newright,newbottom;

	snake *p;

	setcolor(WHITE);
	setfillcolor(GREEN);
	fillrectangle(tail->left,tail->top,tail->right,tail->bottom);
	p = head;
	while(p!=NULL)
	{
		Sleep(100);
		if(p--head)
		{
			newleft = p->left;newtop = p->top;newright = p->right;newbottom = p->bottom;
			if(direction==RIGHT)
			{
				p->left+=20;p->right+=20;
			}
			if(direction == LEFT)
			{
				p->left -=20; p->right +=20;
			}
			if(direction == UP)
			{
				p->top -=20;p->bottom -=20;
			}
			if(direction == DOWN)
			{
				p->top +=20;p->bottom += 20;
			}
		}
	}
}

void main()
{
	initgraph(640,480);
	RECT r = {0,0,300,100};
	
	drawtext(_T("sdkghfhgdkfb"),&r,DT_CENTER | DT_VCENTER | DT_SINGLELINE);
	getch();
	closegraph();
}
