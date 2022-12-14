using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace BestBy {

    [Table("product")]
    public class Product {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string ShortDate { get { return ExpirationDate.ToString("MM/dd/yyyy"); } }
        public string LabelColor { 
            get {
                int daysLeft = (ExpirationDate - DateTime.Now).Days;
                string result;
                if (daysLeft > 7)
                {
                    result = "Green";
                }
                else if (daysLeft >= 0)
                {
                    result = "DarkOrange";
                }
                else
                {
                    result = "Red";
                }
                return result; 
            } 
        }
        public string ExpirationLabel
        {
            get
            {
                int daysLeft = (ExpirationDate - DateTime.Now).Days;
                int monthsLeft = daysLeft / 30;
                if (monthsLeft > 1)
                {
                    return $"Expires in {monthsLeft} months";
                } else if (monthsLeft == 1)
                {
                    return $"Expires in {monthsLeft} month";
                } else if (daysLeft > 1)
                {
                    return $"Expires in {daysLeft} days";
                } else if (daysLeft == 1)
                {
                    return $"Expires in {daysLeft} day";
                }
                else if (daysLeft == 0)
                {
                    return $"Expires today";
                } else
                {
                    return $"Expired on {ShortDate}";
                }
                
            }
        }
        public override string ToString() {
            return string.Format("{0} {1} {2} ({3})", Name, ExpirationDate.ToString("MM/dd/yyyy"), Category, Id);
        }
    }
}
