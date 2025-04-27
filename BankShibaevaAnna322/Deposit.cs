using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankShibaevaAnna322
{
    public class Deposit
    {
        public int Id { get; set; }
        public string NameOfDeposit { get; set; }
        public decimal Amount { get; set; }
        public double InterestRate { get; set; }
        public int Duration { get; set; } // в месяцах
        public string Description { get; set; }
    }
}
