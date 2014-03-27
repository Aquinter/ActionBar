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
		string name;

		public Budget(string name, decimal treshold)
		{
			paymentList = new List<Payment>();
			this.resetList();
			this.treshold = treshold;
			this.name = name;
		}
		public Budget(string name)
		{
			paymentList = new List<Payment>();
			this.resetList();
			this.treshold = 0.0m;
			this.name = name;
		}
		public void addPayment(Payment payment)
		{
			paymentList.Add(payment);
		}
		public void removePayment(Payment payment)
		{
			paymentList.RemoveAt(paymentList.IndexOf(payment));
		}
		public void resetList()
		{
			paymentList.Clear();
		}
		public bool overTreshold()
		{
			return treshold==0.0m ? false : getTotalAmount() >= treshold;
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
		public string getName()
		{
			return name;
		}
		public void setName(string name)
		{
			this.name = name;
		}
	}
}