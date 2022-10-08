#ifndef RANDOM_H
#define RANDOM_H

#define RANDOM_DLL

#ifdef RANDOM_DLL
#define RANDOM_API __declspec(dllexport)			
#else
#define RANDOM_API __declspec(dllimport)			
#endif 


#ifdef __cplusplus
extern "C" {
#endif // __cplusplus

int RANDOM_API RAND(int max, int min, int n);

#ifdef __cplusplus
}
#endif // __cplusplus

#endif

