using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TaxCalculator
{
    internal class Controller
    {
        public int salary;
        public int area;
        public int dp;
        public int kind;

        static int peopleDependOn = 4400000;
        static int reduceFamilySituation = 11000000;
        static double BHXH = 8;
        static double BHYT = 1.5;
        static double BHTN = 1;
        int[] taxTNCN = new int[7] {0, 5000000, 10000000, 18000000, 32000000, 52000000, 80000000} ;

        int[] taxRate = new int[7] {5, 10, 15, 20, 25, 30, 35};

        public void calculator(int kind, int salary, int area, int dp)
        {
            double EBIT, salaryNet, salaryGross;
            switch (kind)
            {
                case 1:
                    EBIT = salary * (100 - BHXH - BHYT - BHTN) / 100;
                    salaryNet = EBIT - TaxTNCN(EBIT, dp);
                    Console.WriteLine("Net salary will be : " + salaryNet);
                    break;
                case 2:
                    salaryGross = GrossSalary(salary, dp) / ((100 - BHXH - BHYT - BHTN) / 100);
                    Console.WriteLine("Gross salary will be : " + salaryGross);
                    break;
            }
        }

        double TaxTNCN(double value, int depend) 
        {
            value = value - peopleDependOn * depend - reduceFamilySituation;
            double personalIncome = 0;
            for(int i = 1;i<7;i++)
            {
                if (value > taxTNCN[i - 1] && value <= taxTNCN[i])
                {
                    personalIncome += (value - taxTNCN[i - 1]) * taxRate[i - 1] / 100;
                    return personalIncome;
                }
                else if (value > taxTNCN[i])
                {
                    personalIncome += (taxTNCN[i] - taxTNCN[i - 1]) * taxRate[i - 1] / 100;
                }
            }
            return 0;
        }

        double GrossSalary(int salary, int dp)
        {
            int count = 1;
            double totalTaxTNCN = 0, salaryBeforeTax = 0;
            while (count <7)
            {
                if (salary - reduceFamilySituation < taxRate[1])
                {
                    salaryBeforeTax = ((salary - (reduceFamilySituation + peopleDependOn * dp) * taxRate[count] / 100) * 100) / ((100 - taxRate[count]) % 100);
                }
                else
                {
                    totalTaxTNCN = totalTaxTNCN + (taxTNCN[count] - taxTNCN[count - 1]) * taxRate[count - 1] / 100;
                    salaryBeforeTax = ((salary + totalTaxTNCN - (reduceFamilySituation + peopleDependOn * dp + taxTNCN[count]) * taxRate[count] / 100) * 100) / ((100 - taxRate[count]) % 100);
                }                    
                Console.WriteLine(salaryBeforeTax+ " + " + totalTaxTNCN);
                if ((salaryBeforeTax - TaxTNCN(salaryBeforeTax, dp)) == salary)
                {
                    return salaryBeforeTax;
                }
                else
                {
                    count++;
                }

            }
            return 0;
        }

        public void InputInformation()
        {
            Console.WriteLine("====<Program Calculate Salary Before And After Tax>====");
            while (true)
            {
                Console.WriteLine("Input salary: ");
                salary = EnterNumber();

                Console.WriteLine("Input area: ");
                area = EnterNumber();

                Console.WriteLine("Input people depend on: ");
                dp = EnterNumber();

                Console.WriteLine("Input kind of salary");
                Console.WriteLine("1. From gross to net");
                Console.WriteLine("2. From net to gross");
                kind = EnterNumber();

                calculator(kind, salary, area, dp);

                Console.WriteLine("Do you want to try again?(Yes/No): ");
                String choose = Console.ReadLine();
                if (choose.ToLower().Equals("no")) { break; }
            }         
        }

        int EnterNumber()
        {
            Regex regex = new Regex("^[0-9]+$");

            while (true)
            {
                string value = Console.ReadLine();
                if (regex.IsMatch(value))
                {
                    return int.Parse(value);
                }
                else
                {
                    Console.WriteLine("Invalid input. Try again: ");
                }
            }
        }
    }
}
