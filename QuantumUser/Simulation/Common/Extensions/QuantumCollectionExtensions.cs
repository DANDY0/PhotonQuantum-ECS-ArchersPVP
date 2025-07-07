using System;
using System.Collections.Generic;
using Quantum.Collections;

namespace Quantum.QuantumUser.Simulation.Common.Extensions
{
    public static class QuantumCollectionExtensions
    {
        /// <summary>
        /// Заполняет QList значениями из переданного списка.
        /// </summary>
        /// <typeparam name="TItem">Тип элементов, хранящихся в списке.</typeparam>
        /// <param name="f">Текущий кадр (Frame).</param>
        /// <param name="qListId">Идентификатор QList, в который будут добавлены значения.</param>
        /// <param name="items">Список значений, которые нужно добавить в QList.</param>
        public static void FillListComponent<TItem>(this Frame f,
            QListPtr<TItem> qListId,
            List<TItem> items) 
            where TItem : unmanaged
        {
            QList<TItem> qList = f.ResolveList(qListId);

            foreach (TItem item in items)
                qList.Add(item);
        }
        
        /// <summary>
        /// Добавляет элементы в существующий QList.
        /// </summary>
        /// <typeparam name="TItem">Тип элементов, хранящихся в списке.</typeparam>
        /// <param name="f">Текущий кадр (Frame).</param>
        /// <param name="qListId">Идентификатор существующего QList.</param>
        /// <param name="items">Список значений, которые нужно добавить в QList.</param>
        public static void AddToListComponent<TItem>(this Frame f,
            QListPtr<TItem> qListId,
            List<TItem> items)
            where TItem : unmanaged
        {
            QList<TItem> qList = f.ResolveList(qListId);

            foreach (TItem item in items)
                qList.Add(item);
        }


        /// <summary>
        /// Заполняет QDictionary значениями из переданного словаря.
        /// </summary>
        /// <typeparam name="TKey">Тип ключей.</typeparam>
        /// <typeparam name="TValue">Тип значений.</typeparam>
        /// <param name="f">Текущий кадр (Frame).</param>
        /// <param name="qDictId">Идентификатор QDictionary, в который будут добавлены значения.</param>
        /// <param name="dictionary">Исходный словарь, который нужно добавить в QDictionary.</param>
        public static void FillDictionaryComponent<TKey, TValue>(this Frame f,
            QDictionaryPtr<TKey, TValue> qDictId,
            Dictionary<TKey, TValue> dictionary) 
            where TKey : unmanaged, IEquatable<TKey>
            where TValue : unmanaged
        {
            QDictionary<TKey, TValue> qDict = f.ResolveDictionary(qDictId);

            foreach (TKey key in dictionary.Keys)
                qDict.Add(key, dictionary[key]);
        }

        /// <summary>
        /// Заполняет QDictionary значениями из переданного словаря.
        /// </summary>
        /// <typeparam name="TKey">Тип ключей.</typeparam>
        /// <typeparam name="TValue">Тип значений.</typeparam>
        /// <param name="f">Текущий кадр (Frame).</param>
        /// <param name="qDictId">Идентификатор QDictionary, в который будут добавлены значения.</param>
        /// <param name="dictionary">Исходный словарь, который нужно добавить в QDictionary.</param>
        public static void FillEnumDictionaryComponent<TKey, TValue>(this Frame f,
            QDictionaryPtr<TKey, TValue> qDictId,
            Dictionary<TKey, TValue> dictionary) 
            where TKey : unmanaged, Enum
            where TValue : unmanaged
        {
            QEnumDictionary<TKey, TValue> qDict = f.ResolveDictionary(qDictId);

            foreach (TKey key in dictionary.Keys)
                qDict.Add(key, dictionary[key]);
        }
    }
}