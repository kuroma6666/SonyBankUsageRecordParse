﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonyBankUsageRecordParse.src.subsystems.Transactions
{
	public class ExpenseTransaction
	{
		public String StoreName { get; set; }
		public Decimal Amount { get; set; }
		public String ExpenseCategory { get; set; }

	}

	public class ExpenseStatisticsTransaction
	{
		public String ExpenseCategory { get; set; }
		public Decimal TotalAmount { get; set; }
	}
}