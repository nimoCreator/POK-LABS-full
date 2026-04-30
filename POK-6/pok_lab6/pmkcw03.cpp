///////////////////////////////
///////
/////// Przykladowa wizualizacja 
/////// modelu komputerowego
/////// prostego wahadla
/////// w OpenGL/GLUT
///////
///////////////////////////////

#include <cstdlib>
#include <cmath>
using namespace std;

#include <GL/glut.h>

//// zmienne globalne

GLenum rgb_mode, doubleBuffer, windType;
GLint windW, windH;

typedef long double real; 

const real Pi = 3.14159265358979323846; 
const real radiany = Pi/180.0;

real om,fi,delt,g,l;
real x, y;

static void Inicjuj(void)
{
//// zainiocjowanie algorytmu programu

// parametry sta³e wahadla:
 delt=0.025;
 g=9.81;
 l=0.9;

// warunki poczatkowe wahadla:
 om=0.0;
 fi=40.0*radiany;
 x = l*sin(fi);
 y = l*cos(fi);
}

static void ZmienionoRozmiar(int width, int height)
{
//// procedura reakcji na zdarzenie zmiany rozmiaru okna

// ustalenie obszaru widoku grafiki:
 windW = (GLint)width;
 windH = (GLint)height;
 glViewport(0, 0, width, height);

// ustalenie typu rzutowania grafiki na ekran:
 glMatrixMode(GL_PROJECTION);
 glLoadIdentity();
 gluOrtho2D(-1, 1, -1, 1); // wspolrzedne x,y zmieniaja sie od -1 do +1
 
// ustalenie transformacji modelu/widoku sceny:
 glMatrixMode(GL_MODELVIEW);
 glLoadIdentity();
}

static void Rysuj(void)
{
//// procedura rysowania w oknie

 const real h = 0.5; // wysokosc podstawy wahadla
 const real p = 0; // przesuniecie zaczepu wahadla

 glEnable(GL_BLEND);
 glEnable(GL_POINT_SMOOTH);
 glClear(GL_COLOR_BUFFER_BIT);

 glColor3f(0.75, 0.5, 0.25); // kolor grafiki
 glLineWidth(3); // grubosc linii
 glPointSize(10); // rozmiar punktow

 glBegin(GL_POINTS); // rysuj punkt serca wahadla
  glVertex2f(x+p, -y+h);
 glEnd();

 glBegin(GL_LINE_STRIP); // rysuj linie wahadla
  glVertex2f(p, h);
  glVertex2f(x+p, -y+h);
 glEnd();

 glBegin(GL_QUADS); // rysuj prostokat podstawy zaczepu wahadla
  glVertex2f(-0.5, h);
  glVertex2f(0.5, h);
  glVertex2f(0.5, h+0.05);
  glVertex2f(-0.5, h+0.05);
 glEnd();
 
 glFlush(); // splukanie operacji graficznych

 if (doubleBuffer) glutSwapBuffers(); // zamien bufory tylni i przedni miejscami
}

void Oblicz(void)
{
//// procedura wykonywana, gdy mechanizm obslugi zdarzen jest w stanie biegu jalowego

// zmien stan wahadla zgodnie z jego rownaniem:

 om = om - delt*(g/l)*sin(fi);
 fi = fi + delt*om;

 x = l*sin(fi);
 y = l*cos(fi);

// wyslij zdarzenie odrysowania okna:
 glutPostRedisplay();
}

int main(int argc, char **argv)
{
//// glowna funkcja programu

// zanicjowanie GLUT:
 glutInit(&argc, argv);

// ustalenie polozenia i rozmiaru okna w pikselach:
 windW = 300;
 windH = 300;
 glutInitWindowPosition(100, 100); 
 glutInitWindowSize(windW, windH);

// ustalenie trybu wyswietlania grafiki w oknie:

 rgb_mode = GL_TRUE; // wybor kodowania kolorow grafiki jako RGB
 doubleBuffer = GL_TRUE; // podwojne buforowanie grafiki

 windType = (rgb_mode) ? GLUT_RGB : GLUT_INDEX;
 windType |= (doubleBuffer) ? GLUT_DOUBLE : GLUT_SINGLE;
 glutInitDisplayMode(windType);

// stworzenie i zainicjowanie okna:

 if (glutCreateWindow("Wahad³o") == GL_FALSE) exit(1);

 glClearColor(0.1, 0.75, 0.2, 0.0); // kolor tla okna
 glBlendFunc(GL_SRC_ALPHA, GL_ZERO);
 
// zainicjowanie programu:
 Inicjuj();

// przeslanie procedur wywolania zwrotnego (ang. callback) do GLUT:
 glutReshapeFunc(ZmienionoRozmiar);
 glutDisplayFunc(Rysuj);
 glutIdleFunc(Oblicz);

// wykonanie glownej petli jako mechanizmu obslugi zdarzen i obliczen:
 glutMainLoop();
 
 return 0;
}
