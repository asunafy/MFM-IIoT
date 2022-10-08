#include "random.h"
#include <iostream>
#include <bitset> 
#include <stdint.h>
#include <time.h>
using namespace std;
int RAND(int max, int min, int n)
{
    int number = 0;
	srand(time(NULL));
    for (int i = 0; i < n; i++)
    {
        number = (double)rand() / (RAND_MAX + 1) * (max - min) + min;  
    }
	return number;
}
