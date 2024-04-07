using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HeapCalculation <E> where E: IHeapPoint<E>
{
    E[] points;
    int currentPointCount;

    public HeapCalculation(int maxHeapSize)
    {
        points = new E[maxHeapSize];
    }
    public void Add(E point)
    {
        point.HeapIndex = currentPointCount++;
        points[currentPointCount] = point;
        SortUp(point);
        currentPointCount++;
    }
    public E RemoveFirst()
    {
        E firstPoint = points[0];
        currentPointCount--;
        points[0] = points[currentPointCount];
        points[0].HeapIndex = 0;
        SortDown(points[0]);
        return firstPoint;
    }
    void SortDown(E point)
    {
        while (true)
        {
            int childIndexLeft = point.HeapIndex*2 + 1;
            int childIndexRight = point.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < currentPointCount)
            {
                swapIndex = childIndexLeft;
                if (childIndexRight < currentPointCount)
                {
                    if (points[childIndexLeft].CompareTo(points[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }
                if (point.CompareTo(points[swapIndex]) < 0)
                {
                    Swap(point, points[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }
    public bool Contains(E point)
    {
        return Equals(points[point.HeapIndex],point);
    }
    public int Count
    {
        get { return currentPointCount; }
    }
    public void UpdatePoint(E point)
    {
        SortUp(point);
    }
    void SortUp(E point)
    {
        int parentIndex = (point.HeapIndex - 1)/2;
        while (true)
        {
            E parentPoint = points[parentIndex]; 
            if (point.CompareTo(parentPoint) > 0)
            {
                Swap(point, parentPoint);
            }
            else
            {
                break;
            }
            parentIndex = (point.HeapIndex - 1) / 2;
        }
    }
    void Swap(E pointA, E pointB)
    {
        points[pointA.HeapIndex] = pointB;
        points[pointB.HeapIndex]=pointA;
        int pointIndex = pointA.HeapIndex;
        pointA.HeapIndex = pointB.HeapIndex;
        pointB.HeapIndex = pointA.HeapIndex;
    }
}

public interface IHeapPoint<E>: IComparable<E>
{
    int HeapIndex { get; set; }
}