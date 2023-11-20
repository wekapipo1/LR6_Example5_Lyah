using System;
public class Car
{
    string nomer;
    public string Nomer
    {
        get { return nomer; }
    }
    public Car(string nomer)
    {
        this.nomer = nomer;
    }
}
public class Sequrity
{
    string name; //поле ім'я
    //властивість
    public string Name
    {
        get { return name; }
    }
    //конструктор
    public Sequrity(string name)
    {
        this.name = name;
    }
    //обробник події NotPlaces
    public void CloseParking()
    {
        Console.ForegroundColor = System.ConsoleColor.Blue;
        Console.WriteLine("Мiсць немає. Охоронець {0} закрив стоянку", name);
        //відписуємось від статичної події
        Parking.NotPlaces-= this.CloseParking;
    }
}
public class Police
{
    string name;
    public string Name
    {
        get { return name; }
    }
    public Police(string name)
    {
        this.name = name;
    }
    public void VideoSwitchOn()
    {
        Console.ForegroundColor= System.ConsoleColor.Magenta;
        Console.WriteLine("Полiцейський {0} включив вiдеоспостереження", name);
        //відписуємось від події
        Parking.NotPlaces -= this.VideoSwitchOn;
    }
    public void DroveOutAddress(int t)
    {
        Console.ForegroundColor=System.ConsoleColor.Magenta;
        Console.WriteLine("Спрацювала сигналiзацiя {0} раз", t);
        Console.WriteLine("Полiцейський {0} приїхав на стоянку", name);
    }
}
public class Parking
{
    //делегат, відповідний події SignalTriggered
    public delegate void SignalTriggeredEventHandler(int k);
    //подія SignalTriggered
    public static event SignalTriggeredEventHandler SignalTriggered;
    //делегат, відповідний події NotPlaces
    public delegate void NotPlacesEventHandler();
    //подія NotPlaces
    public static event NotPlacesEventHandler NotPlaces;
    //поля класу
    bool therePlaces; //міста є
    string adr; //адреса
    int AllPlaces; //кількість місць
    List<Car> cars; //колекція машин
    int t; //лічильник включень сигналізацій
    //властивість
    public bool TherePlaces
    {
        get { return therePlaces; }
    }
    //конструктор
    public Parking(string adr, int AllPlaces)
    {
        this.adr = adr; this.AllPlaces = AllPlaces;
        cars= new List<Car>(0);
        this.therePlaces = true; t = 0;
    }
    public void PushCar(Car car)
    {
        Random o= new Random();
        if ((NotPlaces!=null)&&cars.Count>AllPlaces-1 ) 
        {
            NotPlaces(); //подія відбулась
            therePlaces = false; //змінили поле місць немає
        }
        else
        {
            //додaємо машину
            cars.Add(car);
            Console.ForegroundColor = System.ConsoleColor.Green;
            Console.WriteLine("На стоянку прибула " +car.Nomer);
            int x = o.Next(1, 8);
            if(x==1) //випадково
            {
                t++;
                //відбулась подія: cпрацювала сигналізація
                SignalTriggered(t);
            }
        }
    }
}
class Program
{
    static void Main(string[] args)
    {
        //створюємо об'єкти
        Parking parking = new Parking("вул.Шевченка", 10);
        Sequrity sequrityMan = new Sequrity("Микола");
        Police polisMan = new Police("Олександр");
        //підписка на статисні події
        //клас, в якому відбувається подія
        //подія += об'єкт класу, який обробляє подію, метод обробник
        Parking.NotPlaces += sequrityMan.CloseParking;
        Parking.NotPlaces += polisMan.VideoSwitchOn;
        Parking.SignalTriggered += polisMan.DroveOutAddress;
        int i = 1;
        while (parking.TherePlaces)
        {
            Car c = new Car("машина " + i);
            parking.PushCar(c);
            i++;
        }
        Console.ReadKey();
    }
}