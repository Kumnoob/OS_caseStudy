[![Github](https://img.shields.io/github/followers/chuckboliver?label=chuckboliver&logoColor=pink&style=social)](https://github.com/chuckboliver)

# OS Case study
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

- ใช้ ***Sum_Global*** เป็น array ที่มีขนาดเท่ากับจำนวน thread
- ทำการแบ่ง ***Data_Global*** ออกเป็นส่วนๆโดยมีขนาดเท่ากับจำนวน thread
- แต่ละ thread จะทำการบวกค่าในส่วนของตัวเองเมื่อบวกเสร็จก็เก็บค่าไว้ที่ ***Sum_Global***l array ใน index ของ thread ตัวเอง
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
```
```
static void joinThread()
{
    foreach(Thread t in threadLists)
    {
        t.Join();
    }
}
```
```
static void sum(int start, int stop, int tid)
{
    for (int index = start; index < stop; ++index)
    {
        
        if (Data_Global[index] % 2 == 0)
        {
            Sum_Global[tid] -= Data_Global[index];
        }
        else if (Data_Global[index] % 3 == 0)
        {
            Sum_Global[tid] += (Data_Global[index] * 2);
        }
        else if (Data_Global[index] % 5 == 0)
        {
            Sum_Global[tid] += (Data_Global[index] / 2);
        }
        else if (Data_Global[index] % 7 == 0)
        {
            Sum_Global[tid] += (Data_Global[index] / 3);
        }
        Data_Global[index] = 0;
        //G_index++;
    }
}
```

### Result :

| Threads | Times(ms) |
| --------|-------- |
| 1       | 21778.667|
| 2       |12549|
| 4       | 9613|
| 8       | 8974|
| 16       | 6627.333|
| 32      | 5750|

### Summary :

**ผลลัพธ์ :** ถูกต้อง

**เวลา   :** เร็วขึ้น(ต้องใช้จำนวน thread มาก)

---

## Method 3 

- ทำการแบ่ง ***Data_Global*** ออกเป็นส่วนๆโดยมีขนาดเท่ากับจำนวน thread
- ใน function ***sum*** มีตัวแปร local variable ไว้บวกค่าจาก ***Data_Global***
- แต่ละthread จะทำการบวกค่าในส่วนของตัวเองเมื่อบวกเสร็จก็เก็บค่าไว้ที่ local variable 
- หลังจากบวกค่าเสร็จจะบวกค่าจาก local variable เข้าไปใน static variable ***Sum_Global***

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
### Result :

| Threads | Times(ms) |
| --------|-------- |
| 1       | 21685|
| 2       |10800.667|
| 4       | 5701.667|
| 8       | 3521.667|
| 16       | 2925|
| 32      | 3084.667|

### Summary :

**ผลลัพธ์ :** ผิดพลาด(เมื่อthread มีจำนวนมาก)

**เวลา   :** เร็วขึ้นมากกว่า **Method 2**

---

## Method 4

- แก้ปัญหาจาก **Method 3**
- ใช้ Semaphore เพื่อให้แต่ละ thread เข้าไปบวกค่าใน static variable Sum_Global ได้ทีละ 1 thread
- กำหนด Semaphore เริ่มต้น 1 มากสุด 1

```
static Semaphore semaphoreObject = new Semaphore(initialCount: 1, maximumCount: 1, name: "semaphore");
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
    semaphoreObject.WaitOne();
    Sum_Global += temp;
    semaphoreObject.Release();
}
```

### Summary :

**ผลลัพธ์ :** ถูกต้อง

**เวลา   :** ช้ากว่า **Method 3**

---

## Method 5

- แก้ปัญหาจาก **Method 3**
- ใช้ Mutex เพื่อให้แต่ละ thread เข้าไปบวกค่าใน static variable Sum_Global ได้ทีละ 1 thread
  
```
static Mutex mutex = new Mutex();
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
    mutex.WaitOne();
    Sum_Global += temp;
    mutex.ReleaseMutex();
}
```

### Summary :

**ผลลัพธ์ :** ถูกต้อง

**เวลา   :** ช้ากว่า **Method 3**, **Method 4**

---

## Method 6 :

- นำ **Method4** มาต่อยอด
- ใช้ ThreadPool มาช่วยในการจัดการ thread 
- โดยปกติใน .NET framework ได้รับ request จากการเรียก method ก็ทำการสร้าง thread object ในหน่วยความจำ จากนั้นก็ทำ task ที่ได้รับมา
  เมื่อ task เสร็จ garbage collector ก็จะ clear thread object ออกจากหน่วยความจำเพื่อคืนพื้นที่ 
> request เยอะ = .NET framework สร้าง thread object เยอะ (จองพื้นที่ในหน่วยความจำ) = ช้า

- ThreadPool คือ Collections ของ thread หลังจาก thread ทำงานเสร็จ thread จะถูกส่งไปที่ pool(ไม่ส่งไป garbage collector) เพื่อรอการใช้งาน ก็คือสามารถ reused thread ได้ ไม่ต้องสร้าง thread ใหม่
  
```
static void sumCallback(int start, int stop, CountdownEvent evt)
{
    sum(start, stop);
    evt.Signal();
}
```
```
static void MakeThreadPool()
{
    Console.WriteLine("\nCreate {0} threads.", threadSize);
    using(CountdownEvent counter = new CountdownEvent(threadSize))
    {
        for(int i = 0; i < threadSize; ++i)
        {
            int start = i * batchSize;
            int stop = (i+1) * batchSize;
            ThreadPool.QueueUserWorkItem(callBack => sumCallback(start, stop, counter));
        }
        counter.Wait();
        Console.WriteLine("All threads finish execution.");
    }
}
```

### Summary :

**ผลลัพธ์ :** ถูกต้อง

**เวลา   :** เร็ว