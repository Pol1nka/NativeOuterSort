namespace mergeSort;

public class Program
{
    public static void Main(string[] args)
    {
        NaturalOuterSort sort = new NaturalOuterSort(2, "../../A.csv");
        
        sort.Sort();
    }
}



