using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Collections.Generic; 
using System.Linq;

namespace Problem01
{
    class Program
    {
        static Mutex mutex = new Mutex();
        static Semaphore semaphoreObject = new Semaphore(initialCount: 1, maximumCount: 1, name: "semaphore");
        static List<Thread> threadLists = new List<Thread>();
        static int MAX = 1000000000;
        static int threadSize = 4;
        static int batchSize = MAX / threadSize;
        static byte[] Data_Global = new byte[MAX];
        static long Sum_Global = 0;
        static int G_index = 0;

        static int ReadData()
        {
            int returnData = 0;
            FileStream fs = new FileStream("Problem01.dat", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            try
            {
                Data_Global = (byte[])bf.Deserialize(fs);
            }
            catch (SerializationException se)
            {
                Console.WriteLine("Read Failed:" + se.Message);
                returnData = 1;
            }
            finally
            {
                fs.Close();
            }

            return returnData;
        }
        static void MakeThreadStart() 
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

        static void MakeThread()
        {
            for(int i = 0; i < threadSize; ++i)
            {
                int start = i * batchSize;
                int stop = (i+1) * batchSize;
                Thread newThread = new Thread(() => sum(start, stop));
                threadLists.Add(newThread);
            }
            Console.WriteLine("\nCreate {0} threads.", threadLists.Count);
        }

        static void StartThread()
        {
            foreach(Thread t in threadLists)
            {
                t.Start();
            }
        }

        static void JoinThread()
        {
            foreach(Thread t in threadLists)
            {
                t.Join();
            }
        }

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

        static void sumCallback(int start, int stop, CountdownEvent evt)
        {
            sum(start, stop);
            evt.Signal();
        }

        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            int y;

            /* Read data from file */
            Console.Write("Data read...");
            y = ReadData();
            if (y == 0)
            {
                Console.WriteLine("Complete.");
            }
            else
            {
                Console.WriteLine("Read Failed!");
            }

            /* Start */
            Console.Write("\n\nWorking...");
            sw.Start();
            
            //makeThreadStart();
            //joinThread();
            MakeThreadPool();


            sw.Stop();
            Console.WriteLine("Done.");

            /* Result */
            Console.WriteLine("Summation result: {0}", Sum_Global);
            Console.WriteLine("Time used: " + sw.ElapsedMilliseconds.ToString() + "ms");
        }
    }
}
