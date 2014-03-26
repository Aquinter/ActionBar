using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngHackaton //Moet maar aangepast worden voor het project
{
	public class Budget
	{
		//Class variables
		List<Payment> paymentList;
		decimal treshold;

		public Budget(decimal treshold)
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
		public void removePayment(Payment payment)
		{
			paymentList.RemoveAt(IndexOf(payment));
		}
		public void resetList()
		{
			totalAmount = 0;
			paymentList.Clear();
		}
		public bool overTreshold()
		{
			return getTotalAmount >= treshold;
		}
		public decimal getTotalAmount()
		{
			decimal total = 0.0m;
			foreach (Payment payment in paymentList)
			{
				total += payment.getAmount();
			}
			return total;
		}
	}
}