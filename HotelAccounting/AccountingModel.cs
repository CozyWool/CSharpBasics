using System;

namespace HotelAccounting;

public class AccountingModel : ModelBase
{
    private double _price;
    private int _nightsCount;
    private double _discount;
    private double _total;

    public double Price
    {
        get => _price;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException();
            }

            _price = value;
            UpdateTotal();
            Notify(nameof(Price));
        }
    }


    public int NightsCount
    {
        get => _nightsCount;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentException();
            }

            _nightsCount = value;
            Notify(nameof(NightsCount));
            UpdateTotal();
        }
    }

    public double Discount
    {
        get => _discount;
        set
        {
            if (value > 100.0)
            {
                throw new ArgumentException();
            }

            _discount = value;
            Notify(nameof(Discount));
            UpdateTotal();
        }
    }

    private void UpdateTotal()
    {
        _total = _price * _nightsCount * (1 - _discount / 100);
        Notify(nameof(Total));
    }

    public double Total
    {
        get => _total;
        set => Discount = 100 * (1 - value / (_price * _nightsCount));
    }
}