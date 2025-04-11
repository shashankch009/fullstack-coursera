
using System.Diagnostics;

public class Program 
{
  public static void Main(string[] args) 
  {
    int[] array1 = GenerateRandomArray(size: 100000, minValue: 0, maxValue: 100);
    int[] array2 = new int[array1.Length];
    Array.Copy(array1, array2, array1.Length);
    
    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();
    
    QuickSort.Sort(array1); //see QuickSort.cs file
    
    stopWatch.Stop();
    TimeSpan ts = stopWatch.Elapsed;
    int ms = (int)ts.TotalMilliseconds;
    Console.WriteLine($"QuickSort RunTime : {ms} ms");
    
    
    stopWatch = new Stopwatch();
    stopWatch.Start();
    
    BubbleSort.Sort(array2); //see BubbleSort.cs file
    
    stopWatch.Stop();
    ts = stopWatch.Elapsed;
    ms = (int)ts.TotalMilliseconds;
    Console.WriteLine($"BubbleSort RunTime : {ms} ms");

    /*
      Sample Output 
      Array size   Quick Sort(ms)       Bubble Sort (ms)
      10           0 ms                 0 ms
      100          0 ms                 0 ms 
      1000         0 ms                 2 ms 
      10000        4 ms                 371 ms 
      100000       319 ms               44909 ms
    */
  }

  private static int[] GenerateRandomArray(int size, int minValue, int maxValue)
  {
      Random random = new Random();
      int[] array = new int[size];
      for (int i = 0; i < size; i++)
      {
          array[i] = random.Next(minValue, maxValue + 1);
      }
      return array;
  }
}
