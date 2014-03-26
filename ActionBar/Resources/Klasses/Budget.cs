using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction;

namespace IngHackaton //Moet maar aangepast worden voor het project
{
	public class Budget
	{
		//Class variables
		List<Payment> paymentList;
		Decimal treshold;
		Decimal totalAmount; //Should be resetted once in eg a month


		public Budget(Decimal treshold)
		{
			paymentList = new List<Payment>();
			this.resetTotalAmount();
			this.treshold = treshold;
		}
		public void addPayment(Payment payment)
		{
			paymentList.add(payment);
			totalAmount += payment.getAmount();
		}
		public void resetTotalAmount()
		{
			totalAmount = 0;
		}
		public bool overTreshold()
		{
			return totalAmount >= treshold;
		}

	}
}