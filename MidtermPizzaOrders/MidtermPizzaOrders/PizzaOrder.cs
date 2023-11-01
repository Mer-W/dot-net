using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidtermPizzaOrders
{
    public class PizzaOrder
    {

        public PizzaOrder() { }

        public PizzaOrder(string clientName, string clientPostalCode, DateTime deliveryDeadline, 
            SizeEnum size, int bakingTimeMinutes, string extras)
        {
            ClientName = clientName;
            ClientPostalCode = clientPostalCode;
            DeliveryDeadline = deliveryDeadline;
            Size = size;
            BakingTimeMinutes = bakingTimeMinutes;
            Extras = extras;
            OrderStatus = OrderStatusEnum.Placed;
            // Status = (StatusEnum)Enum.Parse(typeof(StatusEnum),status);
        }

        public int Id { get; set; }

        private string _clientName;
        [Required]
        [StringLength(100)]
        public string ClientName // 1-100 characters
        {
            get
            {
                return _clientName;
            }
            set
            {

                if (value.Length < 1 || value.Length > 100)
                {
                    throw new ArgumentException("Client name length must be 1-100 characters long");
                }
                _clientName = value;
            }
        }
        private string _clientPostalCode;

        [Required]
        [StringLength(7)]
       public  string ClientPostalCode // regexp verified A1A 1A1 format
        {
            get
            {
                return _clientPostalCode;
            }
            set
            {
                // Regex.IsMatch(task, @"^([a-zA-Z0-9\./,;\-+()*!'""\s])+$")
                // ^[^;]{1,100}$
                if (value.Length != 7)
                {
                    throw new ArgumentException("Postal code length must be 7 characters long");
                }
                _clientPostalCode = value;
            }
        }

        private DateTime _deliveryDeadline;


        [Required]
        public DateTime DeliveryDeadline // UI must verify it's at least 45 minutes past current date/time, otherwise order can't be placed
        {
            get
            {
                return _deliveryDeadline;
            }
            set
            {
                if (value.Year < 1900 || value.Year > 2099)
                {
                    throw new ArgumentException("Invalid year. Must be between 1900-2099.");
                }
                _deliveryDeadline = value;
            }
        }

        public enum SizeEnum { Small = 3, Medium = 7, Large = 12 };

        private SizeEnum _size;
        [Required]
        [EnumDataType(typeof(SizeEnum))]
       public  SizeEnum Size { get; set; } // use ComboBox with Tags to match integer values of the enum

        private int _bakingTimeMinutes;
        [Required]
        public int BakingTimeMinutes
        { // 12-18, slider with label displaying current value,
            get
            {
                return _bakingTimeMinutes;
            }
            set
            {
                if (value < 12 || value > 18)
                {
                    throw new ArgumentException("Invalid baking time selection");
                }
                _bakingTimeMinutes = value;
            }
        }
        private string _extras;
        public string Extras {
            get
            {
                return _extras;
            }
            set
            {
                if (value.Split(',').Length > 3)
                {
                    throw new ArgumentException("Invalid extras selection");
                }
                _extras = value;
            }
        } // comma-separated list, e.g. "ExtraCheese,DeepDish", based on UI checkboxes

        // Rows marked as Placed are displayed in green, unless there's less than 40 minutes to the deadline then in red, Fulfilled are in white
        // You will likely need another column [NotMapped] in order to assign color via binding, perhaps similar to this (not tested):
        // https://stackoverflow.com/questions/1847359/programmatically-assigning-a-color-to-a-row-in-datagrid
        public enum OrderStatusEnum { Placed = 0, Fulfilled = 1};

        [Required]
        [EnumDataType(typeof(OrderStatusEnum))]
        public OrderStatusEnum OrderStatus { get; set; } // initially set to Placed

    }
}
