using CodingTasks.Core;

namespace CodingTasks.Solutions.S30;

public sealed class Solution30_RefReturns : ISolution
{
    public int Id => 30;

    public struct Big { public long A, B, C, D; }

    // A `ref` return hands back an alias to existing storage, not a copy.
    private static ref Big Find(Big[] data, int i) => ref data[i];

    public void Run()
    {
        var arr = new Big[3];
        Console.WriteLine($"before: arr[0].A = {arr[0].A}");

        ref Big slot = ref Find(arr, 0);   // ref local: an alias to arr[0]
        slot.A = 99;                       // writes through to the array

        Console.WriteLine($"after ref write: arr[0].A = {arr[0].A}");   // 99

        // by contrast, a by-value copy would not affect the array:
        Big copy = arr[1];
        copy.A = 42;
        Console.WriteLine($"by-value copy: arr[1].A = {arr[1].A} (unchanged)");
    }
}
