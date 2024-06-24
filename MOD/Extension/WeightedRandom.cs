//using Colossal.Randomization;
//using Colossal.UI.Binding;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine.InputSystem;

//namespace ExtendedRadio.Extension;

//public class WeightedRandomExtension
//{
//    public void AddRangeOnlyGUID<TKey>(this WeightedRandom<TKey> weightedRandom, IEnumerable<TKey> key, int weight) where TKey : IComparable<TKey>
//    {
//        foreach (TKey key2 in key)
//        {
//            weightedRandom.Add(key2, weight);
//        }
//    }
//    public void AddOnlyGUID<TKey>(this WeightedRandom<TKey> weightedRandom, TKey key, int weight) where TKey : IComparable<TKey>
//    {
//        weightedRandom.InsertNode(ref weightedRandom.m_Root, key, weight);
//    }

//}