[![Github](https://img.shields.io/github/followers/chuckboliver?label=chuckboliver&logoColor=pink&style=social)](https://github.com/chuckboliver)

# OS_caseStudy
This repository is a part of ***OPERATING SYSTEMS*** KMITL 2021

---

## Original
```
    Summation result: 888701676
    Time used: 24172ms
```

---

## Method 1

- เพิ่ม Thread เข้าไป รวมเป็น 2 threads

```
    Summation result: 439003390
    Time used: 13324ms
```

---

## Method 2 

- ใช้ Sum_Global เป็น array ที่มีขนาดเท่ากับจำนวน thread
- ทำการแบ่ง Data_Global ออกเป็นส่วนๆโดยมีขนาดเท่ากับจำนวน thread
- แต่ละthread จะทำการบวกค่าในส่วนของตัวเองเมื่อบวกเสร็จก็เก็บค่าไว้ที่ Sum_Global array ใน index ของ thread ตัวเอง
- ทำ function ไว้สร้าง thread และ join thread
- เก็บ thread ที่สร้างไว้ใน Lists

```
static void makeThreadStart()
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

### Summary

**ผลลัพธ์ :** ถูกต้อง

**เวลา   :** เร็วขึ้น(ต้องใช้จำนวน thread มาก)

---

## Method 3 

- ทำการแบ่ง Data_Global ออกเป็นส่วนๆโดยมีขนาดเท่ากับจำนวน thread
- ใน function sum มีตัวแปร local variable ไว้บวกค่าจาก Data_Global
- แต่ละthread จะทำการบวกค่าในส่วนของตัวเองเมื่อบวกเสร็จก็เก็บค่าไว้ที่ local variable 
- หลังจากบวกค่าเสร็จจะบวกค่าจาก local variable เข้าไปใน static variable Sum_Global
- ทำ function ไว้สร้าง thread และ join thread
- เก็บ thread ที่สร้างไว้ใน Lists

```
static void makeThreadStart() 
{
    for(int i = 0; i < threadSize; ++i)
    {
        int start = i * batchSize;
        int stop = (i+1) * batchSize;
        Thread newThread = new Thread(() => sum(start, stop));
        newThread.Start();
        threadLists.Add(newThread);
    }
    Console.WriteLine("\nCreate {0} threads.", threadLists.Count);
}
```
```
static void sum(int start, int stop)
{
    long temp = 0;
    for (int index = start; index < stop; ++index)
    {
        
        if (Data_Global[index] % 2 == 0)
        {
            temp -= Data_Global[index];
        }
        else if (Data_Global[index] % 3 == 0)
        {
            temp += (Data_Global[index] * 2);
        }
        else if (Data_Global[index] % 5 == 0)
        {
            temp += (Data_Global[index] / 2);
        }
        else if (Data_Global[index] % 7 == 0)
        {
            temp += (Data_Global[index] / 3);
        }
        Data_Global[index] = 0;
        //G_index++;
    }
    Sum_Global += temp;
}
```
### Result

| Threads | Times(ms) |
| --------|-------- |
| 1       | 21685|
| 2       |10800.667|
| 4       | 5701.667|
| 8       | 3521.667|
| 16       | 2925|
| 32      | 3084.667|

### Summary

**ผลลัพธ์ :** ผิดพลาด(เมื่อthread มีจำนวนมาก)

**เวลา   :** เร็วขึ้นมากกว่า **Method 2**

---