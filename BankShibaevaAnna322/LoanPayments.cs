//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BankShibaevaAnna322
{
    using System;
    using System.Collections.Generic;
    
    public partial class LoanPayments
    {
        public int PaymentID { get; set; }
        public Nullable<int> LoanID { get; set; }
        public Nullable<int> ClientID { get; set; }
        public Nullable<int> PaymentAmount { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
    }
}
