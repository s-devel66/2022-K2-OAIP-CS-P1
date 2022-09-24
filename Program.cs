// я решил немного упороться и сделать расширяемый (?) калькулятор с помощью паттерна "Стратегия".

// почему я не могу сделать так:
// interface IMathAction {
//     double calc(double x, double y);
//     double calc(double x);
// }
// используя перенагрузку (overload)?

interface IBinaryOP {
    double calc(double x, double y);
}

interface IUnaryOP {
    double calc(double x);
}

class Plus: IBinaryOP {
    public double calc(double x, double y) {
        return x + y;
    }
}

class Minus: IBinaryOP {
    public double calc(double x, double y) {
        return x - y;
    }
}

class Multiply: IBinaryOP {
    public double calc(double x, double y) {
        return x * y;
    }
}

class Divide: IBinaryOP {
    public double calc(double x, double y) {
        return x / y;
    }
}

class PowerOf: IBinaryOP {
    double res = 1;
    public double calc(double x, double n) {
        for (int i = 0; i < n; i++) {
            this.res *= x;
        }
        return res;
    }
}

class Sqrt: IUnaryOP {
    public double calc(double x) {
        // х\з как реализовать; слава стандартной библиотеке!
        return Math.Sqrt(x);
    }
}

class Percentage: IUnaryOP {
    public double calc(double x) {
        return x / 100;
    }
}

class Factr: IUnaryOP {
    public double calc(double n) {
        if (n > 1) {
            n *= this.calc(n - 1);
            return n;
        } else {
            return n;
        }
    }
}

class Calculator {
    public double calc(IBinaryOP op, double x, double y) {
        return op.calc(x, y);
    }
    public double calc(IUnaryOP op, double x) {
        return op.calc(x);
    }
}

class OpProcessor {
    string useage;
    Calculator calclr;
    public Dictionary<int, IBinaryOP> biOPMap;
    public Dictionary<int, IUnaryOP> unOPMap;

    public OpProcessor() {
        useage = @"Простейший калькулятор со следующими действиями:
1. Сложить 2 числа.
2. Вычесть 1е из 2го.
3. Перемножить 2 числа.
4. Разделить 1е на 2е.
5. Возвести в степень N 1е.
6. Найти кв. корень числа.
7. Найти 1% от числа.
8. Найти факториал из числа.
-1. Выйти из программы.";
        calclr = new Calculator();

        // заполнение biOPMap и unOPMap:
        biOPMap = new Dictionary<int, IBinaryOP>();
        biOPMap[1] = new Plus();
        biOPMap[2] = new Minus();
        biOPMap[3] = new Multiply();
        biOPMap[4] = new Divide();
        biOPMap[5] = new PowerOf();
        unOPMap = new Dictionary<int, IUnaryOP>();
        unOPMap[6] = new Sqrt();
        unOPMap[7] = new Percentage();
        unOPMap[8] = new Factr();
    }
    public void printUseage() {
        Console.WriteLine(this.useage);
    }
    public double process(int actionNum, double x, double y) {
        return calclr.calc(biOPMap[actionNum], x, y);
    }
    public double process(int actionNum, double x) {
        return calclr.calc(unOPMap[actionNum], x);
    }
}

class EntryPoint
{
    public static void Main()
    {
        OpProcessor OPP = new OpProcessor();
        int actionNum = 0;
        while (true) {
            OPP.printUseage();
            Console.Write("Номер действия: ");
            try {
                actionNum = Convert.ToInt32(Console.ReadLine());
            } catch {
                Console.WriteLine("Вы ввели какую-то ересь. Введите целочисленное число.");
                Console.WriteLine("Нажмите любую кнопку, чтобы продолжить");
                Console.ReadKey();
                Console.Clear();
                continue;
            }
            if (actionNum == -1) {
                break;
            }

            int x = 0, y = 0;
            Console.Write("X = ");
            try {
                x = Convert.ToInt32(Console.ReadLine());
            } catch {
                Console.WriteLine("Вы ввели какую-то ересь.");
                Console.WriteLine("Нажмите любую кнопку, чтобы продолжить");
                Console.ReadKey();
                Console.Clear();
                continue;
            }
            if (OPP.biOPMap.ContainsKey(actionNum)) {
                Console.Write("Y = ");
                try {
                    y = Convert.ToInt32(Console.ReadLine());
                } catch {
                    Console.WriteLine("Вы ввели какую-то ересь.");
                    Console.WriteLine("Нажмите любую кнопку, чтобы продолжить");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                Console.WriteLine("Результат: {0}", OPP.process(actionNum, x, y));
            } else if (OPP.unOPMap.ContainsKey(actionNum)) {
                Console.WriteLine("Результат: {0}", OPP.process(actionNum, x));
            } else {
                Console.WriteLine("Нет такой операции.");
            }

            Console.WriteLine("Нажмите любую кнопку, чтобы продолжить");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
