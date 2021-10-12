[![Github](https://img.shields.io/github/followers/chuckboliver?label=chuckboliver&logoColor=pink&style=social)](https://github.com/chuckboliver)

# OS_caseStudy
This repository is a part of ***OPERATING SYSTEMS*** KMITL 2021

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

| Threads | Times(ms) |
| --------|-------- |
| 1       | 21778.667|
| 2       |12549|
| 4       | 9613|
| 8       | 8974|
| 16       | 6627.333|
| 32      | 5750|
