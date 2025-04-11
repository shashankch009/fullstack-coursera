public static class QuickSort 
{
    public static void Sort(int[] array)
    {
        QuickSortRecursive(array, 0, array.Length - 1);
    }

    private static void QuickSortRecursive(int[] array, int low, int high)
    {
        if (low >= high) // Base case: already sorted
        {
            return;
        }

        int pivotIndex = Partition(array, low, high);
        QuickSortRecursive(array, low, pivotIndex - 1);
        QuickSortRecursive(array, pivotIndex + 1, high);
    }

    private static int Partition(int[] arr, int low, int high)
    {
        int pivot = arr[high];
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (arr[j] <= pivot)
            {
                i++;
                (arr[i], arr[j]) = (arr[j], arr[i]);
            }
        }
        (arr[i + 1], arr[high]) = (arr[high], arr[i + 1]);
        return i + 1;
    }
}
