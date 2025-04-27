using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankShibaevaAnna322
{
    public class Credit
    {
        public int Id { get; set; }
        public string NameOfLoan { get; set; }
        public decimal Amount { get; set; }
        public double InterestRate { get; set; }
        public int Duration { get; set; }  
        public string Description { get; set; }
    }
}
