[![Github](https://img.shields.io/github/followers/chuckboliver?label=chuckboliver&logoColor=pink&style=social)](https://github.com/chuckboliver)

# OS_caseStudy
This repository is a part of OPERATING SYSTEMS KMITL 2021

1.) **Original**
```
    Summation result: 888701676
    Time used: 24172ms
```

2.) **2 threads**
```
    Summation result: 439003390
    Time used: 13324ms
```

## Method 3 (ผลลัพธ์ถูกต้อง)

- ใช้ Sum_Global เป็น array ที่มีขนาดเท่ากับจำนวน thread ที่สร้าง โดยทำการแบ่ง Data_Global ออกเป็นส่วนๆโดยมีขนาดเท่ากับจำนวน thread
- แต่ละthread จะทำการบวกค่าในส่วนของตัวเองเมื่อบวกเสร็จก็เก็บค่าไว้ที่ Sum_Global array ใน index ของ thread ตัวเอง
- ทำ function ไว้สร้าง thread และ join thread
- เก็บ thread ที่สร้างไว้ใน Lists

```
static void makeThread()
{
    for(int i = 0; i < threadSize; ++i)
    {
        int tid = i;
        int start = i * batchSize;
        int stop = (i+1) * batchSize;
        Thread newThread = new Thread(() => sum(start, stop, tid));
        newThread.Start();
        threadLists.Add(newThread);
    }
    Console.WriteLine("\nCreate {0} threads.", threadLists.Count);
}

static void joinThread()
{
    foreach(Thread t in threadLists)
    {
        t.Join();
    }
}
```

### Result

- 1 thread
```
Summation result: 888701676
Time used: 21845ms
Summation result: 888701676
Time used: 21558ms
Summation result: 888701676
Time used: 21933ms
Average time used: 21778.667ms
```

- 2 threads
```
Summation result: 888701676
Time used: 12700ms
Summation result: 888701676
Time used: 12678ms
Summation result: 888701676
Time used: 12269ms
Average time used: 12549ms
```

- 4 threads
```
Summation result: 888701676
Time used: 9710ms
Summation result: 888701676
Time used: 9453ms
Summation result: 888701676
Time used: 9676ms
Average time used: 9613ms
```

- 8 threads
```
Summation result: 888701676
Time used: 8840ms
Summation result: 888701676
Time used: 9139ms
Summation result: 888701676
Time used: 8943ms
Average time used: 8974ms
```

- 16 threads
```
Summation result: 888701676
Time used: 6833ms
Summation result: 888701676
Time used: 6394ms
Summation result: 888701676
Time used: 6655ms
Average time used: 6627.333ms
```

- 32 threads
```
Summation result: 888701676
Time used: 5646ms
Summation result: 888701676
Time used: 5802ms
Summation result: 888701676
Time used: 5802ms
Average time used: 5750ms
```
