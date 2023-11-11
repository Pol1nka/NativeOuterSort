using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

public class NaturalOuterSort
{
    private string? _headers;
    private readonly int _chosenField;
    private readonly string _baseFileName;
    private readonly Type[] _columnOfTableTypes 
        = { typeof(int), typeof(string), typeof(string), typeof(int) };
    private readonly List<int> _series = new();
    public NaturalOuterSort(int chosenField, string baseFileName = "../../UnsortedTable.csv")
    {
        _chosenField = chosenField;
        _baseFileName = baseFileName;
    }
    public void Sort()
    {
        while (true)
        {
            _series.Clear();
            SplitToFiles();
            if (_series.Count == 1)
            {
                break;
            }
            MergePairs();
        }
    }

    private void SplitToFiles()
    {
        using var a = new StreamReader(_baseFileName);
        _headers = a.ReadLine()!;
        using var fileB = new StreamWriter("../../B.csv");
        using var fileC = new StreamWriter("../../C.csv");
        var firstStr = a.ReadLine();
        var secondStr = a.ReadLine();
        var flag = true;
        var counter = 0;
        while (firstStr != null)
        {
            var tempFlag = flag;
            if (secondStr != null)
            {
                if (CompareElements(firstStr, secondStr))
                {
                    counter++;
                }
                else
                {
                    tempFlag = !tempFlag;
                    _series.Add(counter + 1);
                    counter = 0;
                }
            }

            if (flag)
            {
                fileB.WriteLine(firstStr);
            }
            else
            {
                fileC.WriteLine(firstStr);
            }

            firstStr = secondStr;
            secondStr = a.ReadLine();
            flag = tempFlag;
        }
        
        _series.Add(counter + 1);
    }

    private void MergePairs()
    {
        using var writerA = new StreamWriter(_baseFileName);
        using var readerB = new StreamReader("../../B.csv");
        using var readerC = new StreamReader("../../C.csv");
        writerA.WriteLine(_headers);
        var indexB = 0;
        var indexC = 1;
        var counterB = 0;
        var counterC = 0;
        var elementB = readerB.ReadLine();
        var elementC = readerC.ReadLine();
        while (elementB != null || elementC != null)
        {
            if (counterB == _series[indexB] && counterC == _series[indexC])
            {
                counterB = 0;
                counterC = 0;
                indexB += 2;
                indexC += 2;
                continue;
            } 
            
            if (indexB == _series.Count || counterB == _series[indexB])
            {
                writerA.WriteLine(elementC);
                elementC = readerC.ReadLine();
                counterC++;
                continue;
            }
            
            if (indexC == _series.Count || counterC == _series[indexC])
            {
                writerA.WriteLine(elementB);
                elementB = readerB.ReadLine();
                counterB++;
                continue;
            }


            if (CompareElements(elementB, elementC))
            {
                writerA.WriteLine(elementB);
                elementB = readerB.ReadLine();
                counterB++;
            }
            else
            {
                writerA.WriteLine(elementC);
                elementC = readerC.ReadLine();
                counterC++;
            }
        }
    }
    
    private bool CompareElements(string? element1, string? element2)
    {
        
        string parseFirstElement = element1.Split(';')[_chosenField];
        string parseSecondElement = element2.Split(';')[_chosenField];
        
        if (_columnOfTableTypes[_chosenField].IsEquivalentTo(typeof(int)))
        {
            return int.Parse(parseFirstElement).CompareTo(int.Parse(parseSecondElement)) <= 0;
        }
        if (_columnOfTableTypes[_chosenField].IsEquivalentTo(typeof(DateTime)))
        {
            return DateTime.Parse(parseFirstElement).CompareTo(DateTime.Parse(parseSecondElement)) <= 0;
        }
        return string.Compare(parseFirstElement,parseSecondElement, StringComparison.Ordinal) <= 0;
    }
}