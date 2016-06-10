using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Main
{
    class TimeList
    {
        private List<TimeItem> _items;
        private readonly string _filename;

        public TimeList(string filename)
        {
            _filename = filename;
            Load();
        }

        public void Add(int size, int count, double cppTime, double sharpTime)
        {
            _items.Add(new TimeItem(size, count, cppTime, sharpTime));
        }

        public bool Save()
        {
            Console.WriteLine("������� ��������� ������:");
            Print();
            try
            {
                using (Stream stream = File.Open(_filename, FileMode.Create))
                {
                    BinaryFormatter serializer = new BinaryFormatter();
                    serializer.Serialize(stream, _items);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        public bool Load()
        {
            try
            {
                using (Stream stream = File.Open(_filename, FileMode.Open))
                {
                    BinaryFormatter deserializer = new BinaryFormatter();
                    _items = (List<TimeItem>)deserializer.Deserialize(stream);
                }
            }
            catch (FileNotFoundException)
            {
                _items = new List<TimeItem>();
                Console.WriteLine("���� �� ����������, ����� ������ ������...");
                return true;
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            
            Console.WriteLine("������ ������� ��������:");
            Print();
            return true;
        }

        private void Print()
        {
            Console.WriteLine("-------------------------------------------------------------------------");
            Console.WriteLine("| ������ ������� | ������� | ����� (�++) ���. | ����� (C#) ���. | ����. |");
            Console.WriteLine("-------------------------------------------------------------------------");

            if (_items.Count == 0)
            {
                Console.WriteLine("|                               ����� :�                                |");
                Console.WriteLine("-------------------------------------------------------------------------");
                return;
            }

            foreach (var item in _items)
            {
                Console.WriteLine("| {0, 14} | {1, 7} | {2, 16} | {3, 15} | {4, 5:F3} |",
                    item.Size, item.Count, item.CppTime, item.SharpTime, item.K);
                Console.WriteLine("-------------------------------------------------------------------------");
            }
        }

        [Serializable]
        private class TimeItem
        {
            public TimeItem(int size, int count, double cppTime, double sharpTime)
            {
                Size = size;
                Count = count;
                CppTime = cppTime;
                SharpTime = sharpTime;
                K = sharpTime / cppTime;
            }

            public int Size { get; }

            public int Count { get; }

            public double CppTime { get; }

            public double SharpTime { get; }

            public double K { get; }
        }
    }
}