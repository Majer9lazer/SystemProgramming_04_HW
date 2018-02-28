using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        #region Example with return values
        static int SomeExpensiveResult = 0;
        static void SomeExpensiveCalculation()
        {
            Thread.Sleep(10000);
            SomeExpensiveResult = new Random().Next(0, 10);
        }

        static void ReturnValuesThreadRunner()
        {
            Thread calculator = new Thread(SomeExpensiveCalculation);
            calculator.Start();
            calculator.Join();

            Console.WriteLine("Calculation completed");
            Console.WriteLine(SomeExpensiveResult);
        }
        #endregion

        #region Example with Exception catching
        static void SomeDangerousMethod()
        {
            Console.WriteLine("I am kamikaze...");
            throw new Exception();
        }

        static void ExceptionThreadRunner()
        {
            try
            {
                SomeDangerousMethod();
                new Thread(SomeDangerousMethod).Start();

                //new Thread(SomeDangerousMethod).Start();
                //SomeDangerousMethod();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        #endregion

        #region Example with Complex BL
        static void SendEmailConfirmation() => Thread.Sleep(5000);
        static void SendSmsNotification() => Thread.Sleep(10000);
        static void WriteCommandToDb() => Thread.Sleep(5000);

        static void ProcessComplexBl()
        {
            Thread [] threads = new Thread[3] 
            {
                new Thread(SendEmailConfirmation) { Name = "EmailConfirmer" },
                new Thread(SendSmsNotification),
                new Thread(WriteCommandToDb)
            };

            foreach (var item in threads)
            {
                item.Start();
                item.Join();
            }
            //foreach (var item in threads)
            //{
            //    item.Join();
            //}
            // Blocking line
            Console.WriteLine("All threads have been returned");
            foreach (var item in threads)
            {
                Console.WriteLine(item.ThreadState);
            }
        }
        #endregion

        // Thread synchronization
        #region Thread Blocking with Lock
        public class ThreadSafe
        {
            public static object locker = new object();
            public static int x = 10, y = 2;
            public static void Go()
            {
                // Threading Monitor
                lock (locker)
                {
                    if (y != 0)
                        Console.WriteLine(x / y);
                    y = 0;
                }
            }
        }
        #endregion

        #region Thread Interrupting
        static void SomeTask()
        {
            try
            {
                Thread.Sleep(100000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }
        static void MainThread()
        {
            Thread t = new Thread(SomeTask);
            t.Start();
            Thread.Sleep(5000);
            t.Interrupt();
        }
        #endregion

        #region Example with abort thread
        static void LogDateWriter()
        {
            while (true)
            {
                Console.WriteLine(DateTime.Now.ToShortDateString());
                Thread.Sleep(1000);
            }
        }

        static void AbortThreadRunner()
        {
            Thread worker = new Thread(LogDateWriter);
            Thread.Sleep(10000);
            worker.Abort(); // Do not do like this way
        }
        #endregion

        // AutoResetEvent
        #region AutoResetEvent Example
        public class AutoResetEventExample
        {
            static AutoResetEvent waitHandler = new AutoResetEvent(true);
            static int x = 0;

            public static void RunExample()
            {
                for (int i = 0; i < 5; i++)
                {
                    Thread myThread = new Thread(Count);
                    myThread.Name = "Thread " + i.ToString();
                    myThread.Start();
                }
            }
            public static void Count()
            {
                waitHandler.WaitOne();
                x = 1;
                for (int i = 1; i < 9; i++)
                {
                    Console.WriteLine("{0}: {1}", Thread.CurrentThread.Name, x);
                    x++;
                    Thread.Sleep(100);
                }
                waitHandler.Set();
            }
        }
        #endregion
        // Mutex
        #region Mutex Examples
        public class MutexExample
        {
            static Mutex mutexObj = new Mutex();
            static int x = 0;

            static void RunExample()
            {
                for (int i = 0; i < 5; i++)
                {
                    Thread myThread = new Thread(Count);
                    myThread.Name = "Thread " + i.ToString();
                    myThread.Start();
                }

                Console.ReadLine();
            }
            public static void Count()
            {
                mutexObj.WaitOne();
                x = 1;
                for (int i = 1; i < 9; i++)
                {
                    Console.WriteLine("{0}: {1}", Thread.CurrentThread.Name, x);
                    x++;
                    Thread.Sleep(100);
                }
                mutexObj.ReleaseMutex();
            }
        }
        public class MutexExampleTwo
        {
            public static void RunExample()
            {
                bool existed;
                string guid = Marshal.GetTypeLibGuidForAssembly(Assembly.GetExecutingAssembly()).ToString();

                Mutex mutexObj = new Mutex(true, guid, out existed);

                if (existed)
                {
                    Console.WriteLine("Unique app per system");
                }
                else
                {
                    Console.WriteLine("Already running");
                    Thread.Sleep(3000);
                    return;
                }
                Console.ReadLine();
            }
        }
        #endregion

        // Semaphore
        #region Semaphore Example
        public class SemaphoreExample
        {
            static Semaphore s = new Semaphore(3, 3);  // Available= 3; Capacity  =3

            public static void RunExample()
            {
                for (int i = 0; i < 10; i++)
                    new Thread(Go).Start();
            }
            static void Go()
            {
                while (true)
                {            
                    s.WaitOne();
                    Console.WriteLine("Inside");
                    Thread.Sleep(10000);
                    s.Release();
                }
            }
        }
        #endregion
        // Timer
        #region Timer Example
        public class TimerExample
        {
            static void RunExample()
            {
                int num = 0;
                TimerCallback tm = new TimerCallback(Count);
                Timer timer = new Timer(tm, num, 0, 2000);

                Console.ReadLine();
            }
            public static void Count(object obj)
            {
                int x = (int)obj;
                for (int i = 1; i < 9; i++, x++)
                {
                    Console.WriteLine("{0}", x * i);
                }
            }
        }
        #endregion
        // Thread Pool
        static void Main(string[] args)
        {
            Console.ReadLine();
        }
    }
}
