using System;
using System.Linq;


namespace Lab_7
{
    public class Green_3
    {
        public class Student
        {
            private static int _studentID;
            private int _id;
            private string _name;
            private string _surname;
            private int[] _marks;
            private int _count_exams;
            private bool _session_outcome;

            static Student()
            {
                _studentID = 1;
            }

            public Student(string name, string surname)
            {
                _id = _studentID++;
                _name = name;
                _surname = surname;
                _marks = new int[3]; // инициализируем массив нулями
                _session_outcome = false;
                _count_exams = 0;
            }
            public string Name
            {
                get
                {
                    return _name;
                }
            }
            public string Surname
            {
                get
                {
                    return _surname;
                }
            }

            public int ID
            {
                get
                {
                    return _id;
                }
            }

            private static int CurrentID // сделано приватным
            {
                get
                {
                    return _studentID;
                }
            }

            public int[] Marks
            {
                get
                {
                    return (int[])_marks?.Clone() ?? Array.Empty<int>(); // ? предотвращает ошибку, тк вмето вызова Clone возвращается null
                }
            }

            public double AvgMark
            {
                get
                {
                    if (_marks.Length == 0 || _marks == null)
                    {
                        return 0;
                    }

                    double sum = 0;
                    foreach (int mark in _marks)
                    {
                        sum += mark;
                    }

                    return _count_exams != 0 ? sum / (double)_count_exams : 0;
                }
            }

            public bool IsExpelled
            {
                get
                {
                    if (_count_exams == 0)
                    {
                        return false;
                    }
                    for (int i = 0; i < _count_exams; i++)
                    {
                        if (_marks[i] <= 2)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }

            public void Restore()
            {
                // Проверка: студент не должен быть "новым"
                if (_id >= CurrentID)
                {
                    Console.WriteLine("Студента ранее не было в списке. Восстановление невозможно");
                    return;
                }

                if (_session_outcome == true) _session_outcome = false;
            }

            public void Exam(int mark)
            {

                if (_marks.Length == 0 || _marks == null) return;

                if (_count_exams >= 3) return;

                if (!_session_outcome)
                {
                    _marks[_count_exams] = mark;
                    _count_exams++;

                    if (mark <= 2)
                    {
                        _session_outcome = true;
                    }
                }
            }


            public static void SortByAvgMark(Student[] array)
            {
                if (array == null) return;
                for (int i = 1; i < array.Length - 1; i++)
                {
                    for (int j = i + 1; j < array.Length; j++)
                    {
                        if (i == 0 || array[i].AvgMark < array[j].AvgMark)
                        {
                            Student temp = array[i];
                            array[i] = array[j];
                            array[j] = temp;
                        }
                    }
                }
            }

            public void Print()
            {
                Console.WriteLine($"{ID} {Name} {Surname} {AvgMark} {IsExpelled}");
            }
        }

        public class Commission
        {
            public static void Sort(Student[] students)
            {
                if (students == null) return;
                for (int i = 1; i < students.Length - 1; i++)
                {
                    for (int j = i + 1; j < students.Length; j++)
                    {
                        if (students[i].ID > students[j].ID) // смена знака
                        {
                            Student temp = students[i];
                            students[i] = students[j];
                            students[j] = temp;
                        }
                    }
                }

            }
            public static Student[] Expel(ref Student[] students)
            {
                var expelled = students.Where(s => s.IsExpelled).ToArray();
                students = students.Where(s => !s.IsExpelled).ToArray();

                return expelled;
            }


            public static void Restore(ref Student[] students, Student restored)
            {
                if (students.Any(s => s.ID == restored.ID))
                {
                    Console.WriteLine("Студент уже есть в списке. Дублирования не произошло");
                    return;
                }

                restored.Restore(); //Вызов Restore из Student (с проверкой ID и сбросом статуса)

                Student[] newStudentArray = new Student[students.Length + 1];
                bool inserted = false;
                int index = 0;

                for (int i = 0; i < newStudentArray.Length; i++)
                {
                    if (!inserted && (index >= students.Length || restored.ID < students[index].ID))
                    {
                        newStudentArray[i] = restored;
                        inserted = true;
                    }
                    else
                    {
                        newStudentArray[i] = students[index];
                        index++;
                    }
                }
                students = newStudentArray;
            }
        }
    }
}
